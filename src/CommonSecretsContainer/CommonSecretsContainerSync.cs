#if !ASYNC_WITH_CUSTOM && !WITH_CUSTOM

using System;
using System.Collections.Generic;

namespace CSCommonSecrets
{
	/// <summary>
	/// CommonSecretsContainer stores all other data. e.g. KeyDerivationFunctionEntries, LoginInformations etc.
	/// </summary>
	public sealed partial class CommonSecretsContainer
	{
		#region Add helpers

		/// <summary>
		/// Add login information secret to Common secret container
		/// </summary>
		/// <param name="password">Plaintext password</param>
		/// <param name="loginInformation">Loginiformation to add</param>
		/// <param name="keyIdentifier">Key identifier</param>
		/// <param name="algorithm">Symmetric Encryption Algorithm to use</param>
		/// <returns>Tuple that tells if add was success, and possible error</returns>
		public (bool success, string possibleError) AddLoginInformationSecret(string password, LoginInformation loginInformation, string keyIdentifier, SymmetricEncryptionAlgorithm algorithm = SymmetricEncryptionAlgorithm.AES_CTR)
		{
			(bool checkResult, string possibleError) = MandatoryChecks(loginInformation, "LoginInformation", keyIdentifier, password);
			if (!checkResult)
			{
				return (checkResult, possibleError);
			}

			byte[] derivedPassword = this.FindKeyDerivationFunctionEntryWithKeyIdentifier(keyIdentifier).GeneratePasswordBytes(password);

			this.AddLoginInformationSecretActual(derivedPassword, loginInformation, keyIdentifier, algorithm);

			return (success: true, possibleError: "");
		}

		/// <summary>
		/// Add login information secret to Common secret container
		/// </summary>
		/// <param name="derivedPassword">Derived password</param>
		/// <param name="loginInformation">Loginiformation to add</param>
		/// <param name="keyIdentifier">Key identifier</param>
		/// <param name="algorithm">Symmetric Encryption Algorithm to use</param>
		/// <returns>Tuple that tells if add was success, and possible error</returns>
		public (bool success, string possibleError) AddLoginInformationSecret(byte[] derivedPassword, LoginInformation loginInformation, string keyIdentifier, SymmetricEncryptionAlgorithm algorithm = SymmetricEncryptionAlgorithm.AES_CTR)
		{
			(bool checkResult, string possibleError) = MandatoryChecks(loginInformation, "LoginInformation", keyIdentifier, derivedPassword);
			if (!checkResult)
			{
				return (checkResult, possibleError);
			}

			this.AddLoginInformationSecretActual(derivedPassword, loginInformation, keyIdentifier, algorithm);

			return (success: true, possibleError: "");
		}

		private void AddLoginInformationSecretActual(byte[] derivedPassword, LoginInformation loginInformation, string keyIdentifier, SymmetricEncryptionAlgorithm algorithm)
		{
			SymmetricKeyAlgorithm ska = SymmetricKeyAlgorithm.GenerateNew(algorithm);
			this.loginInformationSecrets.Add(new LoginInformationSecret(loginInformation, keyIdentifier, ska, derivedPassword));
		}

		/// <summary>
		/// Add note secret to Common secret container
		/// </summary>
		/// <param name="password">Plaintext password</param>
		/// <param name="note">Note to add</param>
		/// <param name="keyIdentifier">Key identifier</param>
		/// <param name="algorithm">Symmetric Encryption Algorithm to use</param>
		/// <returns>Tuple that tells if add was success, and possible error</returns>
		public (bool success, string possibleError) AddNoteSecret(string password, Note note, string keyIdentifier, SymmetricEncryptionAlgorithm algorithm = SymmetricEncryptionAlgorithm.AES_CTR)
		{
			(bool checkResult, string possibleError) = MandatoryChecks(note, "Note", keyIdentifier, password);
			if (!checkResult)
			{
				return (checkResult, possibleError);
			}

			byte[] derivedPassword = this.FindKeyDerivationFunctionEntryWithKeyIdentifier(keyIdentifier).GeneratePasswordBytes(password);

			this.AddNoteSecretActual(derivedPassword, note, keyIdentifier, algorithm);

			return (success: true, possibleError: "");
		}

