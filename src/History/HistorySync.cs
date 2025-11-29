#if !ASYNC_WITH_CUSTOM && !WITH_CUSTOM

using System;
using System.Text;
using System.Diagnostics.CodeAnalysis;

namespace CSCommonSecrets;

/// <summary>
/// History stores one plaintext (anyone can read) Common Secrets modification entry
/// </summary>
public sealed partial class History
{
	/// <summary>
	/// Default constructor for history entry
	/// </summary>
	/// <param name="eventType">Type of event</param>
	/// <param name="description">Description of event</param>
	[SetsRequiredMembers]
	public History(HistoryEventType eventType, string description) : this (eventType, description, DateTimeOffset.UtcNow)
	{

	}

	/// <summary>
	/// Constructor with occurence time override
	/// </summary>
	/// <param name="eventType">Type of event</param>
	/// <param name="description">Description of event</param>
	/// <param name="time">Occurence time</param>
	[SetsRequiredMembers]
	public History(HistoryEventType eventType, string description, DateTimeOffset time)
	{
		this.occurenceTime = time.ToUnixTimeSeconds();
		this.UpdateHistory(eventType, description, time);
	}

	/// <summary>
	/// Update history entry, uses DateTimeOffset.UtcNow for occurence timestamp
	/// </summary>
	/// <param name="eventType">Type of event</param>
	/// <param name="description">Description of event</param>
	public void UpdateHistory(HistoryEventType eventType, string description)
	{
		this.UpdateHistory(eventType, description, DateTimeOffset.UtcNow);
	}

	/// <summary>
	/// Update history entry, use chosen time for occurence time
	/// </summary>
	/// <param name="eventType">Type of event</param>
	/// <param name="description">Description of event</param>
	/// <param name="time">Occurence time</param>
	public void UpdateHistory(HistoryEventType eventType, string description, DateTimeOffset time)
	{
		this.eventType = eventType.ToString();
		this.descriptionText = Encoding.UTF8.GetBytes(description);
		this.CalculateAndUpdateChecksum();
	}

	/// <summary>
	/// Check if checksum matches content
	/// </summary>
	/// <returns>True if matches; False otherwise</returns>
	public bool CheckIfChecksumMatchesContent()
	{
		return checksum == this.CalculateHexChecksum();
	}

	private string CalculateHexChecksum()
	{
		return ChecksumHelper.CalculateHexChecksum(Encoding.UTF8.GetBytes(this.eventType), this.descriptionText, BitConverter.GetBytes(this.occurenceTime));
	}

	private void CalculateAndUpdateChecksum()
	{
		this.checksum = this.CalculateHexChecksum();
	}
}

#endif // !ASYNC_WITH_CUSTOM && !WITH_CUSTOM