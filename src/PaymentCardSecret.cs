using System;
using System.Text;
using System.Collections.Generic;
using CSharp_AUDALF;

namespace CSCommonSecrets
{
	/// <summary>
	/// Payment card secret stores stores one encrypted payment card
	/// </summary>
	public sealed class PaymentCardSecret
	{
		/// <summary>
		/// Key identifier bytes (this is plaintext information), in normal case it is better to use GetKeyIdentifier()
		/// </summary>
		public byte[] keyIdentifier { get; set; }

		/// <summary>
		/// AUDALF data as byte array (this is secret/encrypted information)
		/// </summary>
		public byte[] audalfData { get; set; } = new byte[0];

		/// <summary>
		/// Symmetric Key Algorithm for this PaymentCardSecret (this is plaintext information)
		/// </summary>
		public SymmetricKeyAlgorithm algorithm { get; set; }

		/// <summary>
		/// Checksum of the data (this is plaintext information)
		/// </summary>
		public string checksum { get; set; } = string.Empty;

		/// <summary>
		/// For deserialization
		/// </summary>
		public PaymentCardSecret()
		{

		}

		/// <summary>
		/// Default constructor for PaymentCardSecret
		/// </summary>
		/// <param name="paymentCard">PaymentCard to encrypt</param>
		/// <param name="keyIdentifier">Key identifier</param>
		/// <param name="algorithm">Symmetric Key Algorithm used for encryption</param>
		/// <param name="derivedPassword">Derived password</param>
		public PaymentCardSecret(PaymentCard paymentCard, string keyIdentifier, SymmetricKeyAlgorithm algorithm, byte[] derivedPassword)
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

			this.keyIdentifier = Encoding.UTF8.GetBytes(keyIdentifier);

			this.algorithm = algorithm;

			// Create AUDALF payload from dictionary
			byte[] serializedBytes = AUDALF_Serialize.Serialize(dictionaryForAUDALF, valueTypes: null, serializationSettings: serializationSettings );

			// Encrypt the AUDALF payload with given algorithm
			this.audalfData = algorithm.EncryptBytes(serializedBytes, derivedPassword);

			// Calculate new checksum
			this.CalculateAndUpdateChecksum();
		}

		/// <summary>
		/// Deep copy existing NoteSecret
		/// </summary>
		/// <param name="copyThis">Deep copy this</param>
		public PaymentCardSecret(NoteSecret copyThis)
		{
			this.keyIdentifier = new byte[copyThis.keyIdentifier.Length];
			Buffer.BlockCopy(copyThis.keyIdentifier, 0, this.keyIdentifier, 0, copyThis.keyIdentifier.Length);

			this.audalfData = new byte[copyThis.audalfData.Length];
			Buffer.BlockCopy(copyThis.audalfData, 0, this.audalfData, 0, copyThis.audalfData.Length);

			this.algorithm = new SymmetricKeyAlgorithm(copyThis.algorithm);
			this.checksum = copyThis.checksum;
		}

		private static readonly SerializationSettings serializationSettings = new SerializationSettings() { dateTimeFormat = DateTimeFormat.UnixInSeconds };

		/// <summary>
		/// Constructor for custom dictionary, use this only if you know what you are doing
		/// </summary>
		/// <param name="noteAsDictionary">Dictionary containing note keys and values</param>
		/// <param name="keyIdentifier">Key identifier</param>
		/// <param name="algorithm">Symmetric Key Algorithm used for encryption</param>
		/// <param name="derivedPassword">Derived password</param>
		public PaymentCardSecret(Dictionary<string, object> noteAsDictionary, string keyIdentifier, SymmetricKeyAlgorithm algorithm, byte[] derivedPassword)
		{
			this.keyIdentifier = Encoding.UTF8.GetBytes(keyIdentifier);

			this.algorithm = algorithm;

			// Create AUDALF payload from dictionary
			byte[] serializedBytes = AUDALF_Serialize.Serialize(noteAsDictionary, valueTypes: null, serializationSettings: serializationSettings );

			// Encrypt the AUDALF payload with given algorithm
			this.audalfData = algorithm.EncryptBytes(serializedBytes, derivedPassword);

			// Calculate new checksum
			this.CalculateAndUpdateChecksum();
		}