		/// <summary>
		/// Add note secret to Common secret container
		/// </summary>
		/// <param name="derivedPassword">Derived password</param>
		/// <param name="note">Note to add</param>
		/// <param name="keyIdentifier">Key identifier</param>
		/// <param name="algorithm">Symmetric Encryption Algorithm to use</param>
		/// <returns>Tuple that tells if add was success, and possible error</returns>
		public (bool success, string possibleError) AddNoteSecret(byte[] derivedPassword, Note note, string keyIdentifier, SymmetricEncryptionAlgorithm algorithm = SymmetricEncryptionAlgorithm.AES_CTR)
		{
			(bool checkResult, string possibleError) = MandatoryChecks(note, "Note", keyIdentifier, derivedPassword);
			if (!checkResult)
			{
				return (checkResult, possibleError);
			}

			this.AddNoteSecretActual(derivedPassword, note, keyIdentifier, algorithm);

			return (success: true, possibleError: "");
		}

		private void AddNoteSecretActual(byte[] derivedPassword, Note note, string keyIdentifier, SymmetricEncryptionAlgorithm algorithm)
		{
			SymmetricKeyAlgorithm ska = SymmetricKeyAlgorithm.GenerateNew(algorithm);
			this.noteSecrets.Add(new NoteSecret(note, keyIdentifier, ska, derivedPassword));
		}

		/// <summary>
		/// Add file entry to Common secret container
		/// </summary>
		/// <param name="password">Plaintext password</param>
		/// <param name="fileEntry">File entry to add</param>
		/// <param name="keyIdentifier">Key identifier</param>
		/// <param name="algorithm">Symmetric Encryption Algorithm to use</param>
		/// <returns>Tuple that tells if add was success, and possible error</returns>
		public (bool success, string possibleError) AddFileEntrySecret(string password, FileEntry fileEntry, string keyIdentifier, SymmetricEncryptionAlgorithm algorithm = SymmetricEncryptionAlgorithm.AES_CTR)
		{
			(bool checkResult, string possibleError) = MandatoryChecks(fileEntry, "FileEntry", keyIdentifier, password);
			if (!checkResult)
			{
				return (checkResult, possibleError);
			}

			byte[] derivedPassword = this.FindKeyDerivationFunctionEntryWithKeyIdentifier(keyIdentifier).GeneratePasswordBytes(password);

			this.AddFileEntrySecretActual(derivedPassword, fileEntry, keyIdentifier, algorithm);

			return (success: true, possibleError: "");
		}

		/// <summary>
		/// Add file entry to Common secret container
		/// </summary>
		/// <param name="derivedPassword">Derived password</param>
		/// <param name="fileEntry">File entry to add</param>
		/// <param name="keyIdentifier">Key identifier</param>
		/// <param name="algorithm">Symmetric Encryption Algorithm to use</param>
		/// <returns>Tuple that tells if add was success, and possible error</returns>
		public (bool success, string possibleError) AddFileEntrySecret(byte[] derivedPassword, FileEntry fileEntry, string keyIdentifier, SymmetricEncryptionAlgorithm algorithm = SymmetricEncryptionAlgorithm.AES_CTR)
		{
			(bool checkResult, string possibleError) = MandatoryChecks(fileEntry, "FileEntry", keyIdentifier, derivedPassword);
			if (!checkResult)
			{
				return (checkResult, possibleError);
			}

			this.AddFileEntrySecretActual(derivedPassword, fileEntry, keyIdentifier, algorithm);

			return (success: true, possibleError: "");
		}

		private void AddFileEntrySecretActual(byte[] derivedPassword, FileEntry fileEntry, string keyIdentifier, SymmetricEncryptionAlgorithm algorithm)
		{
			SymmetricKeyAlgorithm ska = SymmetricKeyAlgorithm.GenerateNew(algorithm);
			this.fileSecrets.Add(new FileEntrySecret(fileEntry, keyIdentifier, ska, derivedPassword));
		}

