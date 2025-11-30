#if ASYNC_WITH_CUSTOM
// This file has async methods

using System;
using System.Collections.Generic;
using System.Text;
using CSharp_AUDALF;
using System.Threading.Tasks;

namespace CSCommonSecrets;

/// <summary>
/// HistorySecret stores one encrypted history event
/// </summary>
public sealed partial class HistorySecret
{
	/// <summary>
	/// Create HistorySecret, async
	/// </summary>
	/// <param name="history">History</param>
	/// <param name="keyIdentifier">Key identifier</param>
	/// <param name="algorithm">Symmetric Key Algorithm</param>
	/// <param name="derivedPassword">Derived password</param>
	/// <param name="securityFunctions">Security functions</param>
	/// <returns>HistorySecret</returns>
	public static async Task<HistorySecret> CreateHistorySecretAsync(History history, string keyIdentifier, SymmetricKeyAlgorithm algorithm, byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
	{
		Dictionary<string, object> dictionaryForAUDALF = new Dictionary<string, object>()
		{
			{ History.eventTypeKey, history.eventType },
			{ History.descriptionTextKey, history.GetDescription() },
			{ History.occurenceTimeKey, DateTimeOffset.FromUnixTimeSeconds(history.occurenceTime) },
		};

		return await CreateHistorySecretAsync(dictionaryForAUDALF, keyIdentifier, algorithm, derivedPassword, securityFunctions);
	}

	/// <summary>
	/// Create HistorySecret, async
	/// </summary>
	/// <param name="historyAsDictionary"></param>
	/// <param name="keyIdentifier">Key identifier</param>
	/// <param name="algorithm">Symmetric Key Algorithm</param>
	/// <param name="derivedPassword">Derived password</param>
	/// <param name="securityFunctions">Security functions</param>
	/// <returns>HistorySecret</returns>
	public static async Task<HistorySecret> CreateHistorySecretAsync(Dictionary<string, object> historyAsDictionary, string keyIdentifier, SymmetricKeyAlgorithm algorithm, byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
	{
		// Create AUDALF payload from dictionary
		byte[] serializedBytes = AUDALF_Serialize.Serialize(historyAsDictionary, valueTypes: null, serializationSettings: serializationSettings);

		HistorySecret HistorySecret = new HistorySecret()
		{
			keyIdentifier = Encoding.UTF8.GetBytes(keyIdentifier),
			algorithm = algorithm,
			// Encrypt the AUDALF payload with given algorithm
			audalfData = await algorithm.EncryptBytesAsync(serializedBytes, derivedPassword, securityFunctions),
		};

		// Calculate new checksum
		await HistorySecret.CalculateAndUpdateChecksumAsync(securityFunctions);

		return HistorySecret;
	}

	#region Common getters

	/// <summary>
	/// Get History. Use this for situation where you want to convert secret -> non secret, async
	/// </summary>
	/// <param name="derivedPassword">Derived password</param>
	/// <param name="securityFunctions">Security functions</param>
	/// <returns>History</returns>
	public async Task<History> GetHistoryAsync(byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
	{
		Dictionary<string, object> dict = await this.GetHistoryAsDictionaryAsync(derivedPassword, securityFunctions);

		return new History()
        {
            eventType = (string)dict[History.eventTypeKey],
			descriptionText = Encoding.UTF8.GetBytes((string)dict[History.descriptionTextKey]),
			occurenceTime = ((DateTimeOffset)dict[History.occurenceTimeKey]).ToUnixTimeSeconds()
        };
	}

	/// <summary>
	/// Get event type
	/// </summary>
	/// <param name="derivedPassword">Derived password</param>
	/// <param name="securityFunctions">Security functions</param>
	/// <returns>Event type</returns>
	public async Task<HistoryEventType> GetEventTypeAsync(byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
	{
		string temp = (string)await Helpers.GetSingleValueAsync(this.audalfData, this.algorithm, derivedPassword, History.eventTypeKey, deserializationSettings, securityFunctions);

		return History.StringToHistoryEventType(temp);
	}

	/// <summary>
	/// Get description text
	/// </summary>
	/// <param name="derivedPassword">Derived password</param>
	/// <param name="securityFunctions">Security functions</param>
	/// <returns>Description text as string</returns>
	public async Task<string> GetDescriptionAsync(byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
	{
		return (string)await Helpers.GetSingleValueAsync(this.audalfData, this.algorithm, derivedPassword, History.descriptionTextKey, deserializationSettings, securityFunctions);
	}

	/// <summary>
	/// Get history entry occurence time
	/// </summary>
	/// <param name="derivedPassword">Derived password</param>
	/// <param name="securityFunctions">Security functions</param>
	/// <returns>History entry occurence time as DateTimeOffset</returns>
	public async Task<DateTimeOffset> GetOccurenceTimeAsync(byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
	{
		return (DateTimeOffset)await Helpers.GetSingleValueAsync(this.audalfData, this.algorithm, derivedPassword, History.occurenceTimeKey, deserializationSettings, securityFunctions);
	}

	private async Task<Dictionary<string, object>> GetHistoryAsDictionaryAsync(byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
	{
		var passwordCheck = Helpers.CheckDerivedPassword(derivedPassword);

		if (!passwordCheck.valid)
		{
			throw passwordCheck.exception;
		}

		// Try to decrypt the binary
		byte[] decryptedAUDALF = await algorithm.DecryptBytesAsync(this.audalfData, derivedPassword, securityFunctions);

		var audalfCheck = Helpers.CheckAUDALFbytes(decryptedAUDALF);

		if (!audalfCheck.valid)
		{
			throw audalfCheck.exception;
		}

		Dictionary<string, object> historyAsDictionary = AUDALF_Deserialize.Deserialize<string, object>(decryptedAUDALF, settings: deserializationSettings);

		return historyAsDictionary;
	}

	/// <summary>
	/// Can the content be decrypted with given derived password, async
	/// </summary>
	/// <param name="derivedPassword">Derived password</param>
	/// <param name="securityFunctions">Security functions</param>
	/// <returns>True if can be; False otherwise</returns>
	public async Task<bool> CanBeDecryptedWithDerivedPasswordAsync(byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
	{
		var passwordCheck = Helpers.CheckDerivedPassword(derivedPassword);

		if (!passwordCheck.valid)
		{
			return false;
		}

		// Try to decrypt the binary
		byte[] decryptedAUDALF = await algorithm.DecryptBytesAsync(this.audalfData, derivedPassword, securityFunctions);

		var audalfCheck = Helpers.CheckAUDALFbytes(decryptedAUDALF);

		if (!audalfCheck.valid)
		{
			return false;
		}

		return true;
	}

	#endregion // Common getters


	#region Common setters

	/// <summary>
	/// Set history event type
	/// </summary>
	/// <param name="eventType">New history event type</param>
	/// <param name="derivedPassword">Derived password</param>
	/// <param name="securityFunctions">Security functions</param>
	/// <returns>True if set was success; False otherwise</returns>
	public async Task<bool> SetEventTypeAsync(HistoryEventType eventType, byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
	{
		return await this.GenericSetAsync(History.eventTypeKey, eventType.ToString(), derivedPassword, securityFunctions);
	}

	/// <summary>
	/// Set description
	/// </summary>
	/// <param name="newDescription">New description</param>
	/// <param name="derivedPassword">Derived password</param>
	/// <param name="securityFunctions">Security functions</param>
	/// <returns>True if set was success; False otherwise</returns>
	public async Task<bool> SetDescriptionAsync(string newDescription, byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
	{
		return await this.GenericSetAsync(History.descriptionTextKey, newDescription, derivedPassword, securityFunctions);
	}

	private async Task<bool> GenericSetAsync(string key, object value, byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
	{
		try 
		{
			Dictionary<string, object> historyAsDictionary = await this.GetHistoryAsDictionaryAsync(derivedPassword, securityFunctions);
			// Update wanted value
			historyAsDictionary[key] = value;

			// Generate new algorithm since data has changed
			this.algorithm = SymmetricKeyAlgorithm.GenerateNew(this.algorithm.GetSymmetricEncryptionAlgorithm(), securityFunctions);

			// Create AUDALF payload from dictionary
			byte[] serializedBytes = AUDALF_Serialize.Serialize(historyAsDictionary, valueTypes: null, serializationSettings: serializationSettings );

			// Encrypt the AUDALF payload with given algorithm
			this.audalfData = await algorithm.EncryptBytesAsync(serializedBytes, derivedPassword, securityFunctions);

			// Calculate new checksum
			await this.CalculateAndUpdateChecksumAsync(securityFunctions);

			return true;
		}
		catch
		{
			return false;
		}
	}

	#endregion // Common setters


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
		return await ChecksumHelper.CalculateHexChecksumAsync(securityFunctions, this.keyIdentifier, this.audalfData, algorithm.GetSettingsAsBytes());
	}

	private async Task CalculateAndUpdateChecksumAsync(ISecurityAsyncFunctions securityFunctions)
	{
		this.checksum = await this.CalculateHexChecksumAsync(securityFunctions);
	}

	#endregion // Checksum
}

#endif