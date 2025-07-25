#if !ASYNC_WITH_CUSTOM && !WITH_CUSTOM

using System;
using System.Collections.Generic;
using System.Text;
using CSharp_AUDALF;

using System.Diagnostics.CodeAnalysis;

namespace CSCommonSecrets;

/// <summary>
/// NoteSecret stores one encrypted note. Note is basically a text file
/// </summary>
public sealed partial class NoteSecret
{
	/// <summary>
	/// Default constructor for NoteSecret
	/// </summary>
	/// <param name="note">Note to encrypt</param>
	/// <param name="keyIdentifier">Key identifier</param>
	/// <param name="algorithm">Symmetric Key Algorithm used for encryption</param>
	/// <param name="derivedPassword">Derived password</param>
	[SetsRequiredMembers]
	public NoteSecret(Note note, string keyIdentifier, SymmetricKeyAlgorithm algorithm, byte[] derivedPassword)
	{
		Dictionary<string, object> dictionaryForAUDALF = new Dictionary<string, object>()
		{
			{ Note.noteTitleKey, note.GetNoteTitle() },
			{ Note.noteTextKey, note.GetNoteText() },
			{ Note.creationTimeKey, DateTimeOffset.FromUnixTimeSeconds(note.creationTime) },
			{ Note.modificationTimeKey, DateTimeOffset.FromUnixTimeSeconds(note.modificationTime) },
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
	/// <param name="noteAsDictionary">Dictionary containing note keys and values</param>
	/// <param name="keyIdentifier">Key identifier</param>
	/// <param name="algorithm">Symmetric Key Algorithm used for encryption</param>
	/// <param name="derivedPassword">Derived password</param>
	[SetsRequiredMembers]
	public NoteSecret(Dictionary<string, object> noteAsDictionary, string keyIdentifier, SymmetricKeyAlgorithm algorithm, byte[] derivedPassword)
	{
		this.keyIdentifier = Encoding.UTF8.GetBytes(keyIdentifier);

		this.algorithm = algorithm;

		// Create AUDALF payload from dictionary
		byte[] serializedBytes = AUDALF_Serialize.Serialize(noteAsDictionary, valueTypes: null, serializationSettings: serializationSettings);

		// Encrypt the AUDALF payload with given algorithm
		this.audalfData = algorithm.EncryptBytes(serializedBytes, derivedPassword);

		// Calculate new checksum
		this.CalculateAndUpdateChecksum();
	}

	#region Common getters

	/// <summary>
	/// Get Note. Use this for situation where you want to convert secret -> non secret
	/// </summary>
	/// <param name="derivedPassword">Derived password</param>
	/// <returns>Note</returns>
	public Note GetNote(byte[] derivedPassword)
	{
		Dictionary<string, object> dict = this.GetNoteAsDictionary(derivedPassword);
		Note returnValue = new Note((string)dict[Note.noteTitleKey], (string)dict[Note.noteTextKey]);

		returnValue.creationTime = ((DateTimeOffset)dict[LoginInformation.creationTimeKey]).ToUnixTimeSeconds();
		returnValue.modificationTime = ((DateTimeOffset)dict[LoginInformation.modificationTimeKey]).ToUnixTimeSeconds();

		return returnValue;
	}

	/// <summary>
	/// Get note title
	/// </summary>
	/// <param name="derivedPassword">Derived password</param>
	/// <returns>Note title as string</returns>
	public string GetNoteTitle(byte[] derivedPassword)
	{
		return (string)Helpers.GetSingleValue(this.audalfData, this.algorithm, derivedPassword, Note.noteTitleKey, deserializationSettings);
	}

	/// <summary>
	/// Get note text
	/// </summary>
	/// <param name="derivedPassword">Derived password</param>
	/// <returns>Note text as string</returns>
	public string GetNoteText(byte[] derivedPassword)
	{
		return (string)Helpers.GetSingleValue(this.audalfData, this.algorithm, derivedPassword, Note.noteTextKey, deserializationSettings);
	}

	/// <summary>
	/// Get note creation time
	/// </summary>
	/// <param name="derivedPassword">Derived password</param>
	/// <returns>Note creation time as DateTimeOffset</returns>
	public DateTimeOffset GetCreationTime(byte[] derivedPassword)
	{
		return (DateTimeOffset)Helpers.GetSingleValue(this.audalfData, this.algorithm, derivedPassword, Note.creationTimeKey, deserializationSettings);
	}

	/// <summary>
	/// Get note modification time
	/// </summary>
	/// <param name="derivedPassword">Derived password</param>
	/// <returns>Note modification time as DateTimeOffset</returns>
	public DateTimeOffset GetModificationTime(byte[] derivedPassword)
	{
		return (DateTimeOffset)Helpers.GetSingleValue(this.audalfData, this.algorithm, derivedPassword, Note.modificationTimeKey, deserializationSettings);
	}

	private Dictionary<string, object> GetNoteAsDictionary(byte[] derivedPassword)
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

		Dictionary<string, object> noteAsDictionary = AUDALF_Deserialize.Deserialize<string, object>(decryptedAUDALF, settings: deserializationSettings);

		return noteAsDictionary;
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
	/// Set note title
	/// </summary>
	/// <param name="newNoteTitle">New title</param>
	/// <param name="derivedPassword">Derived password</param>
	/// <returns>True if set was success; False otherwise</returns>
	public bool SetNoteTitle(string newNoteTitle, byte[] derivedPassword)
	{
		return this.GenericSet(Note.noteTitleKey, newNoteTitle, DateTimeOffset.UtcNow, derivedPassword);
	}

	/// <summary>
	/// Set note text
	/// </summary>
	/// <param name="newNoteText">New text</param>
	/// <param name="derivedPassword">Derived password</param>
	/// <returns>True if set was success; False otherwise</returns>
	public bool SetNoteText(string newNoteText, byte[] derivedPassword)
	{
		return this.GenericSet(Note.noteTextKey, newNoteText, DateTimeOffset.UtcNow, derivedPassword);
	}

	private bool GenericSet(string key, object value, DateTimeOffset modificationTime, byte[] derivedPassword)
	{
		try
		{
			Dictionary<string, object> noteAsDictionary = this.GetNoteAsDictionary(derivedPassword);
			// Update wanted value
			noteAsDictionary[key] = value;
			// Update modification time
			noteAsDictionary[Note.modificationTimeKey] = modificationTime;

			// Generate new algorithm since data has changed
			this.algorithm = SymmetricKeyAlgorithm.GenerateNew(this.algorithm.GetSymmetricEncryptionAlgorithm());

			// Create AUDALF payload from dictionary
			byte[] serializedBytes = AUDALF_Serialize.Serialize(noteAsDictionary, valueTypes: null, serializationSettings: serializationSettings);

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