		/// <summary>
		/// Add contact to Common secret container
		/// </summary>
		/// <param name="password">Plaintext password</param>
		/// <param name="contact">Contact to add</param>
		/// <param name="keyIdentifier">Key identifier</param>
		/// <param name="algorithm">Symmetric Encryption Algorithm to use</param>
		/// <returns>Tuple that tells if add was success, and possible error</returns>
		public (bool success, string possibleError) AddContactSecret(string password, Contact contact, string keyIdentifier, SymmetricEncryptionAlgorithm algorithm = SymmetricEncryptionAlgorithm.AES_CTR)
		{
			(bool checkResult, string possibleError) = MandatoryChecks(contact, "Contact", keyIdentifier, password);
			if (!checkResult)
			{
				return (checkResult, possibleError);
			}

			byte[] derivedPassword = this.FindKeyDerivationFunctionEntryWithKeyIdentifier(keyIdentifier).GeneratePasswordBytes(password);

			this.AddContactSecretActual(derivedPassword, contact, keyIdentifier, algorithm);

			return (success: true, possibleError: "");
		}

		/// <summary>
		/// Add contact to Common secret container
		/// </summary>
		/// <param name="derivedPassword">Derived password</param>
		/// <param name="contact">Contact to add</param>
		/// <param name="keyIdentifier">Key identifier</param>
		/// <param name="algorithm">Symmetric Encryption Algorithm to use</param>
		/// <returns>Tuple that tells if add was success, and possible error</returns>
		public (bool success, string possibleError) AddContactSecret(byte[] derivedPassword, Contact contact, string keyIdentifier, SymmetricEncryptionAlgorithm algorithm = SymmetricEncryptionAlgorithm.AES_CTR)
		{
			(bool checkResult, string possibleError) = MandatoryChecks(contact, "Contact", keyIdentifier, derivedPassword);
			if (!checkResult)
			{
				return (checkResult, possibleError);
			}

			this.AddContactSecretActual(derivedPassword, contact, keyIdentifier, algorithm);

			return (success: true, possibleError: "");
		}

		private void AddContactSecretActual(byte[] derivedPassword, Contact contact, string keyIdentifier, SymmetricEncryptionAlgorithm algorithm)
		{
			SymmetricKeyAlgorithm ska = SymmetricKeyAlgorithm.GenerateNew(algorithm);
			this.contactSecrets.Add(new ContactSecret(contact, keyIdentifier, ska, derivedPassword));
		}

		/// <summary>
		/// Add payment card to Common secret container
		/// </summary>
		/// <param name="password">Plaintext password</param>
		/// <param name="paymentCard">Payment card to add</param>
		/// <param name="keyIdentifier">Key identifier</param>
		/// <param name="algorithm">Symmetric Encryption Algorithm to use</param>
		/// <returns>Tuple that tells if add was success, and possible error</returns>
		public (bool success, string possibleError) AddPaymentCardSecret(string password, PaymentCard paymentCard, string keyIdentifier, SymmetricEncryptionAlgorithm algorithm = SymmetricEncryptionAlgorithm.AES_CTR)
		{
			(bool checkResult, string possibleError) = MandatoryChecks(paymentCard, "PaymentCard", keyIdentifier, password);
			if (!checkResult)
			{
				return (checkResult, possibleError);
			}

			SymmetricKeyAlgorithm ska = SymmetricKeyAlgorithm.GenerateNew(algorithm);

			byte[] derivedPassword = this.FindKeyDerivationFunctionEntryWithKeyIdentifier(keyIdentifier).GeneratePasswordBytes(password);

			this.paymentCardSecrets.Add(new PaymentCardSecret(paymentCard, keyIdentifier, ska, derivedPassword));

			return (success: true, possibleError: "");
		}

		/// <summary>
		/// Add payment card to Common secret container
		/// </summary>
		/// <param name="derivedPassword">Derived password</param>
		/// <param name="paymentCard">Payment card to add</param>
		/// <param name="keyIdentifier">Key identifier</param>
		/// <param name="algorithm">Symmetric Encryption Algorithm to use</param>
		/// <returns>Tuple that tells if add was success, and possible error</returns>
		public (bool success, string possibleError) AddPaymentCardSecret(byte[] derivedPassword, PaymentCard paymentCard, string keyIdentifier, SymmetricEncryptionAlgorithm algorithm = SymmetricEncryptionAlgorithm.AES_CTR)
		{
			(bool checkResult, string possibleError) = MandatoryChecks(paymentCard, "PaymentCard", keyIdentifier, derivedPassword);
			if (!checkResult)
			{
				return (checkResult, possibleError);
			}

			SymmetricKeyAlgorithm ska = SymmetricKeyAlgorithm.GenerateNew(algorithm);

			this.paymentCardSecrets.Add(new PaymentCardSecret(paymentCard, keyIdentifier, ska, derivedPassword));

			return (success: true, possibleError: "");
		}