		/// <summary>
		/// Deep copy existing PaymentCardSecret
		/// </summary>
		/// <param name="copyThis">Deep copy this</param>
		public PaymentCardSecret(PaymentCardSecret copyThis)
		{
			this.keyIdentifier = new byte[copyThis.keyIdentifier.Length];
			Buffer.BlockCopy(copyThis.keyIdentifier, 0, this.keyIdentifier, 0, copyThis.keyIdentifier.Length);

			this.audalfData = new byte[copyThis.audalfData.Length];
			Buffer.BlockCopy(copyThis.audalfData, 0, this.audalfData, 0, copyThis.audalfData.Length);

			this.algorithm = new SymmetricKeyAlgorithm(copyThis.algorithm);
			this.checksum = copyThis.checksum;
		}

		#region Common getters

		/// <summary>
		/// Get PaymentCard. Use this for situation where you want to convert secret -> non secret
		/// </summary>
		/// <param name="derivedPassword">Derived password</param>
		/// <returns>PaymentCard</returns>
		public PaymentCard GetPaymentCard(byte[] derivedPassword)
		{
			Dictionary<string, object> dict = this.GetPaymentCardAsDictionary(derivedPassword);
			PaymentCard returnValue = new PaymentCard((string)dict[PaymentCard.titleKey], (string)dict[PaymentCard.nameOnCardKey], (string)dict[PaymentCard.cardTypeKey],
										(string)dict[PaymentCard.numberKey], (string)dict[PaymentCard.securityCodeKey], (string)dict[PaymentCard.startDateKey], 
										(string)dict[PaymentCard.expirationDateKey], (string)dict[PaymentCard.notesKey]);
			returnValue.creationTime = ((DateTimeOffset)dict[PaymentCard.creationTimeKey]).ToUnixTimeSeconds();
			returnValue.modificationTime = ((DateTimeOffset)dict[PaymentCard.modificationTimeKey]).ToUnixTimeSeconds();
			
			return returnValue;
		}

		/// <summary>
		/// Get title. This tries to decrypt data with given derived password
		/// </summary>
		/// <param name="derivedPassword">Derived password</param>
		/// <returns>Title</returns>
		public string GetTitle(byte[] derivedPassword)
		{
			Dictionary<string, object> paymentCardAsDictionary = this.GetPaymentCardAsDictionary(derivedPassword);
			return (string)paymentCardAsDictionary[PaymentCard.titleKey];
		}

		/// <summary>
		/// Get name on card. This tries to decrypt data with given derived password
		/// </summary>
		/// <param name="derivedPassword">Derived password</param>
		/// <returns>Name on card</returns>
		public string GetNameOnCard(byte[] derivedPassword)
		{
			Dictionary<string, object> paymentCardAsDictionary = this.GetPaymentCardAsDictionary(derivedPassword);
			return (string)paymentCardAsDictionary[PaymentCard.nameOnCardKey];
		}

		/// <summary>
		/// Get card type. This tries to decrypt data with given derived password
		/// </summary>
		/// <param name="derivedPassword">Derived password</param>
		/// <returns>Card type</returns>
		public string GetCardType(byte[] derivedPassword)
		{
			Dictionary<string, object> paymentCardAsDictionary = this.GetPaymentCardAsDictionary(derivedPassword);
			return (string)paymentCardAsDictionary[PaymentCard.cardTypeKey];
		}

		/// <summary>
		/// Get number. This tries to decrypt data with given derived password
		/// </summary>
		/// <param name="derivedPassword">Derived password</param>
		/// <returns>Number</returns>
		public string GetNumber(byte[] derivedPassword)
		{
			Dictionary<string, object> paymentCardAsDictionary = this.GetPaymentCardAsDictionary(derivedPassword);
			return (string)paymentCardAsDictionary[PaymentCard.numberKey];
		}

		/// <summary>
		/// Get security code. This tries to decrypt data with given derived password
		/// </summary>
		/// <param name="derivedPassword">Derived password</param>
		/// <returns>Security code</returns>
		public string GetSecurityCode(byte[] derivedPassword)
		{
			Dictionary<string, object> paymentCardAsDictionary = this.GetPaymentCardAsDictionary(derivedPassword);
			return (string)paymentCardAsDictionary[PaymentCard.securityCodeKey];
		}

		/// <summary>
		/// Get start date. This tries to decrypt data with given derived password
		/// </summary>
		/// <param name="derivedPassword">Derived password</param>
		/// <returns>Start date</returns>
		public string GetStartDate(byte[] derivedPassword)
		{
			Dictionary<string, object> paymentCardAsDictionary = this.GetPaymentCardAsDictionary(derivedPassword);
			return (string)paymentCardAsDictionary[PaymentCard.startDateKey];
		}

