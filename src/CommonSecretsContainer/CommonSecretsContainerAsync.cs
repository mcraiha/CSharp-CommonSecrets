#if ASYNC_WITH_CUSTOM

using System;
using System.Collections.Generic;

using System.Threading.Tasks;

namespace CSCommonSecrets
{
	/// <summary>
	/// CommonSecretsContainer stores all other data. e.g. KeyDerivationFunctionEntries, LoginInformations etc.
	/// </summary>
	public sealed partial class CommonSecretsContainer
	{
		#region Add helpers

		/// <summary>
		/// Add login information secret to Common secret container, async
		/// </summary>
		/// <param name="password">Plaintext password</param>
		/// <param name="loginInformation">Loginiformation to add</param>
		/// <param name="keyIdentifier">Key identifier</param>
		/// <param name="securityFunctions">Security functions</param>
		/// <param name="algorithm">Symmetric Encryption Algorithm to use</param>
		/// <returns>Tuple that tells if add was success, and possible error</returns>
		public async Task<(bool success, string possibleError)> AddLoginInformationSecretAsync(string password, LoginInformation loginInformation, string keyIdentifier, ISecurityAsyncFunctions securityFunctions, SymmetricEncryptionAlgorithm algorithm = SymmetricEncryptionAlgorithm.AES_CTR)
		{
			(bool checkResult, string possibleError) = MandatoryChecks(loginInformation, "LoginInformation", keyIdentifier, password);
			if (!checkResult)
			{
				return (checkResult, possibleError);
			}

			SymmetricKeyAlgorithm ska = SymmetricKeyAlgorithm.GenerateNew(algorithm, securityFunctions);

			byte[] derivedPassword = await this.FindKeyDerivationFunctionEntryWithKeyIdentifier(keyIdentifier).GeneratePasswordBytesAsync(password, securityFunctions);

			this.loginInformationSecrets.Add(await LoginInformationSecret.CreateLoginInformationSecretAsync(loginInformation, keyIdentifier, ska, derivedPassword, securityFunctions));

			return (success: true, possibleError: "");
		}

		/// <summary>
		/// Add login information secret to Common secret container, async
		/// </summary>
		/// <param name="derivedPassword">Derived password</param>
		/// <param name="loginInformation">Loginiformation to add</param>
		/// <param name="keyIdentifier">Key identifier</param>
		/// <param name="securityFunctions">Security functions</param>
		/// <param name="algorithm">Symmetric Encryption Algorithm to use</param>
		/// <returns>Tuple that tells if add was success, and possible error</returns>
		public async Task<(bool success, string possibleError)> AddLoginInformationSecretAsync(byte[] derivedPassword, LoginInformation loginInformation, string keyIdentifier, ISecurityAsyncFunctions securityFunctions, SymmetricEncryptionAlgorithm algorithm = SymmetricEncryptionAlgorithm.AES_CTR)
		{
			(bool checkResult, string possibleError) = MandatoryChecks(loginInformation, "LoginInformation", keyIdentifier, derivedPassword);
			if (!checkResult)
			{
				return (checkResult, possibleError);
			}

			SymmetricKeyAlgorithm ska = SymmetricKeyAlgorithm.GenerateNew(algorithm, securityFunctions);

			this.loginInformationSecrets.Add(await LoginInformationSecret.CreateLoginInformationSecretAsync(loginInformation, keyIdentifier, ska, derivedPassword, securityFunctions));

			return (success: true, possibleError: "");
		}