		#endregion // Add helpers


		#region Replace helpers

		/// <summary>
		/// Replace existing login information secret in Common secret container with another one (basically for editing purposes)
		/// </summary>
		/// <param name="zeroBasedIndex">Zero based index of login information secret that will be replaced</param>
		/// <param name="password">Plaintext password</param>
		/// <param name="loginInformation">Loginiformation to add</param>
		/// <param name="keyIdentifier">Key identifier</param>
		/// <param name="algorithm">Symmetric Encryption Algorithm to use</param>
		/// <returns>Tuple that tells if add was success, and possible error</returns>
		public (bool success, string possibleError) ReplaceLoginInformationSecret(int zeroBasedIndex, string password, LoginInformation loginInformation, string keyIdentifier, SymmetricEncryptionAlgorithm algorithm = SymmetricEncryptionAlgorithm.AES_CTR)
		{
			if (zeroBasedIndex < 0 || zeroBasedIndex >= this.loginInformationSecrets.Count)
			{
				return (false, $"Index {zeroBasedIndex} is out of bounds [0, {this.loginInformationSecrets.Count})");
			}

			(bool checkResult, string possibleError) = MandatoryChecks(loginInformation, "LoginInformation", keyIdentifier, password);
			if (!checkResult)
			{
				return (checkResult, possibleError);
			}

			SymmetricKeyAlgorithm ska = SymmetricKeyAlgorithm.GenerateNew(algorithm);

			byte[] derivedPassword = this.FindKeyDerivationFunctionEntryWithKeyIdentifier(keyIdentifier).GeneratePasswordBytes(password);

			this.loginInformationSecrets[zeroBasedIndex] = new LoginInformationSecret(loginInformation, keyIdentifier, ska, derivedPassword);

			return (success: true, possibleError: "");
		}

		/// <summary>
		/// Replace existing login information secret in Common secret container with another one (basically for editing purposes)
		/// </summary>
		/// <param name="zeroBasedIndex">Zero based index of login information secret that will be replaced</param>
		/// <param name="derivedPassword">Derived password</param>
		/// <param name="loginInformation">Loginiformation to add</param>
		/// <param name="keyIdentifier">Key identifier</param>
		/// <param name="algorithm">Symmetric Encryption Algorithm to use</param>
		/// <returns>Tuple that tells if add was success, and possible error</returns>
		public (bool success, string possibleError) ReplaceLoginInformationSecret(int zeroBasedIndex, byte[] derivedPassword, LoginInformation loginInformation, string keyIdentifier, SymmetricEncryptionAlgorithm algorithm = SymmetricEncryptionAlgorithm.AES_CTR)
		{
			if (zeroBasedIndex < 0 || zeroBasedIndex >= this.loginInformationSecrets.Count)
			{
				return (false, $"Index {zeroBasedIndex} is out of bounds [0, {this.loginInformationSecrets.Count})");
			}

			(bool checkResult, string possibleError) = MandatoryChecks(loginInformation, "LoginInformation", keyIdentifier, derivedPassword);
			if (!checkResult)
			{
				return (checkResult, possibleError);
			}

			SymmetricKeyAlgorithm ska = SymmetricKeyAlgorithm.GenerateNew(algorithm);

			this.loginInformationSecrets[zeroBasedIndex] = new LoginInformationSecret(loginInformation, keyIdentifier, ska, derivedPassword);

			return (success: true, possibleError: "");
		}