		/// <summary>
		/// Get expiration date. This tries to decrypt data with given derived password
		/// </summary>
		/// <param name="derivedPassword">Derived password</param>
		/// <returns>Expiration date</returns>
		public string GetExpirationDate(byte[] derivedPassword)
		{
			Dictionary<string, object> paymentCardAsDictionary = this.GetPaymentCardAsDictionary(derivedPassword);
			return (string)paymentCardAsDictionary[PaymentCard.expirationDateKey];
		}

		/// <summary>
		/// Get notes. This tries to decrypt data with given derived password
		/// </summary>
		/// <param name="derivedPassword">Derived password</param>
		/// <returns>Notes</returns>
		public string GetNotes(byte[] derivedPassword)
		{
			Dictionary<string, object> paymentCardAsDictionary = this.GetPaymentCardAsDictionary(derivedPassword);
			return (string)paymentCardAsDictionary[PaymentCard.notesKey];
		}

		/// <summary>
		/// Get creation time of Payment card secret. This tries to decrypt data with given derived password
		/// </summary>
		/// <param name="derivedPassword">Derived password</param>
		/// <returns>Creation time</returns>
		public DateTimeOffset GetCreationTime(byte[] derivedPassword)
		{
			Dictionary<string, object> paymentCardAsDictionary = this.GetPaymentCardAsDictionary(derivedPassword);
			return (DateTimeOffset)paymentCardAsDictionary[PaymentCard.creationTimeKey];
		}

		/// <summary>
		/// Get modification time of Payment card secret. This tries to decrypt data with given derived password
		/// </summary>
		/// <param name="derivedPassword">Derived password</param>
		/// <returns>Modification time</returns>
		public DateTimeOffset GetModificationTime(byte[] derivedPassword)
		{
			Dictionary<string, object> paymentCardAsDictionary = this.GetPaymentCardAsDictionary(derivedPassword);
			return (DateTimeOffset)paymentCardAsDictionary[PaymentCard.modificationTimeKey];
		}

		/// <summary>
		/// Get key identifer.
		/// </summary>
		/// <returns>Key identifier</returns>
		public string GetKeyIdentifier()
		{
			return System.Text.Encoding.UTF8.GetString(this.keyIdentifier);
		}

		private static readonly DeserializationSettings deserializationSettings = new DeserializationSettings()
		{
			wantedDateTimeType = typeof(DateTimeOffset)
		};