		/// <summary>
		/// Add note secret to Common secret container, async
		/// </summary>
		/// <param name="password">Plaintext password</param>
		/// <param name="note">Note to add</param>
		/// <param name="keyIdentifier">Key identifier</param>
		/// <param name="securityFunctions">Security functions</param>
		/// <param name="algorithm">Symmetric Encryption Algorithm to use</param>
		/// <returns>Tuple that tells if add was success, and possible error</returns>
		public async Task<(bool success, string possibleError)> AddNoteSecretAsync(string password, Note note, string keyIdentifier, ISecurityAsyncFunctions securityFunctions, SymmetricEncryptionAlgorithm algorithm = SymmetricEncryptionAlgorithm.AES_CTR)
		{
			(bool checkResult, string possibleError) = MandatoryChecks(note, "Note", keyIdentifier, password);
			if (!checkResult)
			{
				return (checkResult, possibleError);
			}

			SymmetricKeyAlgorithm ska = SymmetricKeyAlgorithm.GenerateNew(algorithm, securityFunctions);

			byte[] derivedPassword = await this.FindKeyDerivationFunctionEntryWithKeyIdentifier(keyIdentifier).GeneratePasswordBytesAsync(password, securityFunctions);

			this.noteSecrets.Add(await NoteSecret.CreateNoteSecretAsync(note, keyIdentifier, ska, derivedPassword, securityFunctions));

			return (success: true, possibleError: "");
		}

		/// <summary>
		/// Add note secret to Common secret container, async
		/// </summary>
		/// <param name="derivedPassword">Derived password</param>
		/// <param name="note">Note to add</param>
		/// <param name="keyIdentifier">Key identifier</param>
		/// <param name="securityFunctions">Security functions</param>
		/// <param name="algorithm">Symmetric Encryption Algorithm to use</param>
		/// <returns>Tuple that tells if add was success, and possible error</returns>
		public async Task<(bool success, string possibleError)> AddNoteSecretAsync(byte[] derivedPassword, Note note, string keyIdentifier, ISecurityAsyncFunctions securityFunctions, SymmetricEncryptionAlgorithm algorithm = SymmetricEncryptionAlgorithm.AES_CTR)
		{
			(bool checkResult, string possibleError) = MandatoryChecks(note, "Note", keyIdentifier, derivedPassword);
			if (!checkResult)
			{
				return (checkResult, possibleError);
			}

			SymmetricKeyAlgorithm ska = SymmetricKeyAlgorithm.GenerateNew(algorithm, securityFunctions);

			this.noteSecrets.Add(await NoteSecret.CreateNoteSecretAsync(note, keyIdentifier, ska, derivedPassword, securityFunctions));

			return (success: true, possibleError: "");
		}

		/// <summary>
		/// Add file entry to Common secret container, async
		/// </summary>
		/// <param name="password">Plaintext password</param>
		/// <param name="fileEntry">File entry to add</param>
		/// <param name="keyIdentifier">Key identifier</param>
		/// <param name="securityFunctions">Security functions</param>
		/// <param name="algorithm">Symmetric Encryption Algorithm to use</param>
		/// <returns>Tuple that tells if add was success, and possible error</returns>
		public async Task<(bool success, string possibleError)> AddFileEntrySecretAsync(string password, FileEntry fileEntry, string keyIdentifier, ISecurityAsyncFunctions securityFunctions, SymmetricEncryptionAlgorithm algorithm = SymmetricEncryptionAlgorithm.AES_CTR)
		{
			(bool checkResult, string possibleError) = MandatoryChecks(fileEntry, "FileEntry", keyIdentifier, password);
			if (!checkResult)
			{
				return (checkResult, possibleError);
			}

			SymmetricKeyAlgorithm ska = SymmetricKeyAlgorithm.GenerateNew(algorithm, securityFunctions);

			byte[] derivedPassword = await this.FindKeyDerivationFunctionEntryWithKeyIdentifier(keyIdentifier).GeneratePasswordBytesAsync(password, securityFunctions);

			this.fileSecrets.Add(await FileEntrySecret.CreateFileEntrySecretAsync(fileEntry, keyIdentifier, ska, derivedPassword, securityFunctions));

			return (success: true, possibleError: "");
		}