		/// <summary>
		/// Replace existing note secret in Common secret container with another one (basically for editing purposes)
		/// </summary>
		/// <param name="zeroBasedIndex">Zero based index of note secret that will be replaced</param>
		/// <param name="password">Plaintext password</param>
		/// <param name="note">Note to add</param>
		/// <param name="keyIdentifier">Key identifier</param>
		/// <param name="algorithm">Symmetric Encryption Algorithm to use</param>
		/// <returns>Tuple that tells if add was success, and possible error</returns>
		public (bool success, string possibleError) ReplaceNoteSecret(int zeroBasedIndex, string password, Note note, string keyIdentifier, SymmetricEncryptionAlgorithm algorithm = SymmetricEncryptionAlgorithm.AES_CTR)
		{
			if (zeroBasedIndex < 0 || zeroBasedIndex >= this.noteSecrets.Count)
			{
				return (false, $"Index {zeroBasedIndex} is out of bounds [0, {this.noteSecrets.Count})");
			}

			(bool checkResult, string possibleError) = MandatoryChecks(note, "Note", keyIdentifier, password);
			if (!checkResult)
			{
				return (checkResult, possibleError);
			}

			SymmetricKeyAlgorithm ska = SymmetricKeyAlgorithm.GenerateNew(algorithm);

			byte[] derivedPassword = this.FindKeyDerivationFunctionEntryWithKeyIdentifier(keyIdentifier).GeneratePasswordBytes(password);

			this.noteSecrets[zeroBasedIndex] = new NoteSecret(note, keyIdentifier, ska, derivedPassword);

			return (success: true, possibleError: "");
		}

		/// <summary>
		/// Replace existing note secret in Common secret container with another one (basically for editing purposes)
		/// </summary>
		/// <param name="zeroBasedIndex">Zero based index of note secret that will be replaced</param>
		/// <param name="derivedPassword">Derived password</param>
		/// <param name="note">Note to add</param>
		/// <param name="keyIdentifier">Key identifier</param>
		/// <param name="algorithm">Symmetric Encryption Algorithm to use</param>
		/// <returns>Tuple that tells if add was success, and possible error</returns>
		public (bool success, string possibleError) ReplaceNoteSecret(int zeroBasedIndex, byte[] derivedPassword, Note note, string keyIdentifier, SymmetricEncryptionAlgorithm algorithm = SymmetricEncryptionAlgorithm.AES_CTR)
		{
			if (zeroBasedIndex < 0 || zeroBasedIndex >= this.noteSecrets.Count)
			{
				return (false, $"Index {zeroBasedIndex} is out of bounds [0, {this.noteSecrets.Count})");
			}

			(bool checkResult, string possibleError) = MandatoryChecks(note, "Note", keyIdentifier, derivedPassword);
			if (!checkResult)
			{
				return (checkResult, possibleError);
			}

			SymmetricKeyAlgorithm ska = SymmetricKeyAlgorithm.GenerateNew(algorithm);

			this.noteSecrets[zeroBasedIndex] = new NoteSecret(note, keyIdentifier, ska, derivedPassword);

			return (success: true, possibleError: "");
		}

		/// <summary>
		/// Replace existing file entry in Common secret container with another one (basically for editing purposes)
		/// </summary>
		/// <param name="zeroBasedIndex">Zero based index of file entry secret that will be replaced</param>
		/// <param name="password">Plaintext password</param>
		/// <param name="fileEntry">File entry to add</param>
		/// <param name="keyIdentifier">Key identifier</param>
		/// <param name="algorithm">Symmetric Encryption Algorithm to use</param>
		/// <returns>Tuple that tells if add was success, and possible error</returns>
		public (bool success, string possibleError) ReplaceFileEntrySecret(int zeroBasedIndex, string password, FileEntry fileEntry, string keyIdentifier, SymmetricEncryptionAlgorithm algorithm = SymmetricEncryptionAlgorithm.AES_CTR)
		{
			if (zeroBasedIndex < 0 || zeroBasedIndex >= this.fileSecrets.Count)
			{
				return (false, $"Index {zeroBasedIndex} is out of bounds [0, {this.fileSecrets.Count})");
			}

			(bool checkResult, string possibleError) = MandatoryChecks(fileEntry, "FileEntry", keyIdentifier, password);
			if (!checkResult)
			{
				return (checkResult, possibleError);
			}

			SymmetricKeyAlgorithm ska = SymmetricKeyAlgorithm.GenerateNew(algorithm);

			byte[] derivedPassword = this.FindKeyDerivationFunctionEntryWithKeyIdentifier(keyIdentifier).GeneratePasswordBytes(password);

			this.fileSecrets[zeroBasedIndex] = new FileEntrySecret(fileEntry, keyIdentifier, ska, derivedPassword);

			return (success: true, possibleError: "");
		}

