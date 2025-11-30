#if !ASYNC_WITH_CUSTOM && !WITH_CUSTOM

using System;
using System.Collections.Generic;
using System.Text;
using CSharp_AUDALF;

using System.Diagnostics.CodeAnalysis;

namespace CSCommonSecrets;

/// <summary>
/// HistorySecret stores one encrypted history
/// </summary>
public sealed partial class HistorySecret
{
	/// <summary>
	/// Default constructor for HistorySecret
	/// </summary>
	/// <param name="history">History to encrypt</param>
	/// <param name="keyIdentifier">Key identifier</param>
	/// <param name="algorithm">Symmetric Key Algorithm used for encryption</param>
	/// <param name="derivedPassword">Derived password</param>
	[SetsRequiredMembers]
	public HistorySecret(History history, string keyIdentifier, SymmetricKeyAlgorithm algorithm, byte[] derivedPassword)
	{
		Dictionary<string, object> dictionaryForAUDALF = new Dictionary<string, object>()
		{
			{ History.eventTypeKey, history.eventType },
			{ History.descriptionTextKey, history.descriptionText },
			{ History.occurenceTimeKey, DateTimeOffset.FromUnixTimeSeconds(history.occurenceTime) },
		};

		this.keyIdentifier = Encoding.UTF8.GetBytes(keyIdentifier);

		this.algorithm = algorithm;

		// Create AUDALF payload from dictionary
		byte[] serializedBytes = AUDALF_Serialize.Serialize(dictionaryForAUDALF, valueTypes: null, serializationSettings: serializationSettings);

		// Encrypt the AUDALF payload with given algorithm
		this.audalfData = algorithm.EncryptBytes(serializedBytes, derivedPassword);

		// Calculate new checksum
		this.CalculateAndUpdateChecksum();
	}

	/// <summary>
	/// Constructor for custom dictionary, use this only if you know what you are doing
	/// </summary>
	/// <param name="historyAsDictionary">Dictionary containing history keys and values</param>
	/// <param name="keyIdentifier">Key identifier</param>
	/// <param name="algorithm">Symmetric Key Algorithm used for encryption</param>
	/// <param name="derivedPassword">Derived password</param>
	[SetsRequiredMembers]
	public HistorySecret(Dictionary<string, object> historyAsDictionary, string keyIdentifier, SymmetricKeyAlgorithm algorithm, byte[] derivedPassword)
	{
		this.keyIdentifier = Encoding.UTF8.GetBytes(keyIdentifier);

		this.algorithm = algorithm;

		// Create AUDALF payload from dictionary
		byte[] serializedBytes = AUDALF_Serialize.Serialize(historyAsDictionary, valueTypes: null, serializationSettings: serializationSettings);

		// Encrypt the AUDALF payload with given algorithm
		this.audalfData = algorithm.EncryptBytes(serializedBytes, derivedPassword);

		// Calculate new checksum
		this.CalculateAndUpdateChecksum();
	}

	#region Common getters

	/// <summary>
	/// Get History. Use this for situation where you want to convert secret -> non secret
	/// </summary>
	/// <param name="derivedPassword">Derived password</param>
	/// <returns>History</returns>
	public History GetHistory(byte[] derivedPassword)
	{
		Dictionary<string, object> dict = this.GetHistoryAsDictionary(derivedPassword);
		return new History()
        {
            eventType = (string)dict[History.eventTypeKey],
			descriptionText = (byte[])dict[History.descriptionTextKey],
			occurenceTime = ((DateTimeOffset)dict[History.occurenceTimeKey]).ToUnixTimeSeconds()
        };
	}

	/// <summary>
	/// Get event type
	/// </summary>
	/// <param name="derivedPassword">Derived password</param>
	/// <returns>Event type</returns>
	public HistoryEventType GetEventType(byte[] derivedPassword)
	{
		string temp = (string)Helpers.GetSingleValue(this.audalfData, this.algorithm, derivedPassword, History.eventTypeKey, deserializationSettings);

		return History.StringToHistoryEventType(temp);
	}

	/// <summary>
	/// Get description text
	/// </summary>
	/// <param name="derivedPassword">Derived password</param>
	/// <returns>Description text as string</returns>
	public string GetDescription(byte[] derivedPassword)
	{
		return Encoding.UTF8.GetString((byte[])Helpers.GetSingleValue(this.audalfData, this.algorithm, derivedPassword, History.descriptionTextKey, deserializationSettings));
	}