		/// <summary>
		/// Add file entry to Common secret container, async
		/// </summary>
		/// <param name="derivedPassword">Derived password</param>
		/// <param name="fileEntry">File entry to add</param>
		/// <param name="keyIdentifier">Key identifier</param>
		/// <param name="securityFunctions">Security functions</param>
		/// <param name="algorithm">Symmetric Encryption Algorithm to use</param>
		/// <returns>Tuple that tells if add was success, and possible error</returns>
		public async Task<(bool success, string possibleError)> AddFileEntrySecretAsync(byte[] derivedPassword, FileEntry fileEntry, string keyIdentifier, ISecurityAsyncFunctions securityFunctions, SymmetricEncryptionAlgorithm algorithm = SymmetricEncryptionAlgorithm.AES_CTR)
		{
			(bool checkResult, string possibleError) = MandatoryChecks(fileEntry, "FileEntry", keyIdentifier, derivedPassword);
			if (!checkResult)
			{
				return (checkResult, possibleError);
			}

			SymmetricKeyAlgorithm ska = SymmetricKeyAlgorithm.GenerateNew(algorithm, securityFunctions);

			this.fileSecrets.Add(await FileEntrySecret.CreateFileEntrySecretAsync(fileEntry, keyIdentifier, ska, derivedPassword, securityFunctions));

			return (success: true, possibleError: "");
		}

		/// <summary>
		/// Add contact to Common secret container, async
		/// </summary>
		/// <param name="password">Plaintext password</param>
		/// <param name="contact">Contact to add</param>
		/// <param name="keyIdentifier">Key identifier</param>
		/// <param name="securityFunctions">Security functions</param>
		/// <param name="algorithm">Symmetric Encryption Algorithm to use</param>
		/// <returns>Tuple that tells if add was success, and possible error</returns>
		public async Task<(bool success, string possibleError)> AddContactSecretAsync(string password, Contact contact, string keyIdentifier, ISecurityAsyncFunctions securityFunctions, SymmetricEncryptionAlgorithm algorithm = SymmetricEncryptionAlgorithm.AES_CTR)
		{
			(bool checkResult, string possibleError) = MandatoryChecks(contact, "Contact", keyIdentifier, password);
			if (!checkResult)
			{
				return (checkResult, possibleError);
			}

			SymmetricKeyAlgorithm ska = SymmetricKeyAlgorithm.GenerateNew(algorithm, securityFunctions);

			byte[] derivedPassword = await this.FindKeyDerivationFunctionEntryWithKeyIdentifier(keyIdentifier).GeneratePasswordBytesAsync(password, securityFunctions);

			this.contactSecrets.Add(await ContactSecret.CreateContactSecretAsync(contact, keyIdentifier, ska, derivedPassword, securityFunctions));

			return (success: true, possibleError: "");
		}

		/// <summary>
		/// Add contact to Common secret container, async
		/// </summary>
		/// <param name="derivedPassword">Derived password</param>
		/// <param name="contact">Contact to add</param>
		/// <param name="keyIdentifier">Key identifier</param>
		/// <param name="securityFunctions">Security functions</param>
		/// <param name="algorithm">Symmetric Encryption Algorithm to use</param>
		/// <returns>Tuple that tells if add was success, and possible error</returns>
		public async Task<(bool success, string possibleError)> AddContactSecretAsync(byte[] derivedPassword, Contact contact, string keyIdentifier, ISecurityAsyncFunctions securityFunctions, SymmetricEncryptionAlgorithm algorithm = SymmetricEncryptionAlgorithm.AES_CTR)
		{
			(bool checkResult, string possibleError) = MandatoryChecks(contact, "Contact", keyIdentifier, derivedPassword);
			if (!checkResult)
			{
				return (checkResult, possibleError);
			}

			SymmetricKeyAlgorithm ska = SymmetricKeyAlgorithm.GenerateNew(algorithm, securityFunctions);

			this.contactSecrets.Add(await ContactSecret.CreateContactSecretAsync(contact, keyIdentifier, ska, derivedPassword, securityFunctions));

			return (success: true, possibleError: "");
		}