		/// <summary>
		/// Replace existing file entry in Common secret container with another one (basically for editing purposes)
		/// </summary>
		/// <param name="zeroBasedIndex">Zero based index of file entry secret that will be replaced</param>
		/// <param name="derivedPassword">Derived password</param>
		/// <param name="fileEntry">File entry to add</param>
		/// <param name="keyIdentifier">Key identifier</param>
		/// <param name="algorithm">Symmetric Encryption Algorithm to use</param>
		/// <returns>Tuple that tells if add was success, and possible error</returns>
		public (bool success, string possibleError) ReplaceFileEntrySecret(int zeroBasedIndex, byte[] derivedPassword, FileEntry fileEntry, string keyIdentifier, SymmetricEncryptionAlgorithm algorithm = SymmetricEncryptionAlgorithm.AES_CTR)
		{
			if (zeroBasedIndex < 0 || zeroBasedIndex >= this.fileSecrets.Count)
			{
				return (false, $"Index {zeroBasedIndex} is out of bounds [0, {this.fileSecrets.Count})");
			}

			(bool checkResult, string possibleError) = MandatoryChecks(fileEntry, "FileEntry", keyIdentifier, derivedPassword);
			if (!checkResult)
			{
				return (checkResult, possibleError);
			}

			SymmetricKeyAlgorithm ska = SymmetricKeyAlgorithm.GenerateNew(algorithm);

			this.fileSecrets[zeroBasedIndex] = new FileEntrySecret(fileEntry, keyIdentifier, ska, derivedPassword);

			return (success: true, possibleError: "");
		}

		/// <summary>
		/// Replace existing contact in Common secret container with another one (basically for editing purposes)
		/// </summary>
		/// <param name="zeroBasedIndex">Zero based index of contact secret that will be replaced</param>
		/// <param name="password">Plaintext password</param>
		/// <param name="contact">Contact to add</param>
		/// <param name="keyIdentifier">Key identifier</param>
		/// <param name="algorithm">Symmetric Encryption Algorithm to use</param>
		/// <returns>Tuple that tells if add was success, and possible error</returns>
		public (bool success, string possibleError) ReplaceContactSecret(int zeroBasedIndex, string password, Contact contact, string keyIdentifier, SymmetricEncryptionAlgorithm algorithm = SymmetricEncryptionAlgorithm.AES_CTR)
		{
			if (zeroBasedIndex < 0 || zeroBasedIndex >= this.contactSecrets.Count)
			{
				return (false, $"Index {zeroBasedIndex} is out of bounds [0, {this.contactSecrets.Count})");
			}

			(bool checkResult, string possibleError) = MandatoryChecks(contact, "Contact", keyIdentifier, password);
			if (!checkResult)
			{
				return (checkResult, possibleError);
			}

			SymmetricKeyAlgorithm ska = SymmetricKeyAlgorithm.GenerateNew(algorithm);

			byte[] derivedPassword = this.FindKeyDerivationFunctionEntryWithKeyIdentifier(keyIdentifier).GeneratePasswordBytes(password);

			this.contactSecrets[zeroBasedIndex] = new ContactSecret(contact, keyIdentifier, ska, derivedPassword);

			return (success: true, possibleError: "");
		}

		/// <summary>
		/// Replace existing contact in Common secret container with another one (basically for editing purposes)
		/// </summary>
		/// <param name="zeroBasedIndex">Zero based index of contact secret that will be replaced</param>
		/// <param name="derivedPassword">Derived password</param>
		/// <param name="contact">Contact to add</param>
		/// <param name="keyIdentifier">Key identifier</param>
		/// <param name="algorithm">Symmetric Encryption Algorithm to use</param>
		/// <returns>Tuple that tells if add was success, and possible error</returns>
		public (bool success, string possibleError) ReplaceContactSecret(int zeroBasedIndex, byte[] derivedPassword, Contact contact, string keyIdentifier, SymmetricEncryptionAlgorithm algorithm = SymmetricEncryptionAlgorithm.AES_CTR)
		{
			if (zeroBasedIndex < 0 || zeroBasedIndex >= this.contactSecrets.Count)
			{
				return (false, $"Index {zeroBasedIndex} is out of bounds [0, {this.contactSecrets.Count})");
			}

			(bool checkResult, string possibleError) = MandatoryChecks(contact, "Contact", keyIdentifier, derivedPassword);
			if (!checkResult)
			{
				return (checkResult, possibleError);
			}

			SymmetricKeyAlgorithm ska = SymmetricKeyAlgorithm.GenerateNew(algorithm);

			this.contactSecrets[zeroBasedIndex] = new ContactSecret(contact, keyIdentifier, ska, derivedPassword);

			return (success: true, possibleError: "");
		}

