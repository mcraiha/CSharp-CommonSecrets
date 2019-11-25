using System;
using System.Collections.Generic;

namespace CSCommonSecrets
{
	public sealed class CommonSecretsContainer
	{
		public static readonly int currentVersionNumber = 1;

		public int version { get; set; }

		public List<KeyDerivationFunctionEntry> keyDerivationFunctionEntries { get; set; } = new List<KeyDerivationFunctionEntry>();

		// Login informations
		public List<LoginInformation> loginInformations { get; set; } = new List<LoginInformation>();
		public List<LoginInformationSecret> loginInformationSecrets { get; set; } = new List<LoginInformationSecret>();

		// Notes
		public List<Note> notes { get; set; } = new List<Note>();
		public List<NoteSecret> noteSecrets { get; set; } = new List<NoteSecret>();

		// Files
		public List<FileEntry> files { get; set; } = new List<FileEntry>();
		public List<FileEntrySecret> fileSecrets { get; set; } = new List<FileEntrySecret>();

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