		/// <summary>
		/// Add payment card to Common secret container, async
		/// </summary>
		/// <param name="password">Plaintext password</param>
		/// <param name="paymentCard">Payment card to add</param>
		/// <param name="keyIdentifier">Key identifier</param>
		/// <param name="securityFunctions">Security functions</param>
		/// <param name="algorithm">Symmetric Encryption Algorithm to use</param>
		/// <returns>Tuple that tells if add was success, and possible error</returns>
		public async Task<(bool success, string possibleError)> AddPaymentCardSecretAsync(string password, PaymentCard paymentCard, string keyIdentifier, ISecurityAsyncFunctions securityFunctions, SymmetricEncryptionAlgorithm algorithm = SymmetricEncryptionAlgorithm.AES_CTR)
		{
			(bool checkResult, string possibleError) = MandatoryChecks(paymentCard, "PaymentCard", keyIdentifier, password);
			if (!checkResult)
			{
				return (checkResult, possibleError);
			}

			SymmetricKeyAlgorithm ska = SymmetricKeyAlgorithm.GenerateNew(algorithm, securityFunctions);

			byte[] derivedPassword = await this.FindKeyDerivationFunctionEntryWithKeyIdentifier(keyIdentifier).GeneratePasswordBytesAsync(password, securityFunctions);

			this.paymentCardSecrets.Add(await PaymentCardSecret.CreatePaymentCardSecretAsync(paymentCard, keyIdentifier, ska, derivedPassword, securityFunctions));

			return (success: true, possibleError: "");
		}

		/// <summary>
		/// Add payment card to Common secret container, async
		/// </summary>
		/// <param name="derivedPassword">Derived password</param>
		/// <param name="paymentCard">Payment card to add</param>
		/// <param name="keyIdentifier">Key identifier</param>
		/// <param name="securityFunctions">Security functions</param>
		/// <param name="algorithm">Symmetric Encryption Algorithm to use</param>
		/// <returns>Tuple that tells if add was success, and possible error</returns>
		public async Task<(bool success, string possibleError)> AddPaymentCardSecretAsync(byte[] derivedPassword, PaymentCard paymentCard, string keyIdentifier, ISecurityAsyncFunctions securityFunctions, SymmetricEncryptionAlgorithm algorithm = SymmetricEncryptionAlgorithm.AES_CTR)
		{
			(bool checkResult, string possibleError) = MandatoryChecks(paymentCard, "PaymentCard", keyIdentifier, derivedPassword);
			if (!checkResult)
			{
				return (checkResult, possibleError);
			}

			SymmetricKeyAlgorithm ska = SymmetricKeyAlgorithm.GenerateNew(algorithm, securityFunctions);

			this.paymentCardSecrets.Add(await PaymentCardSecret.CreatePaymentCardSecretAsync(paymentCard, keyIdentifier, ska, derivedPassword, securityFunctions));

			return (success: true, possibleError: "");
		}

		#endregion // Add helpers


		#region Replace helpers

		/// <summary>
		/// Replace existing login information secret in Common secret container with another one (basically for editing purposes), async
		/// </summary>
		/// <param name="zeroBasedIndex">Zero based index of login information secret that will be replaced</param>
		/// <param name="password">Plaintext password</param>
		/// <param name="loginInformation">Loginiformation to add</param>
		/// <param name="keyIdentifier">Key identifier</param>
		/// <param name="securityFunctions">Security functions</param>
		/// <param name="algorithm">Symmetric Encryption Algorithm to use</param>
		/// <returns>Tuple that tells if add was success, and possible error</returns>
		public async Task<(bool success, string possibleError)> ReplaceLoginInformationSecretAsync(int zeroBasedIndex, string password, LoginInformation loginInformation, string keyIdentifier, ISecurityAsyncFunctions securityFunctions, SymmetricEncryptionAlgorithm algorithm = SymmetricEncryptionAlgorithm.AES_CTR)
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

			SymmetricKeyAlgorithm ska = SymmetricKeyAlgorithm.GenerateNew(algorithm, securityFunctions);

			byte[] derivedPassword = await this.FindKeyDerivationFunctionEntryWithKeyIdentifier(keyIdentifier).GeneratePasswordBytesAsync(password, securityFunctions);

			this.loginInformationSecrets[zeroBasedIndex] = await LoginInformationSecret.CreateLoginInformationSecretAsync(loginInformation, keyIdentifier, ska, derivedPassword, securityFunctions);

			return (success: true, possibleError: "");
		}

