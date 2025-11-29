#if ASYNC_WITH_CUSTOM

using System;
using System.Text;

using System.Threading.Tasks;

namespace CSCommonSecrets;

/// <summary>
/// History stores one plaintext (anyone can read) Common Secrets modification entry
/// </summary>
public sealed partial class History
{
	/// <summary>
	/// Create history entry, async
	/// </summary>
	/// <param name="eventType">Type of event</param>
	/// <param name="description">Description of event</param>
	/// <param name="securityFunctions">Security functions</param>
	/// <returns>History</returns>
	public static async Task<History> CreateHistoryAsync(HistoryEventType eventType, string description, ISecurityAsyncFunctions securityFunctions)
	{
		return await CreateHistoryAsync(eventType, description, DateTimeOffset.UtcNow, securityFunctions);
	}

	/// <summary>
	/// Create history entry, async
	/// </summary>
	/// <param name="eventType">Type of event</param>
	/// <param name="description">Description of event</param>
	/// <param name="time">Creation time</param>
	/// <param name="securityFunctions">Security functions</param>
	/// <returns>History</returns>
	public static async Task<History> CreateHistoryAsync(HistoryEventType eventType, string description, DateTimeOffset time, ISecurityAsyncFunctions securityFunctions)
	{
		History history = new History() { eventType = eventType.ToString() };
		await history.UpdateHistoryAsync(eventType, description, time, securityFunctions);
		return history;
	}

	/// <summary>
	/// Update history entry, uses DateTimeOffset.UtcNow for occurence timestamp, async
	/// </summary>
	/// <param name="eventType">Type of event</param>
	/// <param name="description">Description of event</param>
	/// <param name="securityFunctions">Security functions</param>
	public async Task UpdateHistoryAsync(HistoryEventType eventType, string description, ISecurityAsyncFunctions securityFunctions)
	{
		await this.UpdateHistoryAsync(eventType, description, DateTimeOffset.UtcNow, securityFunctions);
	}

	/// <summary>
	/// Update history entry, use chosen time for occurence time, async
	/// </summary>
	/// <param name="eventType">Type of event</param>
	/// <param name="description">Description of event</param>
	/// <param name="time">Occurence time</param>
	/// <param name="securityFunctions">Security functions</param>
	public async Task UpdateHistoryAsync(HistoryEventType eventType, string description, DateTimeOffset time, ISecurityAsyncFunctions securityFunctions)
	{
		this.eventType = eventType.ToString();
		this.descriptionText = Encoding.UTF8.GetBytes(description);
		this.occurenceTime = time.ToUnixTimeSeconds();
		await this.CalculateAndUpdateChecksumAsync(securityFunctions);
	}

	#region Checksum

	/// <summary>
	/// Check if checksum matches content, async
	/// </summary>
	/// <param name="securityFunctions">Security functions</param>
	/// <returns>True if matches; False otherwise</returns>
	public async Task<bool> CheckIfChecksumMatchesContentAsync(ISecurityAsyncFunctions securityFunctions)
	{
		return checksum == await this.CalculateHexChecksumAsync(securityFunctions);
	}

	private async Task<string> CalculateHexChecksumAsync(ISecurityAsyncFunctions securityFunctions)
	{
		return await ChecksumHelper.CalculateHexChecksumAsync(securityFunctions, Encoding.UTF8.GetBytes(this.eventType), this.descriptionText, BitConverter.GetBytes(this.occurenceTime));
	}

	private async Task CalculateAndUpdateChecksumAsync(ISecurityAsyncFunctions securityFunctions)
	{
		this.checksum = await this.CalculateHexChecksumAsync(securityFunctions);
	}

	#endregion // Checksum
}

#endif // ASYNC_WITH_CUSTOM