		/// <summary>
		/// Replace existing payment card in Common secret container with another one (basically for editing purposes)
		/// </summary>
		/// <param name="zeroBasedIndex">Zero based index of payment card secret that will be replaced</param>
		/// <param name="password">Plaintext password</param>
		/// <param name="paymentCard">Payment card to add</param>
		/// <param name="keyIdentifier">Key identifier</param>
		/// <param name="algorithm">Symmetric Encryption Algorithm to use</param>
		/// <returns>Tuple that tells if add was success, and possible error</returns>
		public (bool success, string possibleError) ReplacePaymentCardSecret(int zeroBasedIndex, string password, PaymentCard paymentCard, string keyIdentifier, SymmetricEncryptionAlgorithm algorithm = SymmetricEncryptionAlgorithm.AES_CTR)
		{
			if (zeroBasedIndex < 0 || zeroBasedIndex >= this.paymentCardSecrets.Count)
			{
				return (false, $"Index {zeroBasedIndex} is out of bounds [0, {this.paymentCardSecrets.Count})");
			}

			(bool checkResult, string possibleError) = MandatoryChecks(paymentCard, "PaymentCard", keyIdentifier, password);
			if (!checkResult)
			{
				return (checkResult, possibleError);
			}

			SymmetricKeyAlgorithm ska = SymmetricKeyAlgorithm.GenerateNew(algorithm);

			byte[] derivedPassword = this.FindKeyDerivationFunctionEntryWithKeyIdentifier(keyIdentifier).GeneratePasswordBytes(password);

			this.paymentCardSecrets[zeroBasedIndex] = new PaymentCardSecret(paymentCard, keyIdentifier, ska, derivedPassword);

			return (success: true, possibleError: "");
		}

		/// <summary>
		/// Replace existing payment card in Common secret container with another one (basically for editing purposes)
		/// </summary>
		/// <param name="zeroBasedIndex">Zero based index of payment card secret that will be replaced</param>
		/// <param name="derivedPassword">Derived password</param>
		/// <param name="paymentCard">Payment card to add</param>
		/// <param name="keyIdentifier">Key identifier</param>
		/// <param name="algorithm">Symmetric Encryption Algorithm to use</param>
		/// <returns>Tuple that tells if add was success, and possible error</returns>
		public (bool success, string possibleError) ReplacePaymentCardSecret(int zeroBasedIndex, byte[] derivedPassword, PaymentCard paymentCard, string keyIdentifier, SymmetricEncryptionAlgorithm algorithm = SymmetricEncryptionAlgorithm.AES_CTR)
		{
			if (zeroBasedIndex < 0 || zeroBasedIndex >= this.paymentCardSecrets.Count)
			{
				return (false, $"Index {zeroBasedIndex} is out of bounds [0, {this.paymentCardSecrets.Count})");
			}

			(bool checkResult, string possibleError) = MandatoryChecks(paymentCard, "PaymentCard", keyIdentifier, derivedPassword);
			if (!checkResult)
			{
				return (checkResult, possibleError);
			}

			SymmetricKeyAlgorithm ska = SymmetricKeyAlgorithm.GenerateNew(algorithm);

			this.paymentCardSecrets[zeroBasedIndex] = new PaymentCardSecret(paymentCard, keyIdentifier, ska, derivedPassword);

			return (success: true, possibleError: "");
		}

		#endregion // Replace helpers
	}
}

#endif // !ASYNC_WITH_CUSTOM && !WITH_CUSTOM