		/// <summary>
		/// Replace existing login information secret in Common secret container with another one (basically for editing purposes), async
		/// </summary>
		/// <param name="zeroBasedIndex">Zero based index of login information secret that will be replaced</param>
		/// <param name="derivedPassword">Derived password</param>
		/// <param name="loginInformation">Loginiformation to add</param>
		/// <param name="keyIdentifier">Key identifier</param>
		/// <param name="securityFunctions">Security functions</param>
		/// <param name="algorithm">Symmetric Encryption Algorithm to use</param>
		/// <returns>Tuple that tells if add was success, and possible error</returns>
		public async Task<(bool success, string possibleError)> ReplaceLoginInformationSecretAsync(int zeroBasedIndex, byte[] derivedPassword, LoginInformation loginInformation, string keyIdentifier, ISecurityAsyncFunctions securityFunctions, SymmetricEncryptionAlgorithm algorithm = SymmetricEncryptionAlgorithm.AES_CTR)
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

			SymmetricKeyAlgorithm ska = SymmetricKeyAlgorithm.GenerateNew(algorithm, securityFunctions);

			this.loginInformationSecrets[zeroBasedIndex] = await LoginInformationSecret.CreateLoginInformationSecretAsync(loginInformation, keyIdentifier, ska, derivedPassword, securityFunctions);

