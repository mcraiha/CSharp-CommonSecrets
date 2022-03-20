#if ASYNC_WITH_CUSTOM

using System;
using System.Text;
using System.Collections.Generic;
using CSharp_AUDALF;

using System.Threading.Tasks;

namespace CSCommonSecrets
{
	/// <summary>
	/// Payment card secret stores stores one encrypted payment card
	/// </summary>
	public sealed partial class PaymentCardSecret
	{
		/// <summary>
		/// Default constructor for PaymentCardSecret
		/// </summary>
		/// <param name="paymentCard">PaymentCard to encrypt</param>
		/// <param name="keyIdentifier">Key identifier</param>
		/// <param name="algorithm">Symmetric Key Algorithm used for encryption</param>
		/// <param name="derivedPassword">Derived password</param>
		/// <param name="securityFunctions">Security functions</param>
		public static async Task<PaymentCardSecret> CreatePaymentCardSecret(PaymentCard paymentCard, string keyIdentifier, SymmetricKeyAlgorithm algorithm, byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
		{
			Dictionary<string, object> dictionaryForAUDALF = new Dictionary<string, object>()
			{
				{ PaymentCard.titleKey, paymentCard.GetTitle() },
				{ PaymentCard.nameOnCardKey, paymentCard.GetNameOnCard() },
				{ PaymentCard.cardTypeKey, paymentCard.GetCardType() },
				{ PaymentCard.numberKey, paymentCard.GetNumber() },
				{ PaymentCard.securityCodeKey, paymentCard.GetSecurityCode() },
				{ PaymentCard.startDateKey, paymentCard.GetStartDate() },
				{ PaymentCard.expirationDateKey, paymentCard.GetExpirationDate() },
				{ PaymentCard.notesKey, paymentCard.GetNotes() },
				{ PaymentCard.creationTimeKey, DateTimeOffset.FromUnixTimeSeconds(paymentCard.creationTime) },
				{ PaymentCard.modificationTimeKey, DateTimeOffset.FromUnixTimeSeconds(paymentCard.modificationTime) },
			};

			PaymentCardSecret paymentCardSecret = new PaymentCardSecret();

			paymentCardSecret.keyIdentifier = Encoding.UTF8.GetBytes(keyIdentifier);

			paymentCardSecret.algorithm = algorithm;

			// Create AUDALF payload from dictionary
			byte[] serializedBytes = AUDALF_Serialize.Serialize(dictionaryForAUDALF, valueTypes: null, serializationSettings: serializationSettings );

			// Encrypt the AUDALF payload with given algorithm
			paymentCardSecret.audalfData = await algorithm.EncryptBytesAsync(serializedBytes, derivedPassword, securityFunctions);

			// Calculate new checksum
			await paymentCardSecret.CalculateAndUpdateChecksumAsync(securityFunctions);

			return paymentCardSecret;
		}

		/// <summary>
		/// Constructor for custom dictionary, use this only if you know what you are doing
		/// </summary>
		/// <param name="paymentCardAsDictionary">Dictionary containing payment card keys and values</param>
		/// <param name="keyIdentifier">Key identifier</param>
		/// <param name="algorithm">Symmetric Key Algorithm used for encryption</param>
		/// <param name="derivedPassword">Derived password</param>
		/// <param name="securityFunctions">Security functions</param>
		public static async Task<PaymentCardSecret> CreatePaymentCardSecret(Dictionary<string, object> paymentCardAsDictionary, string keyIdentifier, SymmetricKeyAlgorithm algorithm, byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
		{
			PaymentCardSecret paymentCardSecret = new PaymentCardSecret();

			paymentCardSecret.keyIdentifier = Encoding.UTF8.GetBytes(keyIdentifier);

			paymentCardSecret.algorithm = algorithm;

			// Create AUDALF payload from dictionary
			byte[] serializedBytes = AUDALF_Serialize.Serialize(paymentCardAsDictionary, valueTypes: null, serializationSettings: serializationSettings );

			// Encrypt the AUDALF payload with given algorithm
			paymentCardSecret.audalfData = await algorithm.EncryptBytesAsync(serializedBytes, derivedPassword, securityFunctions);

			// Calculate new checksum
			await paymentCardSecret.CalculateAndUpdateChecksumAsync(securityFunctions);

			return paymentCardSecret;
		}

		#region Common getters

		/// <summary>
		/// Get PaymentCard. Use this for situation where you want to convert secret -> non secret
		/// </summary>
		/// <param name="derivedPassword">Derived password</param>
		/// <param name="securityFunctions">Security functions</param>
		/// <returns>PaymentCard</returns>
		public async Task<PaymentCard> GetPaymentCardAsync(byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
		{
			Dictionary<string, object> dict = await this.GetPaymentCardAsDictionaryAsync(derivedPassword, securityFunctions);
			PaymentCard returnValue = await PaymentCard.CreatePaymentCardAsync((string)dict[PaymentCard.titleKey], (string)dict[PaymentCard.nameOnCardKey], (string)dict[PaymentCard.cardTypeKey],
										(string)dict[PaymentCard.numberKey], (string)dict[PaymentCard.securityCodeKey], (string)dict[PaymentCard.startDateKey], 
										(string)dict[PaymentCard.expirationDateKey], (string)dict[PaymentCard.notesKey], securityFunctions);
			returnValue.creationTime = ((DateTimeOffset)dict[PaymentCard.creationTimeKey]).ToUnixTimeSeconds();
			returnValue.modificationTime = ((DateTimeOffset)dict[PaymentCard.modificationTimeKey]).ToUnixTimeSeconds();
			
			return returnValue;
		}

		/// <summary>
		/// Get title. This tries to decrypt data with given derived password
		/// </summary>
		/// <param name="derivedPassword">Derived password</param>
		/// <param name="securityFunctions">Security functions</param>
		/// <returns>Title</returns>
		public async Task<string> GetTitleAsync(byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
		{
			return (string)await Helpers.GetSingleValueAsync(this.audalfData, this.algorithm, derivedPassword, PaymentCard.titleKey, deserializationSettings, securityFunctions);
		}

		/// <summary>
		/// Get name on card. This tries to decrypt data with given derived password
		/// </summary>
		/// <param name="derivedPassword">Derived password</param>
		/// <param name="securityFunctions">Security functions</param>
		/// <returns>Name on card</returns>
		public async Task<string> GetNameOnCardAsync(byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
		{
			return (string)await Helpers.GetSingleValueAsync(this.audalfData, this.algorithm, derivedPassword, PaymentCard.nameOnCardKey, deserializationSettings, securityFunctions);
		}

		/// <summary>
		/// Get card type. This tries to decrypt data with given derived password
		/// </summary>
		/// <param name="derivedPassword">Derived password</param>
		/// <param name="securityFunctions">Security functions</param>
		/// <returns>Card type</returns>
		public async Task<string> GetCardTypeAsync(byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
		{
			return (string)await Helpers.GetSingleValueAsync(this.audalfData, this.algorithm, derivedPassword, PaymentCard.cardTypeKey, deserializationSettings, securityFunctions);
		}

		/// <summary>
		/// Get number. This tries to decrypt data with given derived password
		/// </summary>
		/// <param name="derivedPassword">Derived password</param>
		/// <param name="securityFunctions">Security functions</param>
		/// <returns>Number</returns>
		public async Task<string> GetNumberAsync(byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
		{
			return (string)await Helpers.GetSingleValueAsync(this.audalfData, this.algorithm, derivedPassword, PaymentCard.numberKey, deserializationSettings, securityFunctions);
		}

		/// <summary>
		/// Get security code. This tries to decrypt data with given derived password
		/// </summary>
		/// <param name="derivedPassword">Derived password</param>
		/// <param name="securityFunctions">Security functions</param>
		/// <returns>Security code</returns>
		public async Task<string> GetSecurityCodeAsync(byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
		{
			return (string)await Helpers.GetSingleValueAsync(this.audalfData, this.algorithm, derivedPassword, PaymentCard.securityCodeKey, deserializationSettings, securityFunctions);
		}

		/// <summary>
		/// Get start date. This tries to decrypt data with given derived password
		/// </summary>
		/// <param name="derivedPassword">Derived password</param>
		/// <param name="securityFunctions">Security functions</param>
		/// <returns>Start date</returns>
		public async Task<string> GetStartDateAsync(byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
		{
			return (string)await Helpers.GetSingleValueAsync(this.audalfData, this.algorithm, derivedPassword, PaymentCard.startDateKey, deserializationSettings, securityFunctions);
		}

		/// <summary>
		/// Get expiration date. This tries to decrypt data with given derived password
		/// </summary>
		/// <param name="derivedPassword">Derived password</param>
		/// <param name="securityFunctions">Security functions</param>
		/// <returns>Expiration date</returns>
		public async Task<string> GetExpirationDateAsync(byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
		{
			return (string)await Helpers.GetSingleValueAsync(this.audalfData, this.algorithm, derivedPassword, PaymentCard.expirationDateKey, deserializationSettings, securityFunctions);
		}

		/// <summary>
		/// Get notes. This tries to decrypt data with given derived password
		/// </summary>
		/// <param name="derivedPassword">Derived password</param>
		/// <param name="securityFunctions">Security functions</param>
		/// <returns>Notes</returns>
		public async Task<string> GetNotesAsync(byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
		{
			return (string)await Helpers.GetSingleValueAsync(this.audalfData, this.algorithm, derivedPassword, PaymentCard.notesKey, deserializationSettings, securityFunctions);
		}

		/// <summary>
		/// Get creation time of Payment card secret. This tries to decrypt data with given derived password
		/// </summary>
		/// <param name="derivedPassword">Derived password</param>
		/// <param name="securityFunctions">Security functions</param>
		/// <returns>Creation time</returns>
		public async Task<DateTimeOffset> GetCreationTimeAsync(byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
		{
			return (DateTimeOffset)await Helpers.GetSingleValueAsync(this.audalfData, this.algorithm, derivedPassword, PaymentCard.creationTimeKey, deserializationSettings, securityFunctions);
		}

		/// <summary>
		/// Get modification time of Payment card secret. This tries to decrypt data with given derived password
		/// </summary>
		/// <param name="derivedPassword">Derived password</param>
		/// <param name="securityFunctions">Security functions</param>
		/// <returns>Modification time</returns>
		public async Task<DateTimeOffset> GetModificationTimeAsync(byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
		{
			return (DateTimeOffset)await Helpers.GetSingleValueAsync(this.audalfData, this.algorithm, derivedPassword, PaymentCard.modificationTimeKey, deserializationSettings, securityFunctions);
		}

		/// <summary>
		/// Get key identifer.
		/// </summary>
		/// <returns>Key identifier</returns>
		public string GetKeyIdentifier()
		{
			return System.Text.Encoding.UTF8.GetString(this.keyIdentifier);
		}

		private async Task<Dictionary<string, object>> GetPaymentCardAsDictionaryAsync(byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
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

			Dictionary<string, object> paymentCardAsDictionary = AUDALF_Deserialize.Deserialize<string, object>(decryptedAUDALF, settings: deserializationSettings);

			return paymentCardAsDictionary;
		}

		/// <summary>
		/// Can the content be decrypted with given derived password
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
		/// Try to set new title for payment card secret by decrypting the current payment card secret, setting a new value and then encrypting the modified payment card secret
		/// </summary>
		/// <param name="newTitle">New title</param>
		/// <param name="derivedPassword">Derived password</param>
		/// <param name="securityFunctions">Security functions</param>
		/// <returns>True if set goes correctly; False otherwise</returns>
		public async Task<bool> SetTitleAsync(string newTitle, byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
		{
			return await this.GenericSetAsync(PaymentCard.titleKey, newTitle, DateTimeOffset.UtcNow, derivedPassword, securityFunctions);
		}

		/// <summary>
		/// Try to set new name on card for payment card secret by decrypting the current payment card secret, setting a new value and then encrypting the modified payment card secret
		/// </summary>
		/// <param name="newNameOnCard">New name on card</param>
		/// <param name="derivedPassword">Derived password</param>
		/// <param name="securityFunctions">Security functions</param>
		/// <returns>True if set goes correctly; False otherwise</returns>
		public async Task<bool> SetNameOnCardAsync(string newNameOnCard, byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
		{
			return await this.GenericSetAsync(PaymentCard.nameOnCardKey, newNameOnCard, DateTimeOffset.UtcNow, derivedPassword, securityFunctions);
		}

		/// <summary>
		/// Try to set new card type for payment card secret by decrypting the current payment card secret, setting a new value and then encrypting the modified payment card secret
		/// </summary>
		/// <param name="newCardType">New card type</param>
		/// <param name="derivedPassword">Derived password</param>
		/// <param name="securityFunctions">Security functions</param>
		/// <returns>True if set goes correctly; False otherwise</returns>
		public async Task<bool> SetCardTypeAsync(string newCardType, byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
		{
			return await this.GenericSetAsync(PaymentCard.cardTypeKey, newCardType, DateTimeOffset.UtcNow, derivedPassword, securityFunctions);
		}

		/// <summary>
		/// Try to set new number for payment card secret by decrypting the current payment card secret, setting a new value and then encrypting the modified payment card secret
		/// </summary>
		/// <param name="newNumber">New number</param>
		/// <param name="derivedPassword">Derived password</param>
		/// <param name="securityFunctions">Security functions</param>
		/// <returns>True if set goes correctly; False otherwise</returns>
		public async Task<bool> SetNumberAsync(string newNumber, byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
		{
			return await this.GenericSetAsync(PaymentCard.numberKey, newNumber, DateTimeOffset.UtcNow, derivedPassword, securityFunctions);
		}

		/// <summary>
		/// Try to set new security code for payment card secret by decrypting the current payment card secret, setting a new value and then encrypting the modified payment card secret
		/// </summary>
		/// <param name="newSecurityCode">New security code</param>
		/// <param name="derivedPassword">Derived password</param>
		/// <param name="securityFunctions">Security functions</param>
		/// <returns>True if set goes correctly; False otherwise</returns>
		public async Task<bool> SetSecurityCodeAsync(string newSecurityCode, byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
		{
			return await this.GenericSetAsync(PaymentCard.securityCodeKey, newSecurityCode, DateTimeOffset.UtcNow, derivedPassword, securityFunctions);
		}

		/// <summary>
		/// Try to set new start date for payment card secret by decrypting the current payment card secret, setting a new value and then encrypting the modified payment card secret
		/// </summary>
		/// <param name="newStartDate">New start date</param>
		/// <param name="derivedPassword">Derived password</param>
		/// <param name="securityFunctions">Security functions</param>
		/// <returns>True if set goes correctly; False otherwise</returns>
		public async Task<bool> SetStartDateAsync(string newStartDate, byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
		{
			return await this.GenericSetAsync(PaymentCard.startDateKey, newStartDate, DateTimeOffset.UtcNow, derivedPassword, securityFunctions);
		}

		/// <summary>
		/// Try to set new expiration date for payment card secret by decrypting the current payment card secret, setting a new value and then encrypting the modified payment card secret
		/// </summary>
		/// <param name="newExpirationDate">New expiration date</param>
		/// <param name="derivedPassword">Derived password</param>
		/// <param name="securityFunctions">Security functions</param>
		/// <returns>True if set goes correctly; False otherwise</returns>
		public async Task<bool> SetExpirationDateAsync(string newExpirationDate, byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
		{
			return await this.GenericSetAsync(PaymentCard.expirationDateKey, newExpirationDate, DateTimeOffset.UtcNow, derivedPassword, securityFunctions);
		}

		/// <summary>
		/// Try to set new notes for payment card secret by decrypting the current payment card secret, setting a new value and then encrypting the modified payment card secret
		/// </summary>
		/// <param name="newNotes">New notes</param>
		/// <param name="derivedPassword">Derived password</param>
		/// <param name="securityFunctions">Security functions</param>
		/// <returns>True if set goes correctly; False otherwise</returns>
		public async Task<bool> SetNotesAsync(string newNotes, byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
		{
			return await this.GenericSetAsync(PaymentCard.notesKey, newNotes, DateTimeOffset.UtcNow, derivedPassword, securityFunctions);
		}

		private async Task<bool> GenericSetAsync(string key, object value, DateTimeOffset modificationTime, byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
		{
			try 
			{
				Dictionary<string, object> paymentCardAsDictionary = await this.GetPaymentCardAsDictionaryAsync(derivedPassword, securityFunctions);
				paymentCardAsDictionary[key] = value;

				if (key != PaymentCard.creationTimeKey && key != PaymentCard.modificationTimeKey)
				{
					paymentCardAsDictionary[PaymentCard.modificationTimeKey] = modificationTime;
				}

				// Generate new algorithm since data has changed
				this.algorithm = SymmetricKeyAlgorithm.GenerateNew(this.algorithm.GetSymmetricEncryptionAlgorithm(), securityFunctions);

				// Create AUDALF payload from dictionary
				byte[] serializedBytes = AUDALF_Serialize.Serialize(paymentCardAsDictionary, valueTypes: null, serializationSettings: serializationSettings );

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
		/// Check if checksum matches content
		/// </summary>
		/// <param name="securityFunctions">Security functions</param>
		/// <returns>True if matches; False otherwise</returns>
		public async Task<bool> CheckIfChecksumMatchesContentAsync(ISecurityAsyncFunctions securityFunctions)
		{
			return checksum == await CalculateHexChecksumAsync(securityFunctions);
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
}

#endif // ASYNC_WITH_CUSTOM