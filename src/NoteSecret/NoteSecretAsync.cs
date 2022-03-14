#if ASYNC_WITH_CUSTOM

using System;
using System.Collections.Generic;
using System.Text;
using CSharp_AUDALF;
using System.Threading.Tasks;

namespace CSCommonSecrets
{
	/// <summary>
	/// NoteSecret stores one encrypted note. Note is basically a text file
	/// </summary>
	public sealed partial class NoteSecret
	{
		/// <summary>
		/// Create NoteSecret, async
		/// </summary>
		/// <param name="note">Note</param>
		/// <param name="keyIdentifier">Key identifier</param>
		/// <param name="algorithm">Symmetric Key Algorithm</param>
		/// <param name="derivedPassword">Derived password</param>
		/// <param name="securityFunctions">Security functions</param>
		/// <returns>NoteSecret</returns>
		public static async Task<NoteSecret> CreateNoteSecretAsync(Note note, string keyIdentifier, SymmetricKeyAlgorithm algorithm, byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
		{
			Dictionary<string, object> dictionaryForAUDALF = new Dictionary<string, object>()
			{
				{ Note.noteTitleKey, note.GetNoteTitle() },
				{ Note.noteTextKey, note.GetNoteText() },
				{ Note.creationTimeKey, DateTimeOffset.FromUnixTimeSeconds(note.creationTime) },
				{ Note.modificationTimeKey, DateTimeOffset.FromUnixTimeSeconds(note.modificationTime) },
			};

			return await CreateNoteSecretAsync(dictionaryForAUDALF, keyIdentifier, algorithm, derivedPassword, securityFunctions);
		}

		/// <summary>
		/// Create NoteSecret, async
		/// </summary>
		/// <param name="noteAsDictionary">Note as dictionary</param>
		/// <param name="keyIdentifier">Key identifier</param>
		/// <param name="algorithm">Symmetric Key Algorithm</param>
		/// <param name="derivedPassword">Derived password</param>
		/// <param name="securityFunctions">Security functions</param>
		/// <returns>NoteSecret</returns>
		public static async Task<NoteSecret> CreateNoteSecretAsync(Dictionary<string, object> noteAsDictionary, string keyIdentifier, SymmetricKeyAlgorithm algorithm, byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
		{
			NoteSecret noteSecret = new NoteSecret();

			noteSecret.keyIdentifier = Encoding.UTF8.GetBytes(keyIdentifier);

			noteSecret.algorithm = algorithm;

			// Create AUDALF payload from dictionary
			byte[] serializedBytes = AUDALF_Serialize.Serialize(noteAsDictionary, valueTypes: null, serializationSettings: serializationSettings );

			// Encrypt the AUDALF payload with given algorithm
			noteSecret.audalfData = await algorithm.EncryptBytesAsync(serializedBytes, derivedPassword, securityFunctions);

			// Calculate new checksum
			await noteSecret.CalculateAndUpdateChecksumAsync(securityFunctions);

			return noteSecret;
		}

		#region Common getters

		/// <summary>
		/// Get Note. Use this for situation where you want to convert secret -> non secret, async
		/// </summary>
		/// <param name="derivedPassword">Derived password</param>
		/// <param name="securityFunctions">Security functions</param>
		/// <returns>Note</returns>
		public async Task<Note> GetNoteAsync(byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
		{
			Dictionary<string, object> dict = await this.GetNoteAsDictionaryAsync(derivedPassword, securityFunctions);
			Note returnValue = await Note.CreateNoteAsync((string)dict[Note.noteTitleKey], (string)dict[Note.noteTextKey], securityFunctions);

			returnValue.creationTime = ((DateTimeOffset)dict[LoginInformation.creationTimeKey]).ToUnixTimeSeconds();
			returnValue.modificationTime = ((DateTimeOffset)dict[LoginInformation.modificationTimeKey]).ToUnixTimeSeconds();

			return returnValue;
		}

		/// <summary>
		/// Get note title, async
		/// </summary>
		/// <param name="derivedPassword">Derived password</param>
		/// <param name="securityFunctions">Security functions</param>
		/// <returns>Note title</returns>
		public async Task<string> GetNoteTitleAsync(byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
		{
			return (string)await Helpers.GetSingleValueAsync(this.audalfData, this.algorithm, derivedPassword, Note.noteTitleKey, deserializationSettings, securityFunctions);
		}

		/// <summary>
		/// Get note text, async
		/// </summary>
		/// <param name="derivedPassword">Derived password</param>
		/// <param name="securityFunctions">Security functions</param>
		/// <returns>Note text</returns>
		public async Task<string> GetNoteTextAsync(byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
		{
			return (string)await Helpers.GetSingleValueAsync(this.audalfData, this.algorithm, derivedPassword, Note.noteTextKey, deserializationSettings, securityFunctions);
		}

		/// <summary>
		/// Get note creation time, async
		/// </summary>
		/// <param name="derivedPassword">Derived password</param>
		/// <param name="securityFunctions">Security functions</param>
		/// <returns>Note creation time</returns>
		public async Task<DateTimeOffset> GetCreationTimeAsync(byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
		{
			return (DateTimeOffset)await Helpers.GetSingleValueAsync(this.audalfData, this.algorithm, derivedPassword, Note.creationTimeKey, deserializationSettings, securityFunctions);
		}

		/// <summary>
		/// Get note modification time, async
		/// </summary>
		/// <param name="derivedPassword">Derived password</param>
		/// <param name="securityFunctions">Security functions</param>
		/// <returns>Note modification time</returns>
		public async Task<DateTimeOffset> GetModificationTimeAsync(byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
		{
			return (DateTimeOffset)await Helpers.GetSingleValueAsync(this.audalfData, this.algorithm, derivedPassword, Note.modificationTimeKey, deserializationSettings, securityFunctions);
		}

		private async Task<Dictionary<string, object>> GetNoteAsDictionaryAsync(byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
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

			Dictionary<string, object> noteAsDictionary = AUDALF_Deserialize.Deserialize<string, object>(decryptedAUDALF, settings: deserializationSettings);

			return noteAsDictionary;
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
		/// Set note title, async
		/// </summary>
		/// <param name="newNoteTitle">New title</param>
		/// <param name="derivedPassword">Derived password</param>
		/// <param name="securityFunctions">Security functions</param>
		/// <returns>True if set was success; False otherwise</returns>
		public async Task<bool> SetNoteTitleAsync(string newNoteTitle, byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
		{
			return await this.GenericSet(Note.noteTitleKey, newNoteTitle, DateTimeOffset.UtcNow, derivedPassword, securityFunctions);
		}

		/// <summary>
		/// Set note text, async
		/// </summary>
		/// <param name="newNoteText">New text</param>
		/// <param name="derivedPassword">Derived password</param>
		/// <param name="securityFunctions">Security functions</param>
		/// <returns>True if set was success; False otherwise</returns>
		public async Task<bool> SetNoteTextAsync(string newNoteText, byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
		{
			return await this.GenericSet(Note.noteTextKey, newNoteText, DateTimeOffset.UtcNow, derivedPassword, securityFunctions);
		}

		private async Task<bool> GenericSet(string key, object value, DateTimeOffset modificationTime, byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
		{
			try 
			{
				Dictionary<string, object> noteAsDictionary = await this.GetNoteAsDictionaryAsync(derivedPassword, securityFunctions);
				// Update wanted value
				noteAsDictionary[key] = value;
				// Update modification time
				noteAsDictionary[Note.modificationTimeKey] = modificationTime;

				// Generate new algorithm since data has changed
				this.algorithm = SymmetricKeyAlgorithm.GenerateNew(this.algorithm.GetSymmetricEncryptionAlgorithm(), securityFunctions);

				// Create AUDALF payload from dictionary
				byte[] serializedBytes = AUDALF_Serialize.Serialize(noteAsDictionary, valueTypes: null, serializationSettings: serializationSettings );

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