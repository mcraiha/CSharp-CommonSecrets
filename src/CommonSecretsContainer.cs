using System;
using System.Collections.Generic;

namespace CSCommonSecrets
{
	/// <summary>
	/// CommonSecretsContainer stores all other data. e.g. KeyDerivationFunctionEntries, LoginInformations etc.
	/// </summary>
	public sealed class CommonSecretsContainer
	{
		/// <summary>
		/// Current common secrets container specification version
		/// </summary>
		public static readonly int currentVersionNumber = 1;

		/// <summary>
		/// Specification version of this Common Secrets Container instance
		/// </summary>
		public int version { get; set; }

		/// <summary>
		/// All key derivation function entries in list
		/// </summary>
		public List<KeyDerivationFunctionEntry> keyDerivationFunctionEntries { get; set; } = new List<KeyDerivationFunctionEntry>();

		/// <summary>
		/// List of login informations (plain text ones)
		/// </summary>
		public List<LoginInformation> loginInformations { get; set; } = new List<LoginInformation>();

		/// <summary>
		/// List of login information secrets
		/// </summary>
		public List<LoginInformationSecret> loginInformationSecrets { get; set; } = new List<LoginInformationSecret>();

		/// <summary>
		/// List of notes (plain text ones)
		/// </summary>
		public List<Note> notes { get; set; } = new List<Note>();

		/// <summary>
		/// List of note secrets
		/// </summary>
		public List<NoteSecret> noteSecrets { get; set; } = new List<NoteSecret>();

		/// <summary>
		/// List of files (plain text ones)
		/// </summary>
		public List<FileEntry> files { get; set; } = new List<FileEntry>();

		/// <summary>
		/// List of file secrets
		/// </summary>
		public List<FileEntrySecret> fileSecrets { get; set; } = new List<FileEntrySecret>();

		/// <summary>
		/// List of contacts (plain text ones)
		/// </summary>
		public List<Contact> contacts { get; set; } = new List<Contact>();

		/// <summary>
		/// List of contact secrets
		/// </summary>
		public List<ContactSecret> contactSecrets { get; set; } = new List<ContactSecret>();

		/// <summary>
		/// List of payment cards (plain text ones)
		/// </summary>
		public List<PaymentCard> paymentCards { get; set; } = new List<PaymentCard>();

		/// <summary>
		/// List of payment card secrets
		/// </summary>
		public List<PaymentCardSecret> paymentCardSecrets { get; set; } = new List<PaymentCardSecret>();

		/// <summary>
		/// Constructor without parameters for creating empty CommonSecretsContainer
		/// </summary>
		public CommonSecretsContainer()
		{
			this.version = currentVersionNumber;
		}

		/// <summary>
		/// Constructor for creating CommonSecretsContainer with one Key Derivation Function
		/// </summary>
		/// <param name="primaryKDF">"Primary" key derivation function</param>
		public CommonSecretsContainer(KeyDerivationFunctionEntry primaryKDF)
		{
			this.version = currentVersionNumber;
			this.keyDerivationFunctionEntries.Add(primaryKDF);
		}

		#region Helpers

		/// <summary>
		/// Find KeyDerivationFunctionEntry with key identifier
		/// </summary>
		/// <param name="keyIdentifier">Key identifier to seek</param>
		/// <returns>KeyDerivationFunctionEntry if match is found; null otherwise</returns>
		public KeyDerivationFunctionEntry FindKeyDerivationFunctionEntryWithKeyIdentifier(string keyIdentifier)
		{
			KeyDerivationFunctionEntry returnValue = null;

			foreach (KeyDerivationFunctionEntry kdfe in keyDerivationFunctionEntries)
			{
				if (keyIdentifier == kdfe.GetKeyIdentifier())
				{
					returnValue = kdfe;
					break;
				}
			}

			return returnValue;
		}


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

			SymmetricKeyAlgorithm ska = SymmetricKeyAlgorithm.GenerateNew(algorithm);

			byte[] derivedPassword = this.FindKeyDerivationFunctionEntryWithKeyIdentifier(keyIdentifier).GeneratePasswordBytes(password);

			this.loginInformationSecrets.Add(new LoginInformationSecret(loginInformation, keyIdentifier, ska, derivedPassword));

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

