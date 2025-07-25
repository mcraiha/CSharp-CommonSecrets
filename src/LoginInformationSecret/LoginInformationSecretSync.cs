#if !ASYNC_WITH_CUSTOM && !WITH_CUSTOM

using System;
using System.Collections.Generic;
using System.Text;
using CSharp_AUDALF;

using System.Diagnostics.CodeAnalysis;

namespace CSCommonSecrets;

/// <summary>
/// LoginInformationSecret stores one encrypted login information
/// </summary>
public sealed partial class LoginInformationSecret
{
	/// <summary>
	/// Default constructor for LoginInformationSecret
	/// </summary>
	/// <param name="loginInformation">LoginInformation that will be stored as secret</param>
	/// <param name="keyIdentifier">Key identifier</param>
	/// <param name="algorithm">SymmetricKeyAlgorithm used to encrypt the LoginInformation</param>
	/// <param name="derivedPassword">Derived password</param>
	[SetsRequiredMembers]
	public LoginInformationSecret(LoginInformation loginInformation, string keyIdentifier, SymmetricKeyAlgorithm algorithm, byte[] derivedPassword)
	{
		Dictionary<string, object> dictionaryForAUDALF = new Dictionary<string, object>()
		{
			{ LoginInformation.titleKey, loginInformation.GetTitle() },
			{ LoginInformation.urlKey, loginInformation.GetURL() },
			{ LoginInformation.emailKey, loginInformation.GetEmail() },
			{ LoginInformation.usernameKey, loginInformation.GetUsername() },
			{ LoginInformation.passwordKey, loginInformation.GetPassword() },
			{ LoginInformation.notesKey, loginInformation.GetNotes() },
			{ LoginInformation.mfaKey, loginInformation.GetMFA() },
			{ LoginInformation.creationTimeKey, DateTimeOffset.FromUnixTimeSeconds(loginInformation.creationTime) },
			{ LoginInformation.modificationTimeKey, DateTimeOffset.FromUnixTimeSeconds(loginInformation.modificationTime) },
			{ LoginInformation.iconKey, loginInformation.GetIcon() },
			{ LoginInformation.categoryKey, loginInformation.GetCategory() },
			{ LoginInformation.tagsKey, loginInformation.GetTags() },
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
	/// <param name="loginInformationAsDictionary">Dictionary containing login information keys and values</param>
	/// <param name="keyIdentifier">Key identifier</param>
	/// <param name="algorithm">Symmetric Key Algorithm used for encryption</param>
	/// <param name="derivedPassword">Derived password</param>
	[SetsRequiredMembers]
	public LoginInformationSecret(Dictionary<string, object> loginInformationAsDictionary, string keyIdentifier, SymmetricKeyAlgorithm algorithm, byte[] derivedPassword)
	{
		this.keyIdentifier = Encoding.UTF8.GetBytes(keyIdentifier);

		this.algorithm = algorithm;

		// Create AUDALF payload from dictionary
		byte[] serializedBytes = AUDALF_Serialize.Serialize(loginInformationAsDictionary, valueTypes: null, serializationSettings: serializationSettings);

		// Encrypt the AUDALF payload with given algorithm
		this.audalfData = algorithm.EncryptBytes(serializedBytes, derivedPassword);

		// Calculate new checksum
		this.CalculateAndUpdateChecksum();
	}

	#region Common getters

	/// <summary>
	/// Get LoginInformation. Use this for situation where you want to convert secret -> non secret
	/// </summary>
	/// <param name="derivedPassword">Derived password</param>
	/// <returns>LoginInformation</returns>
	public LoginInformation GetLoginInformation(byte[] derivedPassword)
	{
		Dictionary<string, object> dict = this.GetLoginInformationAsDictionary(derivedPassword);
		LoginInformation returnValue = new LoginInformation((string)dict[LoginInformation.titleKey], (string)dict[LoginInformation.urlKey], (string)dict[LoginInformation.emailKey],
									(string)dict[LoginInformation.usernameKey], (string)dict[LoginInformation.passwordKey], (string)dict[LoginInformation.notesKey], (string)dict[LoginInformation.mfaKey],
									(byte[])dict[LoginInformation.iconKey], (string)dict[LoginInformation.categoryKey], (string)dict[LoginInformation.tagsKey]
									);
		returnValue.creationTime = ((DateTimeOffset)dict[LoginInformation.creationTimeKey]).ToUnixTimeSeconds();
		returnValue.modificationTime = ((DateTimeOffset)dict[LoginInformation.modificationTimeKey]).ToUnixTimeSeconds();

		return returnValue;
	}

	/// <summary>
	/// Get title. This tries to decrypt data with given derived password
	/// </summary>
	/// <param name="derivedPassword">Derived password</param>
	/// <returns>Title</returns>
	public string GetTitle(byte[] derivedPassword)
	{
		return (string)Helpers.GetSingleValue(this.audalfData, this.algorithm, derivedPassword, LoginInformation.titleKey, deserializationSettings);
	}

	/// <summary>
	/// Get URL. This tries to decrypt data with given derived password
	/// </summary>
	/// <param name="derivedPassword">Derived password</param>
	/// <returns>URL</returns>
	public string GetURL(byte[] derivedPassword)
	{
		return (string)Helpers.GetSingleValue(this.audalfData, this.algorithm, derivedPassword, LoginInformation.urlKey, deserializationSettings);
	}

	/// <summary>
	/// Get email. This tries to decrypt data with given derived password
	/// </summary>
	/// <param name="derivedPassword">Derived password</param>
	/// <returns>Email</returns>
	public string GetEmail(byte[] derivedPassword)
	{
		return (string)Helpers.GetSingleValue(this.audalfData, this.algorithm, derivedPassword, LoginInformation.emailKey, deserializationSettings);
	}

	/// <summary>
	/// Get username. This tries to decrypt data with given derived password
	/// </summary>
	/// <param name="derivedPassword">Derived password</param>
	/// <returns>Username</returns>
	public string GetUsername(byte[] derivedPassword)
	{
		return (string)Helpers.GetSingleValue(this.audalfData, this.algorithm, derivedPassword, LoginInformation.usernameKey, deserializationSettings);
	}

	/// <summary>
	/// Get password. This tries to decrypt data with given derived password
	/// </summary>
	/// <param name="derivedPassword">Derived password</param>
	/// <returns>Password</returns>
	public string GetPassword(byte[] derivedPassword)
	{
		return (string)Helpers.GetSingleValue(this.audalfData, this.algorithm, derivedPassword, LoginInformation.passwordKey, deserializationSettings);
	}

	/// <summary>
	/// Get notes. This tries to decrypt data with given derived password
	/// </summary>
	/// <param name="derivedPassword">Derived password</param>
	/// <returns>Notes</returns>
	public string GetNotes(byte[] derivedPassword)
	{
		return (string)Helpers.GetSingleValue(this.audalfData, this.algorithm, derivedPassword, LoginInformation.notesKey, deserializationSettings);
	}

	/// <summary>
	/// Get MFA (e.g. TOTP Url). This tries to decrypt data with given derived password
	/// </summary>
	/// <param name="derivedPassword">Derived password</param>
	/// <returns>MFA</returns>
	public string GetMFA(byte[] derivedPassword)
	{
		return (string)Helpers.GetSingleValue(this.audalfData, this.algorithm, derivedPassword, LoginInformation.mfaKey, deserializationSettings);
	}

	/// <summary>
	/// Get creation time of Login Information secret. This tries to decrypt data with given derived password
	/// </summary>
	/// <param name="derivedPassword">Derived password</param>
	/// <returns>Creation time</returns>
	public DateTimeOffset GetCreationTime(byte[] derivedPassword)
	{
		return (DateTimeOffset)Helpers.GetSingleValue(this.audalfData, this.algorithm, derivedPassword, LoginInformation.creationTimeKey, deserializationSettings);
	}

	/// <summary>
	/// Get modification time of Login Information secret. This tries to decrypt data with given derived password
	/// </summary>
	/// <param name="derivedPassword">Derived password</param>
	/// <returns>Modification time</returns>
	public DateTimeOffset GetModificationTime(byte[] derivedPassword)
	{
		return (DateTimeOffset)Helpers.GetSingleValue(this.audalfData, this.algorithm, derivedPassword, LoginInformation.modificationTimeKey, deserializationSettings);
	}

	/// <summary>
	/// Get icon. This tries to decrypt data with given derived password
	/// </summary>
	/// <param name="derivedPassword">Derived password</param>
	/// <returns>Icon</returns>
	public byte[] GetIcon(byte[] derivedPassword)
	{
		return (byte[])Helpers.GetSingleValue(this.audalfData, this.algorithm, derivedPassword, LoginInformation.iconKey, deserializationSettings);
	}

	/// <summary>
	/// Get category. This tries to decrypt data with given derived password
	/// </summary>
	/// <param name="derivedPassword">Derived password</param>
	/// <returns>Category</returns>
	public string GetCategory(byte[] derivedPassword)
	{
		return (string)Helpers.GetSingleValue(this.audalfData, this.algorithm, derivedPassword, LoginInformation.categoryKey, deserializationSettings);
	}

	/// <summary>
	/// Get tags. This tries to decrypt data with given derived password
	/// </summary>
	/// <param name="derivedPassword">Derived password</param>
	/// <returns>Tags</returns>
	public string GetTags(byte[] derivedPassword)
	{
		return (string)Helpers.GetSingleValue(this.audalfData, this.algorithm, derivedPassword, LoginInformation.tagsKey, deserializationSettings);
	}

	private Dictionary<string, object> GetLoginInformationAsDictionary(byte[] derivedPassword)
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

		Dictionary<string, object> loginInformationAsDictionary = AUDALF_Deserialize.Deserialize<string, object>(decryptedAUDALF, settings: deserializationSettings);

		return loginInformationAsDictionary;
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
	/// Try to set new title for login information secret by decrypting the current login information secret, setting a new value and then encrypting the modified login information secret
	/// </summary>
	/// <param name="newTitle">New title</param>
	/// <param name="derivedPassword">Derived password</param>
	/// <returns>True if set goes correctly; False otherwise</returns>
	public bool SetTitle(string newTitle, byte[] derivedPassword)
	{
		return this.GenericSet(LoginInformation.titleKey, newTitle, DateTimeOffset.UtcNow, derivedPassword);
	}

	/// <summary>
	/// Try to set new URL for login information secret by decrypting the current login information secret, setting a new value and then encrypting the modified login information secret
	/// </summary>
	/// <param name="newURL">New URL</param>
	/// <param name="derivedPassword">Derived password</param>
	/// <returns>True if set goes correctly; False otherwise</returns>
	public bool SetURL(string newURL, byte[] derivedPassword)
	{
		return this.GenericSet(LoginInformation.urlKey, newURL, DateTimeOffset.UtcNow, derivedPassword);
	}

	/// <summary>
	/// Try to set new email for login information secret by decrypting the current login information secret, setting a new value and then encrypting the modified login information secret
	/// </summary>
	/// <param name="newEmail">New email</param>
	/// <param name="derivedPassword">Derived password</param>
	/// <returns>True if set goes correctly; False otherwise</returns>
	public bool SetEmail(string newEmail, byte[] derivedPassword)
	{
		return this.GenericSet(LoginInformation.emailKey, newEmail, DateTimeOffset.UtcNow, derivedPassword);
	}

	/// <summary>
	/// Try to set new username for login information secret by decrypting the current login information secret, setting a new value and then encrypting the modified login information secret
	/// </summary>
	/// <param name="newUsername">New username</param>
	/// <param name="derivedPassword">Derived password</param>
	/// <returns>True if set goes correctly; False otherwise</returns>
	public bool SetUsername(string newUsername, byte[] derivedPassword)
	{
		return this.GenericSet(LoginInformation.usernameKey, newUsername, DateTimeOffset.UtcNow, derivedPassword);
	}

	/// <summary>
	/// Try to set new password for login information secret by decrypting the current login information secret, setting a new value and then encrypting the modified login information secret
	/// </summary>
	/// <param name="newPassword">New password</param>
	/// <param name="derivedPassword">Derived password</param>
	/// <returns>True if set goes correctly; False otherwise</returns>
	public bool SetPassword(string newPassword, byte[] derivedPassword)
	{
		return this.GenericSet(LoginInformation.passwordKey, newPassword, DateTimeOffset.UtcNow, derivedPassword);
	}

	/// <summary>
	/// Try to set new notes for login information secret by decrypting the current login information secret, setting a new value and then encrypting the modified login information secret
	/// </summary>
	/// <param name="newNotes">New notes</param>
	/// <param name="derivedPassword">Derived password</param>
	/// <returns>True if set goes correctly; False otherwise</returns>
	public bool SetNotes(string newNotes, byte[] derivedPassword)
	{
		return this.GenericSet(LoginInformation.notesKey, newNotes, DateTimeOffset.UtcNow, derivedPassword);
	}

	/// <summary>
	/// Try to set new MFA for login information secret by decrypting the current login information secret, setting a new value and then encrypting the modified login information secret
	/// </summary>
	/// <param name="newMFA">New MFA</param>
	/// <param name="derivedPassword">Derived password</param>
	/// <returns>True if set goes correctly; False otherwise</returns>
	public bool SetMFA(string newMFA, byte[] derivedPassword)
	{
		return this.GenericSet(LoginInformation.mfaKey, newMFA, DateTimeOffset.UtcNow, derivedPassword);
	}

	/// <summary>
	/// Try to set new creation time for login information secret by decrypting the current login information secret, setting a new value and then encrypting the modified login information secret
	/// </summary>
	/// <param name="newCreationTime">New creation time</param>
	/// <param name="derivedPassword">Derived password</param>
	/// <returns>True if set goes correctly; False otherwise</returns>
	public bool SetCreationTime(DateTimeOffset newCreationTime, byte[] derivedPassword)
	{
		return this.GenericSet(LoginInformation.creationTimeKey, newCreationTime, DateTimeOffset.UtcNow, derivedPassword);
	}

	/// <summary>
	/// Try to set new modification time for login information secret by decrypting the current login information secret, setting a new value and then encrypting the modified login information secret
	/// </summary>
	/// <param name="newModificationTime">New modification time</param>
	/// <param name="derivedPassword">Derived password</param>
	/// <returns>True if set goes correctly; False otherwise</returns>
	public bool SetModificationTime(DateTimeOffset newModificationTime, byte[] derivedPassword)
	{
		return this.GenericSet(LoginInformation.modificationTimeKey, newModificationTime, DateTimeOffset.UtcNow, derivedPassword);
	}

	/// <summary>
	/// Try to set new icon for login information secret by decrypting the current login information secret, setting a new value and then encrypting the modified login information secret
	/// </summary>
	/// <param name="newIcon">New icon</param>
	/// <param name="derivedPassword">Derived password</param>
	/// <returns>True if set goes correctly; False otherwise</returns>
	public bool SetIcon(byte[] newIcon, byte[] derivedPassword)
	{
		return this.GenericSet(LoginInformation.iconKey, newIcon, DateTimeOffset.UtcNow, derivedPassword);
	}

	/// <summary>
	/// Try to set new category for login information secret by decrypting the current login information secret, setting a new value and then encrypting the modified login information secret
	/// </summary>
	/// <param name="newCategory">New category</param>
	/// <param name="derivedPassword">Derived password</param>
	/// <returns>True if set goes correctly; False otherwise</returns>
	public bool SetCategory(string newCategory, byte[] derivedPassword)
	{
		return this.GenericSet(LoginInformation.categoryKey, newCategory, DateTimeOffset.UtcNow, derivedPassword);
	}

	/// <summary>
	/// Try to set new tags for login information secret by decrypting the current login information secret, setting a new value and then encrypting the modified login information secret
	/// </summary>
	/// <param name="newTags">New tags</param>
	/// <param name="derivedPassword">Derived password</param>
	/// <returns>True if set goes correctly; False otherwise</returns>
	public bool SetTags(string newTags, byte[] derivedPassword)
	{
		return this.GenericSet(LoginInformation.tagsKey, newTags, DateTimeOffset.UtcNow, derivedPassword);
	}

	private bool GenericSet(string key, object value, DateTimeOffset modificationTime, byte[] derivedPassword)
	{
		try
		{
			Dictionary<string, object> loginInformationAsDictionary = this.GetLoginInformationAsDictionary(derivedPassword);
			loginInformationAsDictionary[key] = value;

			if (key != LoginInformation.creationTimeKey && key != LoginInformation.modificationTimeKey)
			{
				loginInformationAsDictionary[LoginInformation.modificationTimeKey] = modificationTime;
			}

			// Generate new algorithm since data has changed
			this.algorithm = SymmetricKeyAlgorithm.GenerateNew(this.algorithm.GetSymmetricEncryptionAlgorithm());

			// Create AUDALF payload from dictionary
			byte[] serializedBytes = AUDALF_Serialize.Serialize(loginInformationAsDictionary, valueTypes: null, serializationSettings: serializationSettings);

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
		return ChecksumHelper.CalculateHexChecksum(this.keyIdentifier, this.audalfData, this.algorithm.GetSettingsAsBytes());
	}

	private void CalculateAndUpdateChecksum()
	{
		this.checksum = this.CalculateHexChecksum();
	}

	#endregion // Checksum
}

#endif // !ASYNC_WITH_CUSTOM && !WITH_CUSTOM