	/// <summary>
	/// Get history entry occurence time
	/// </summary>
	/// <param name="derivedPassword">Derived password</param>
	/// <returns>History entry occurence time as DateTimeOffset</returns>
	public DateTimeOffset GetOccurenceTime(byte[] derivedPassword)
	{
		return (DateTimeOffset)Helpers.GetSingleValue(this.audalfData, this.algorithm, derivedPassword, History.occurenceTimeKey, deserializationSettings);
	}

	private Dictionary<string, object> GetHistoryAsDictionary(byte[] derivedPassword)
	{
		var passwordCheck = Helpers.CheckDerivedPassword(derivedPassword);

		if (!passwordCheck.valid)
		{
			throw passwordCheck.exception;
		}

		// Try to decrypt the binary
		byte[] decryptedAUDALF = algorithm.DecryptBytes(this.audalfData, derivedPassword);

		var audalfCheck = Helpers.CheckAUDALFbytes(decryptedAUDALF);

		if (!audalfCheck.valid)
		{
			throw audalfCheck.exception;
		}

		Dictionary<string, object> historyAsDictionary = AUDALF_Deserialize.Deserialize<string, object>(decryptedAUDALF, settings: deserializationSettings);

		return historyAsDictionary;
	}

	/// <summary>
	/// Can the content be decrypted with given derived password
	/// </summary>
	/// <param name="derivedPassword">Derived password</param>
	/// <returns>True if can be; False otherwise</returns>
	public bool CanBeDecryptedWithDerivedPassword(byte[] derivedPassword)
	{
		var passwordCheck = Helpers.CheckDerivedPassword(derivedPassword);

		if (!passwordCheck.valid)
		{
			return false;
		}

		// Try to decrypt the binary
		byte[] decryptedAUDALF = algorithm.DecryptBytes(this.audalfData, derivedPassword);

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
	/// <returns>True if set was success; False otherwise</returns>
	public bool SetEventType(HistoryEventType eventType, byte[] derivedPassword)
	{
		return this.GenericSet(History.eventTypeKey, eventType.ToString(), derivedPassword);
	}

	/// <summary>
	/// Set description
	/// </summary>
	/// <param name="newDescription">New description</param>
	/// <param name="derivedPassword">Derived password</param>
	/// <returns>True if set was success; False otherwise</returns>
	public bool SetDescription(string newDescription, byte[] derivedPassword)
	{
		return this.GenericSet(History.descriptionTextKey, Encoding.UTF8.GetBytes(newDescription), derivedPassword);
	}

	private bool GenericSet(string key, object value, byte[] derivedPassword)
	{
		try
		{
			Dictionary<string, object> historyAsDictionary = this.GetHistoryAsDictionary(derivedPassword);
			// Update wanted value
			historyAsDictionary[key] = value;

			// Generate new algorithm since data has changed
			this.algorithm = SymmetricKeyAlgorithm.GenerateNew(this.algorithm.GetSymmetricEncryptionAlgorithm());

			// Create AUDALF payload from dictionary
			byte[] serializedBytes = AUDALF_Serialize.Serialize(historyAsDictionary, valueTypes: null, serializationSettings: serializationSettings);

			// Encrypt the AUDALF payload with given algorithm
			this.audalfData = algorithm.EncryptBytes(serializedBytes, derivedPassword);

			// Calculate new checksum
			this.CalculateAndUpdateChecksum();

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
	/// Check if checksum matches content
	/// </summary>
	/// <returns>True if matches; False otherwise</returns>
	public bool CheckIfChecksumMatchesContent()
	{
		return checksum == this.CalculateHexChecksum();
	}

	private string CalculateHexChecksum()
	{
		return ChecksumHelper.CalculateHexChecksum(this.keyIdentifier, this.audalfData, algorithm.GetSettingsAsBytes());
	}

	private void CalculateAndUpdateChecksum()
	{
		this.checksum = this.CalculateHexChecksum();
	}

	#endregion // Checksum
}

#endif // !ASYNC_WITH_CUSTOM && !WITH_CUSTOM