			return (success: true, possibleError: "");
		}

		/// <summary>
		/// Replace existing note secret in Common secret container with another one (basically for editing purposes), async
		/// </summary>
		/// <param name="zeroBasedIndex">Zero based index of note secret that will be replaced</param>
		/// <param name="password">Plaintext password</param>
		/// <param name="note">Note to add</param>
		/// <param name="keyIdentifier">Key identifier</param>
		/// <param name="securityFunctions">Security functions</param>
		/// <param name="algorithm">Symmetric Encryption Algorithm to use</param>
		/// <returns>Tuple that tells if add was success, and possible error</returns>
		public async Task<(bool success, string possibleError)> ReplaceNoteSecretAsync(int zeroBasedIndex, string password, Note note, string keyIdentifier, ISecurityAsyncFunctions securityFunctions, SymmetricEncryptionAlgorithm algorithm = SymmetricEncryptionAlgorithm.AES_CTR)
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

			SymmetricKeyAlgorithm ska = SymmetricKeyAlgorithm.GenerateNew(algorithm, securityFunctions);

			byte[] derivedPassword = await this.FindKeyDerivationFunctionEntryWithKeyIdentifier(keyIdentifier).GeneratePasswordBytesAsync(password, securityFunctions);

			this.noteSecrets[zeroBasedIndex] = await NoteSecret.CreateNoteSecretAsync(note, keyIdentifier, ska, derivedPassword, securityFunctions);

			return (success: true, possibleError: "");
		}

		/// <summary>
		/// Replace existing note secret in Common secret container with another one (basically for editing purposes), async
		/// </summary>
		/// <param name="zeroBasedIndex">Zero based index of note secret that will be replaced</param>
		/// <param name="derivedPassword">Derived password</param>
		/// <param name="note">Note to add</param>
		/// <param name="keyIdentifier">Key identifier</param>
		/// <param name="securityFunctions">Security functions</param>
		/// <param name="algorithm">Symmetric Encryption Algorithm to use</param>
		/// <returns>Tuple that tells if add was success, and possible error</returns>
		public async Task<(bool success, string possibleError)> ReplaceNoteSecretAsync(int zeroBasedIndex, byte[] derivedPassword, Note note, string keyIdentifier, ISecurityAsyncFunctions securityFunctions, SymmetricEncryptionAlgorithm algorithm = SymmetricEncryptionAlgorithm.AES_CTR)
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

			SymmetricKeyAlgorithm ska = SymmetricKeyAlgorithm.GenerateNew(algorithm, securityFunctions);

			this.noteSecrets[zeroBasedIndex] = await NoteSecret.CreateNoteSecretAsync(note, keyIdentifier, ska, derivedPassword, securityFunctions);

			return (success: true, possibleError: "");
		}

		/// <summary>
		/// Replace existing file entry in Common secret container with another one (basically for editing purposes), async
		/// </summary>
		/// <param name="zeroBasedIndex">Zero based index of file entry secret that will be replaced</param>
		/// <param name="password">Plaintext password</param>
		/// <param name="fileEntry">File entry to add</param>
		/// <param name="keyIdentifier">Key identifier</param>
		/// <param name="securityFunctions">Security functions</param>
		/// <param name="algorithm">Symmetric Encryption Algorithm to use</param>
		/// <returns>Tuple that tells if add was success, and possible error</returns>
		public async Task<(bool success, string possibleError)> ReplaceFileEntrySecretAsync(int zeroBasedIndex, string password, FileEntry fileEntry, string keyIdentifier, ISecurityAsyncFunctions securityFunctions, SymmetricEncryptionAlgorithm algorithm = SymmetricEncryptionAlgorithm.AES_CTR)
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

			SymmetricKeyAlgorithm ska = SymmetricKeyAlgorithm.GenerateNew(algorithm, securityFunctions);

			byte[] derivedPassword = await this.FindKeyDerivationFunctionEntryWithKeyIdentifier(keyIdentifier).GeneratePasswordBytesAsync(password, securityFunctions);

			this.fileSecrets[zeroBasedIndex] = await FileEntrySecret.CreateFileEntrySecretAsync(fileEntry, keyIdentifier, ska, derivedPassword, securityFunctions);

			return (success: true, possibleError: "");
		}

		/// <summary>
		/// Replace existing file entry in Common secret container with another one (basically for editing purposes), async
		/// </summary>
		/// <param name="zeroBasedIndex">Zero based index of file entry secret that will be replaced</param>
		/// <param name="derivedPassword">Derived password</param>
		/// <param name="fileEntry">File entry to add</param>
		/// <param name="keyIdentifier">Key identifier</param>
		/// <param name="securityFunctions">Security functions</param>
		/// <param name="algorithm">Symmetric Encryption Algorithm to use</param>
		/// <returns>Tuple that tells if add was success, and possible error</returns>
		public async Task<(bool success, string possibleError)> ReplaceFileEntrySecretAsync(int zeroBasedIndex, byte[] derivedPassword, FileEntry fileEntry, string keyIdentifier, ISecurityAsyncFunctions securityFunctions, SymmetricEncryptionAlgorithm algorithm = SymmetricEncryptionAlgorithm.AES_CTR)
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

			SymmetricKeyAlgorithm ska = SymmetricKeyAlgorithm.GenerateNew(algorithm, securityFunctions);

			this.fileSecrets[zeroBasedIndex] = await FileEntrySecret.CreateFileEntrySecretAsync(fileEntry, keyIdentifier, ska, derivedPassword, securityFunctions);

			return (success: true, possibleError: "");
		}

		/// <summary>
		/// Replace existing contact in Common secret container with another one (basically for editing purposes), async
		/// </summary>
		/// <param name="zeroBasedIndex">Zero based index of contact secret that will be replaced</param>
		/// <param name="password">Plaintext password</param>
		/// <param name="contact">Contact to add</param>
		/// <param name="keyIdentifier">Key identifier</param>
		/// <param name="securityFunctions">Security functions</param>
		/// <param name="algorithm">Symmetric Encryption Algorithm to use</param>
		/// <returns>Tuple that tells if add was success, and possible error</returns>
		public async Task<(bool success, string possibleError)> ReplaceContactSecretAsync(int zeroBasedIndex, string password, Contact contact, string keyIdentifier, ISecurityAsyncFunctions securityFunctions, SymmetricEncryptionAlgorithm algorithm = SymmetricEncryptionAlgorithm.AES_CTR)
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

			SymmetricKeyAlgorithm ska = SymmetricKeyAlgorithm.GenerateNew(algorithm, securityFunctions);

			byte[] derivedPassword = await this.FindKeyDerivationFunctionEntryWithKeyIdentifier(keyIdentifier).GeneratePasswordBytesAsync(password, securityFunctions);

			this.contactSecrets[zeroBasedIndex] = await ContactSecret.CreateContactSecretAsync(contact, keyIdentifier, ska, derivedPassword, securityFunctions);

			return (success: true, possibleError: "");
		}

		/// <summary>
		/// Replace existing contact in Common secret container with another one (basically for editing purposes), async
		/// </summary>
		/// <param name="zeroBasedIndex">Zero based index of contact secret that will be replaced</param>
		/// <param name="derivedPassword">Derived password</param>
		/// <param name="contact">Contact to add</param>
		/// <param name="keyIdentifier">Key identifier</param>
		/// <param name="securityFunctions">Security functions</param>
		/// <param name="algorithm">Symmetric Encryption Algorithm to use</param>
		/// <returns>Tuple that tells if add was success, and possible error</returns>
		public async Task<(bool success, string possibleError)> ReplaceContactSecretAsync(int zeroBasedIndex, byte[] derivedPassword, Contact contact, string keyIdentifier, ISecurityAsyncFunctions securityFunctions, SymmetricEncryptionAlgorithm algorithm = SymmetricEncryptionAlgorithm.AES_CTR)
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

			SymmetricKeyAlgorithm ska = SymmetricKeyAlgorithm.GenerateNew(algorithm, securityFunctions);

			this.contactSecrets[zeroBasedIndex] = await ContactSecret.CreateContactSecretAsync(contact, keyIdentifier, ska, derivedPassword, securityFunctions);

			return (success: true, possibleError: "");
		}

		/// <summary>
		/// Replace existing payment card in Common secret container with another one (basically for editing purposes), async
		/// </summary>
		/// <param name="zeroBasedIndex">Zero based index of payment card secret that will be replaced</param>
		/// <param name="password">Plaintext password</param>
		/// <param name="paymentCard">Payment card to add</param>
		/// <param name="keyIdentifier">Key identifier</param>
		/// <param name="securityFunctions">Security functions</param>
		/// <param name="algorithm">Symmetric Encryption Algorithm to use</param>
		/// <returns>Tuple that tells if add was success, and possible error</returns>
		public async Task<(bool success, string possibleError)> ReplacePaymentCardSecretAsync(int zeroBasedIndex, string password, PaymentCard paymentCard, string keyIdentifier, ISecurityAsyncFunctions securityFunctions, SymmetricEncryptionAlgorithm algorithm = SymmetricEncryptionAlgorithm.AES_CTR)
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

			SymmetricKeyAlgorithm ska = SymmetricKeyAlgorithm.GenerateNew(algorithm, securityFunctions);

			byte[] derivedPassword = await this.FindKeyDerivationFunctionEntryWithKeyIdentifier(keyIdentifier).GeneratePasswordBytesAsync(password, securityFunctions);

			this.paymentCardSecrets[zeroBasedIndex] = await PaymentCardSecret.CreatePaymentCardSecretAsync(paymentCard, keyIdentifier, ska, derivedPassword, securityFunctions);

			return (success: true, possibleError: "");
		}

		/// <summary>
		/// Replace existing payment card in Common secret container with another one (basically for editing purposes), async
		/// </summary>
		/// <param name="zeroBasedIndex">Zero based index of payment card secret that will be replaced</param>
		/// <param name="derivedPassword">Derived password</param>
		/// <param name="paymentCard">Payment card to add</param>
		/// <param name="keyIdentifier">Key identifier</param>
		/// <param name="securityFunctions">Security functions</param>
		/// <param name="algorithm">Symmetric Encryption Algorithm to use</param>
		/// <returns>Tuple that tells if add was success, and possible error</returns>
		public async Task<(bool success, string possibleError)> ReplacePaymentCardSecretAsync(int zeroBasedIndex, byte[] derivedPassword, PaymentCard paymentCard, string keyIdentifier, ISecurityAsyncFunctions securityFunctions, SymmetricEncryptionAlgorithm algorithm = SymmetricEncryptionAlgorithm.AES_CTR)
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

			SymmetricKeyAlgorithm ska = SymmetricKeyAlgorithm.GenerateNew(algorithm, securityFunctions);

			this.paymentCardSecrets[zeroBasedIndex] = await PaymentCardSecret.CreatePaymentCardSecretAsync(paymentCard, keyIdentifier, ska, derivedPassword, securityFunctions);

			return (success: true, possibleError: "");
		}

		#endregion // Replace helpers
	}
}

#endif // ASYNC_WITH_CUSTOM