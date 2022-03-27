#if ASYNC_WITH_CUSTOM

using System;
using System.Collections.Generic;
using System.Text;
using CSharp_AUDALF;

using System.Threading.Tasks;

namespace CSCommonSecrets
{
	/// <summary>
	/// LoginInformationSecret stores one encrypted login information
	/// </summary>
	public sealed partial class LoginInformationSecret
	{
		/// <summary>
		/// Default constructor for LoginInformationSecret, async
		/// </summary>
		/// <param name="loginInformation">LoginInformation that will be stored as secret</param>
		/// <param name="keyIdentifier">Key identifier</param>
		/// <param name="algorithm">SymmetricKeyAlgorithm used to encrypt the LoginInformation</param>
		/// <param name="derivedPassword">Derived password</param>
		/// <param name="securityFunctions">Security functions</param>
		public static async Task<LoginInformationSecret> CreateLoginInformationSecretAsync(LoginInformation loginInformation, string keyIdentifier, SymmetricKeyAlgorithm algorithm, byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
		{
			LoginInformationSecret loginInformationSecret = new LoginInformationSecret();

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

			loginInformationSecret.keyIdentifier = Encoding.UTF8.GetBytes(keyIdentifier);

			loginInformationSecret.algorithm = algorithm;

			// Create AUDALF payload from dictionary
			byte[] serializedBytes = AUDALF_Serialize.Serialize(dictionaryForAUDALF, valueTypes: null, serializationSettings: serializationSettings );

			// Encrypt the AUDALF payload with given algorithm
			loginInformationSecret.audalfData = await algorithm.EncryptBytesAsync(serializedBytes, derivedPassword, securityFunctions);

			// Calculate new checksum
			await loginInformationSecret.CalculateAndUpdateChecksumAsync(securityFunctions);

			return loginInformationSecret;
		}

		/// <summary>
		/// Constructor for custom dictionary, use this only if you know what you are doing, async
		/// </summary>
		/// <param name="loginInformationAsDictionary">Dictionary containing login information keys and values</param>
		/// <param name="keyIdentifier">Key identifier</param>
		/// <param name="algorithm">Symmetric Key Algorithm used for encryption</param>
		/// <param name="derivedPassword">Derived password</param>
		/// <param name="securityFunctions">Security functions</param>
		public static async Task<LoginInformationSecret> CreateLoginInformationSecretAsync(Dictionary<string, object> loginInformationAsDictionary, string keyIdentifier, SymmetricKeyAlgorithm algorithm, byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
		{
			LoginInformationSecret loginInformationSecret = new LoginInformationSecret();

			loginInformationSecret.keyIdentifier = Encoding.UTF8.GetBytes(keyIdentifier);

			loginInformationSecret.algorithm = algorithm;

			// Create AUDALF payload from dictionary
			byte[] serializedBytes = AUDALF_Serialize.Serialize(loginInformationAsDictionary, valueTypes: null, serializationSettings: serializationSettings );

			// Encrypt the AUDALF payload with given algorithm
			loginInformationSecret.audalfData = await algorithm.EncryptBytesAsync(serializedBytes, derivedPassword, securityFunctions);

			// Calculate new checksum
			await loginInformationSecret.CalculateAndUpdateChecksumAsync(securityFunctions);

			return loginInformationSecret;
		}

		#region Common getters

		/// <summary>
		/// Get LoginInformation. Use this for situation where you want to convert secret -> non secret, async
		/// </summary>
		/// <param name="derivedPassword">Derived password</param>
		/// <param name="securityFunctions">Security functions</param>
		/// <returns>LoginInformation</returns>
		public async Task<LoginInformation> GetLoginInformationAsync(byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
		{
			Dictionary<string, object> dict = await this.GetLoginInformationAsDictionaryAsync(derivedPassword, securityFunctions);
			LoginInformation returnValue = await LoginInformation.CreateLoginInformationAsync((string)dict[LoginInformation.titleKey], (string)dict[LoginInformation.urlKey], (string)dict[LoginInformation.emailKey],
										(string)dict[LoginInformation.usernameKey], (string)dict[LoginInformation.passwordKey], (string)dict[LoginInformation.notesKey], (string)dict[LoginInformation.mfaKey],
										(byte[])dict[LoginInformation.iconKey], (string)dict[LoginInformation.categoryKey], (string)dict[LoginInformation.tagsKey], securityFunctions
										);
			returnValue.creationTime = ((DateTimeOffset)dict[LoginInformation.creationTimeKey]).ToUnixTimeSeconds();
			returnValue.modificationTime = ((DateTimeOffset)dict[LoginInformation.modificationTimeKey]).ToUnixTimeSeconds();
			
			return returnValue;
		}

		/// <summary>
		/// Get title. This tries to decrypt data with given derived password, async
		/// </summary>
		/// <param name="derivedPassword">Derived password</param>
		/// <param name="securityFunctions">Security functions</param>
		/// <returns>Title</returns>
		public async Task<string> GetTitleAsync(byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
		{
			return (string)await Helpers.GetSingleValueAsync(this.audalfData, this.algorithm, derivedPassword, LoginInformation.titleKey, deserializationSettings, securityFunctions);
		}

		/// <summary>
		/// Get URL. This tries to decrypt data with given derived password, async
		/// </summary>
		/// <param name="derivedPassword">Derived password</param>
		/// <param name="securityFunctions">Security functions</param>
		/// <returns>URL</returns>
		public async Task<string> GetURLAsync(byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
		{
			return (string)await Helpers.GetSingleValueAsync(this.audalfData, this.algorithm, derivedPassword, LoginInformation.urlKey, deserializationSettings, securityFunctions);
		}

		/// <summary>
		/// Get email. This tries to decrypt data with given derived password, async
		/// </summary>
		/// <param name="derivedPassword">Derived password</param>
		/// <param name="securityFunctions">Security functions</param>
		/// <returns>Email</returns>
		public async Task<string> GetEmailAsync(byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
		{
			return (string)await Helpers.GetSingleValueAsync(this.audalfData, this.algorithm, derivedPassword, LoginInformation.emailKey, deserializationSettings, securityFunctions);
		}

		/// <summary>
		/// Get username. This tries to decrypt data with given derived password, async
		/// </summary>
		/// <param name="derivedPassword">Derived password</param>
		/// <param name="securityFunctions">Security functions</param>
		/// <returns>Username</returns>
		public async Task<string> GetUsernameAsync(byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
		{
			return (string)await Helpers.GetSingleValueAsync(this.audalfData, this.algorithm, derivedPassword, LoginInformation.usernameKey, deserializationSettings, securityFunctions);
		}

		/// <summary>
		/// Get password. This tries to decrypt data with given derived password, async
		/// </summary>
		/// <param name="derivedPassword">Derived password</param>
		/// <param name="securityFunctions">Security functions</param>
		/// <returns>Password</returns>
		public async Task<string> GetPasswordAsync(byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
		{
			return (string)await Helpers.GetSingleValueAsync(this.audalfData, this.algorithm, derivedPassword, LoginInformation.passwordKey, deserializationSettings, securityFunctions);
		}

		/// <summary>
		/// Get notes. This tries to decrypt data with given derived password, async
		/// </summary>
		/// <param name="derivedPassword">Derived password</param>
		/// <param name="securityFunctions">Security functions</param>
		/// <returns>Notes</returns>
		public async Task<string> GetNotesAsync(byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
		{
			return (string)await Helpers.GetSingleValueAsync(this.audalfData, this.algorithm, derivedPassword, LoginInformation.notesKey, deserializationSettings, securityFunctions);
		}

		/// <summary>
		/// Get MFA (e.g. TOTP Url). This tries to decrypt data with given derived password, async
		/// </summary>
		/// <param name="derivedPassword">Derived password</param>
		/// <param name="securityFunctions">Security functions</param>
		/// <returns>MFA</returns>
		public async Task<string> GetMFAAsync(byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
		{
			return (string)await Helpers.GetSingleValueAsync(this.audalfData, this.algorithm, derivedPassword, LoginInformation.mfaKey, deserializationSettings, securityFunctions);
		}

		/// <summary>
		/// Get creation time of Login Information secret. This tries to decrypt data with given derived password, async
		/// </summary>
		/// <param name="derivedPassword">Derived password</param>
		/// <param name="securityFunctions">Security functions</param>
		/// <returns>Creation time</returns>
		public async Task<DateTimeOffset> GetCreationTimeAsync(byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
		{
			return (DateTimeOffset)await Helpers.GetSingleValueAsync(this.audalfData, this.algorithm, derivedPassword, LoginInformation.creationTimeKey, deserializationSettings, securityFunctions);
		}

		/// <summary>
		/// Get modification time of Login Information secret. This tries to decrypt data with given derived password, async
		/// </summary>
		/// <param name="derivedPassword">Derived password</param>
		/// <param name="securityFunctions">Security functions</param>
		/// <returns>Modification time</returns>
		public async Task<DateTimeOffset> GetModificationTimeAsync(byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
		{
			return (DateTimeOffset)await Helpers.GetSingleValueAsync(this.audalfData, this.algorithm, derivedPassword, LoginInformation.modificationTimeKey, deserializationSettings, securityFunctions);
		}

		/// <summary>
		/// Get icon. This tries to decrypt data with given derived password, async
		/// </summary>
		/// <param name="derivedPassword">Derived password</param>
		/// <param name="securityFunctions">Security functions</param>
		/// <returns>Icon</returns>
		public async Task<byte[]> GetIconAsync(byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
		{
			return (byte[])await Helpers.GetSingleValueAsync(this.audalfData, this.algorithm, derivedPassword, LoginInformation.iconKey, deserializationSettings, securityFunctions);
		}

		/// <summary>
		/// Get category. This tries to decrypt data with given derived password, async
		/// </summary>
		/// <param name="derivedPassword">Derived password</param>
		/// <param name="securityFunctions">Security functions</param>
		/// <returns>Category</returns>
		public  async Task<string> GetCategoryAsync(byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
		{
			return (string)await Helpers.GetSingleValueAsync(this.audalfData, this.algorithm, derivedPassword, LoginInformation.categoryKey, deserializationSettings, securityFunctions);
		}

		/// <summary>
		/// Get tags. This tries to decrypt data with given derived password, async
		/// </summary>
		/// <param name="derivedPassword">Derived password</param>
		/// <param name="securityFunctions">Security functions</param>
		/// <returns>Tags</returns>
		public  async Task<string> GetTagsAsync(byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
		{
			return (string)await Helpers.GetSingleValueAsync(this.audalfData, this.algorithm, derivedPassword, LoginInformation.tagsKey, deserializationSettings, securityFunctions);
		}

		private async Task<Dictionary<string, object>> GetLoginInformationAsDictionaryAsync(byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
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

			Dictionary<string, object> loginInformationAsDictionary = AUDALF_Deserialize.Deserialize<string, object>(decryptedAUDALF, settings: deserializationSettings);

			return loginInformationAsDictionary;
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
		/// Try to set new title for login information secret by decrypting the current login information secret, setting a new value and then encrypting the modified login information secret, async
		/// </summary>
		/// <param name="newTitle">New title</param>
		/// <param name="derivedPassword">Derived password</param>
		/// <param name="securityFunctions">Security functions</param>
		/// <returns>True if set goes correctly; False otherwise</returns>
		public async Task<bool> SetTitleAsync(string newTitle, byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
		{
			return await this.GenericSetAsync(LoginInformation.titleKey, newTitle, DateTimeOffset.UtcNow, derivedPassword, securityFunctions);
		}

		/// <summary>
		/// Try to set new URL for login information secret by decrypting the current login information secret, setting a new value and then encrypting the modified login information secret, async
		/// </summary>
		/// <param name="newURL">New URL</param>
		/// <param name="derivedPassword">Derived password</param>
		/// <param name="securityFunctions">Security functions</param>
		/// <returns>True if set goes correctly; False otherwise</returns>
		public async Task<bool> SetURLAsync(string newURL, byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
		{
			return await this.GenericSetAsync(LoginInformation.urlKey, newURL, DateTimeOffset.UtcNow, derivedPassword, securityFunctions);
		}

		/// <summary>
		/// Try to set new email for login information secret by decrypting the current login information secret, setting a new value and then encrypting the modified login information secret, async
		/// </summary>
		/// <param name="newEmail">New email</param>
		/// <param name="derivedPassword">Derived password</param>
		/// <param name="securityFunctions">Security functions</param>
		/// <returns>True if set goes correctly; False otherwise</returns>
		public async Task<bool> SetEmailAsync(string newEmail, byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
		{
			return await this.GenericSetAsync(LoginInformation.emailKey, newEmail, DateTimeOffset.UtcNow, derivedPassword, securityFunctions);
		}

		/// <summary>
		/// Try to set new username for login information secret by decrypting the current login information secret, setting a new value and then encrypting the modified login information secret, async
		/// </summary>
		/// <param name="newUsername">New username</param>
		/// <param name="derivedPassword">Derived password</param>
		/// <param name="securityFunctions">Security functions</param>
		/// <returns>True if set goes correctly; False otherwise</returns>
		public async Task<bool> SetUsernameAsync(string newUsername, byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
		{
			return await this.GenericSetAsync(LoginInformation.usernameKey, newUsername, DateTimeOffset.UtcNow, derivedPassword, securityFunctions);
		}

		/// <summary>
		/// Try to set new password for login information secret by decrypting the current login information secret, setting a new value and then encrypting the modified login information secret, async
		/// </summary>
		/// <param name="newPassword">New password</param>
		/// <param name="derivedPassword">Derived password</param>
		/// <param name="securityFunctions">Security functions</param>
		/// <returns>True if set goes correctly; False otherwise</returns>
		public async Task<bool> SetPasswordAsync(string newPassword, byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
		{
			return await this.GenericSetAsync(LoginInformation.passwordKey, newPassword, DateTimeOffset.UtcNow, derivedPassword, securityFunctions);
		}

		/// <summary>
		/// Try to set new notes for login information secret by decrypting the current login information secret, setting a new value and then encrypting the modified login information secret, async
		/// </summary>
		/// <param name="newNotes">New notes</param>
		/// <param name="derivedPassword">Derived password</param>
		/// <param name="securityFunctions">Security functions</param>
		/// <returns>True if set goes correctly; False otherwise</returns>
		public async Task<bool> SetNotesAsync(string newNotes, byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
		{
			return await this.GenericSetAsync(LoginInformation.notesKey, newNotes, DateTimeOffset.UtcNow, derivedPassword, securityFunctions);
		}

		/// <summary>
		/// Try to set new MFA for login information secret by decrypting the current login information secret, setting a new value and then encrypting the modified login information secret, async
		/// </summary>
		/// <param name="newMFA">New MFA</param>
		/// <param name="derivedPassword">Derived password</param>
		/// <param name="securityFunctions">Security functions</param>
		/// <returns>True if set goes correctly; False otherwise</returns>
		public async Task<bool> SetMFAAsync(string newMFA, byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
		{
			return await this.GenericSetAsync(LoginInformation.mfaKey, newMFA, DateTimeOffset.UtcNow, derivedPassword, securityFunctions);
		}

		/// <summary>
		/// Try to set new creation time for login information secret by decrypting the current login information secret, setting a new value and then encrypting the modified login information secret, async
		/// </summary>
		/// <param name="newCreationTime">New creation time</param>
		/// <param name="derivedPassword">Derived password</param>
		/// <param name="securityFunctions">Security functions</param>
		/// <returns>True if set goes correctly; False otherwise</returns>
		public async Task<bool> SetCreationTimeAsync(DateTimeOffset newCreationTime, byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
		{
			return await this.GenericSetAsync(LoginInformation.creationTimeKey, newCreationTime,  DateTimeOffset.UtcNow, derivedPassword, securityFunctions);
		}

		/// <summary>
		/// Try to set new modification time for login information secret by decrypting the current login information secret, setting a new value and then encrypting the modified login information secret, async
		/// </summary>
		/// <param name="newModificationTime">New modification time</param>
		/// <param name="derivedPassword">Derived password</param>
		/// <param name="securityFunctions">Security functions</param>
		/// <returns>True if set goes correctly; False otherwise</returns>
		public async Task<bool> SetModificationTimeAsync(DateTimeOffset newModificationTime, byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
		{
			return await this.GenericSetAsync(LoginInformation.modificationTimeKey, newModificationTime,  DateTimeOffset.UtcNow, derivedPassword, securityFunctions);
		}

		/// <summary>
		/// Try to set new icon for login information secret by decrypting the current login information secret, setting a new value and then encrypting the modified login information secret, async
		/// </summary>
		/// <param name="newIcon">New icon</param>
		/// <param name="derivedPassword">Derived password</param>
		/// <param name="securityFunctions">Security functions</param>
		/// <returns>True if set goes correctly; False otherwise</returns>
		public async Task<bool> SetIconAsync(byte[] newIcon, byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
		{
			return await this.GenericSetAsync(LoginInformation.iconKey, newIcon, DateTimeOffset.UtcNow, derivedPassword, securityFunctions);
		}

		/// <summary>
		/// Try to set new category for login information secret by decrypting the current login information secret, setting a new value and then encrypting the modified login information secret, async
		/// </summary>
		/// <param name="newCategory">New category</param>
		/// <param name="derivedPassword">Derived password</param>
		/// <param name="securityFunctions">Security functions</param>
		/// <returns>True if set goes correctly; False otherwise</returns>
		public async Task<bool> SetCategoryAsync(string newCategory, byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
		{
			return await this.GenericSetAsync(LoginInformation.categoryKey, newCategory, DateTimeOffset.UtcNow, derivedPassword, securityFunctions);
		}

		/// <summary>
		/// Try to set new tags for login information secret by decrypting the current login information secret, setting a new value and then encrypting the modified login information secret, async
		/// </summary>
		/// <param name="newTags">New tags</param>
		/// <param name="derivedPassword">Derived password</param>
		/// <param name="securityFunctions">Security functions</param>
		/// <returns>True if set goes correctly; False otherwise</returns>
		public async Task<bool> SetTagsAsync(string newTags, byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
		{
			return await this.GenericSetAsync(LoginInformation.tagsKey, newTags, DateTimeOffset.UtcNow, derivedPassword, securityFunctions);
		}

		private async Task<bool> GenericSetAsync(string key, object value, DateTimeOffset modificationTime, byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
		{
			try 
			{
				Dictionary<string, object> loginInformationAsDictionary = await this.GetLoginInformationAsDictionaryAsync(derivedPassword, securityFunctions);
				loginInformationAsDictionary[key] = value;

				if (key != LoginInformation.creationTimeKey && key != LoginInformation.modificationTimeKey)
				{
					loginInformationAsDictionary[LoginInformation.modificationTimeKey] = modificationTime;
				}

				// Generate new algorithm since data has changed
				this.algorithm = SymmetricKeyAlgorithm.GenerateNew(this.algorithm.GetSymmetricEncryptionAlgorithm(), securityFunctions);

				// Create AUDALF payload from dictionary
				byte[] serializedBytes = AUDALF_Serialize.Serialize(loginInformationAsDictionary, valueTypes: null, serializationSettings: serializationSettings );

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

}

#endif // ASYNC_WITH_CUSTOM