		private Dictionary<string, object> GetPaymentCardAsDictionary(byte[] derivedPassword)
		{
			var passwordCheck = Helpers.CheckDerivedPassword(derivedPassword);

			if (!passwordCheck.valid)
			{
				throw passwordCheck.exception;
			}

			// Try to decrypt the binary
			byte[] decryptedAUDALF = algorithm.EncryptBytes(this.audalfData, derivedPassword);

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
		/// <returns>True if can be; False otherwise</returns>
		public bool CanBeDecryptedWithDerivedPassword(byte[] derivedPassword)
		{
			var passwordCheck = Helpers.CheckDerivedPassword(derivedPassword);

			if (!passwordCheck.valid)
			{
				return false;
			}

			// Try to decrypt the binary
			byte[] decryptedAUDALF = algorithm.EncryptBytes(this.audalfData, derivedPassword);

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
		/// <returns>True if set goes correctly; False otherwise</returns>
		public bool SetTitle(string newTitle, byte[] derivedPassword)
		{
			return this.GenericSet(PaymentCard.titleKey, newTitle, DateTimeOffset.UtcNow, derivedPassword);
		}

		/// <summary>
		/// Try to set new name on card for payment card secret by decrypting the current payment card secret, setting a new value and then encrypting the modified payment card secret
		/// </summary>
		/// <param name="newNameOnCard">New name on card</param>
		/// <param name="derivedPassword">Derived password</param>
		/// <returns>True if set goes correctly; False otherwise</returns>
		public bool SetNameOnCard(string newNameOnCard, byte[] derivedPassword)
		{
			return this.GenericSet(PaymentCard.nameOnCardKey, newNameOnCard, DateTimeOffset.UtcNow, derivedPassword);
		}

		/// <summary>
		/// Try to set new card type for payment card secret by decrypting the current payment card secret, setting a new value and then encrypting the modified payment card secret
		/// </summary>
		/// <param name="newCardType">New card type</param>
		/// <param name="derivedPassword">Derived password</param>
		/// <returns>True if set goes correctly; False otherwise</returns>
		public bool SetCardType(string newCardType, byte[] derivedPassword)
		{
			return this.GenericSet(PaymentCard.cardTypeKey, newCardType, DateTimeOffset.UtcNow, derivedPassword);
		}

		/// <summary>
		/// Try to set new number for payment card secret by decrypting the current payment card secret, setting a new value and then encrypting the modified payment card secret
		/// </summary>
		/// <param name="newNumber">New number</param>
		/// <param name="derivedPassword">Derived password</param>
		/// <returns>True if set goes correctly; False otherwise</returns>
		public bool SetNumber(string newNumber, byte[] derivedPassword)
		{
			return this.GenericSet(PaymentCard.numberKey, newNumber, DateTimeOffset.UtcNow, derivedPassword);
		}

		/// <summary>
		/// Try to set new security code for payment card secret by decrypting the current payment card secret, setting a new value and then encrypting the modified payment card secret
		/// </summary>
		/// <param name="newSecurityCode">New security code</param>
		/// <param name="derivedPassword">Derived password</param>
		/// <returns>True if set goes correctly; False otherwise</returns>
		public bool SetSecurityCode(string newSecurityCode, byte[] derivedPassword)
		{
			return this.GenericSet(PaymentCard.securityCodeKey, newSecurityCode, DateTimeOffset.UtcNow, derivedPassword);
		}

		/// <summary>
		/// Try to set new start date for payment card secret by decrypting the current payment card secret, setting a new value and then encrypting the modified payment card secret
		/// </summary>
		/// <param name="newStartDate">New start date</param>
		/// <param name="derivedPassword">Derived password</param>
		/// <returns>True if set goes correctly; False otherwise</returns>
		public bool SetStartDate(string newStartDate, byte[] derivedPassword)
		{
			return this.GenericSet(PaymentCard.startDateKey, newStartDate, DateTimeOffset.UtcNow, derivedPassword);
		}

		/// <summary>
		/// Try to set new expiration date for payment card secret by decrypting the current payment card secret, setting a new value and then encrypting the modified payment card secret
		/// </summary>
		/// <param name="newExpirationDate">New expiration date</param>
		/// <param name="derivedPassword">Derived password</param>
		/// <returns>True if set goes correctly; False otherwise</returns>
		public bool SetExpirationDate(string newExpirationDate, byte[] derivedPassword)
		{
			return this.GenericSet(PaymentCard.expirationDateKey, newExpirationDate, DateTimeOffset.UtcNow, derivedPassword);
		}

		/// <summary>
		/// Try to set new notes for payment card secret by decrypting the current payment card secret, setting a new value and then encrypting the modified payment card secret
		/// </summary>
		/// <param name="newNotes">New notes</param>
		/// <param name="derivedPassword">Derived password</param>
		/// <returns>True if set goes correctly; False otherwise</returns>
		public bool SetNotes(string newNotes, byte[] derivedPassword)
		{
			return this.GenericSet(PaymentCard.notesKey, newNotes, DateTimeOffset.UtcNow, derivedPassword);
		}

		private bool GenericSet(string key, object value, DateTimeOffset modificationTime, byte[] derivedPassword)
		{
			try 
			{
				Dictionary<string, object> paymentCardAsDictionary = this.GetPaymentCardAsDictionary(derivedPassword);
				paymentCardAsDictionary[key] = value;

				if (key != PaymentCard.creationTimeKey && key != PaymentCard.modificationTimeKey)
				{
					paymentCardAsDictionary[PaymentCard.modificationTimeKey] = modificationTime;
				}

				// Generate new algorithm since data has changed
				this.algorithm = SymmetricKeyAlgorithm.GenerateNew(this.algorithm.GetSymmetricEncryptionAlgorithm());

				// Create AUDALF payload from dictionary
				byte[] serializedBytes = AUDALF_Serialize.Serialize(paymentCardAsDictionary, valueTypes: null, serializationSettings: serializationSettings );

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
		/// Get checksum as hex
		/// </summary>
		/// <returns>Hex string</returns>
		public string GetChecksumAsHex()
		{
			return this.checksum;
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
}