			SymmetricKeyAlgorithm ska = SymmetricKeyAlgorithm.GenerateNew(algorithm);

			this.loginInformationSecrets.Add(new LoginInformationSecret(loginInformation, keyIdentifier, ska, derivedPassword));

			return (success: true, possibleError: "");
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

			SymmetricKeyAlgorithm ska = SymmetricKeyAlgorithm.GenerateNew(algorithm);

			byte[] derivedPassword = this.FindKeyDerivationFunctionEntryWithKeyIdentifier(keyIdentifier).GeneratePasswordBytes(password);

			this.noteSecrets.Add(new NoteSecret(note, keyIdentifier, ska, derivedPassword));

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

			SymmetricKeyAlgorithm ska = SymmetricKeyAlgorithm.GenerateNew(algorithm);

			this.noteSecrets.Add(new NoteSecret(note, keyIdentifier, ska, derivedPassword));

			return (success: true, possibleError: "");
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

			SymmetricKeyAlgorithm ska = SymmetricKeyAlgorithm.GenerateNew(algorithm);

			byte[] derivedPassword = this.FindKeyDerivationFunctionEntryWithKeyIdentifier(keyIdentifier).GeneratePasswordBytes(password);

			this.fileSecrets.Add(new FileEntrySecret(fileEntry, keyIdentifier, ska, derivedPassword));

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

			SymmetricKeyAlgorithm ska = SymmetricKeyAlgorithm.GenerateNew(algorithm);

			this.fileSecrets.Add(new FileEntrySecret(fileEntry, keyIdentifier, ska, derivedPassword));

			return (success: true, possibleError: "");
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

			SymmetricKeyAlgorithm ska = SymmetricKeyAlgorithm.GenerateNew(algorithm);

			byte[] derivedPassword = this.FindKeyDerivationFunctionEntryWithKeyIdentifier(keyIdentifier).GeneratePasswordBytes(password);

			this.contactSecrets.Add(new ContactSecret(contact, keyIdentifier, ska, derivedPassword));

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

			SymmetricKeyAlgorithm ska = SymmetricKeyAlgorithm.GenerateNew(algorithm);

			this.contactSecrets.Add(new ContactSecret(contact, keyIdentifier, ska, derivedPassword));

			return (success: true, possibleError: "");
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


		private (bool success, string possibleError) MandatoryChecks(object checkForNull, string objectToCheckForError, string keyIdentifier, string password)
		{
			(bool commonCheckResult, string possibleCommonCheckError) = this.MandatoryCommonChecks(checkForNull, objectToCheckForError, keyIdentifier);

			if (!commonCheckResult)
			{
				return (commonCheckResult, possibleCommonCheckError);
			}

			if (string.IsNullOrEmpty(password))
			{
				return (false, "Password must contain something!");
			}

			return (success: true, possibleError: "");
		}

		private (bool success, string possibleError) MandatoryChecks(object checkForNull, string objectToCheckForError, string keyIdentifier, byte[] derivedPassword)
		{
			(bool commonCheckResult, string possibleCommonCheckError) = this.MandatoryCommonChecks(checkForNull, objectToCheckForError, keyIdentifier);

			if (!commonCheckResult)
			{
				return (commonCheckResult, possibleCommonCheckError);
			}

			(bool derivedPasswordValid, Exception exception) = Helpers.CheckDerivedPassword(derivedPassword);

			if (!derivedPasswordValid)
			{
				return (derivedPasswordValid, exception.ToString());
			}

			return (success: true, possibleError: "");
		}

		private (bool success, string possibleError) MandatoryCommonChecks(object checkForNull, string objectToCheckForError, string keyIdentifier)
		{
			if (checkForNull == null)
			{
				return (success: false, possibleError: $"{objectToCheckForError} cannot be null");
			}

			KeyDerivationFunctionEntry kdfe = this.FindKeyDerivationFunctionEntryWithKeyIdentifier(keyIdentifier);

			if (kdfe == null)
			{
				return (success: false, possibleError: $"Cannot find key identifier matching to: {keyIdentifier}");
			}

			return (success: true, possibleError: "");
		}

		#endregion // Helpers
	}
}
