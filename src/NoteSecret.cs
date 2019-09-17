using System;
using System.Collections.Generic;
using CSharp_AUDALF;

namespace CSCommonSecrets
{
	public sealed class NoteSecret
	{
		public byte[] audalfData { get; set; } = new byte[0];

		public SymmetricKeyAlgorithm algorithm { get; set; }
		private string checksum = string.Empty;

		/// <summary>
		/// For deserialization
		/// </summary>
		public NoteSecret()
		{

		}

		public NoteSecret(Note note, SymmetricKeyAlgorithm algorithm, byte[] derivedPassword)
		{
			Dictionary<string, object> dictionaryForAUDALF = new Dictionary<string, object>()
			{
				{ Note.noteTitleKey, note.GetNoteTitle() },
				{ Note.noteTextKey, note.GetNoteText() },
				{ Note.creationTimeKey, note.creationTime },
				{ Note.modificationTimeKey, note.modificationTime },
			};

			this.algorithm = algorithm;

			// Create AUDALF payload from dictionary
			byte[] serializedBytes = AUDALF_Serialize.Serialize(dictionaryForAUDALF, valueTypes: null, serializationSettings: serializationSettings );

			// Encrypt the AUDALF payload with given algorithm
			this.audalfData = algorithm.EncryptBytes(serializedBytes, derivedPassword);
		}

		private static readonly SerializationSettings serializationSettings = new SerializationSettings() { dateTimeFormat = DateTimeFormat.UnixInSeconds };

		public NoteSecret(Dictionary<string, object> noteAsDictionary, SymmetricKeyAlgorithm algorithm, byte[] derivedPassword)
		{
			this.algorithm = algorithm;

			// Create AUDALF payload from dictionary
			byte[] serializedBytes = AUDALF_Serialize.Serialize(noteAsDictionary, valueTypes: null, serializationSettings: serializationSettings );

			// Encrypt the AUDALF payload with given algorithm
			this.audalfData = algorithm.EncryptBytes(serializedBytes, derivedPassword);
		}

		#region Common getters

		public string GetNoteTitle(byte[] derivedPassword)
		{
			Dictionary<string, object> noteAsDictionary = this.GetNoteAsDictionary(derivedPassword);
			return (string)noteAsDictionary[Note.noteTitleKey];
		}

		public string GetNoteText(byte[] derivedPassword)
		{
			Dictionary<string, object> noteAsDictionary = this.GetNoteAsDictionary(derivedPassword);
			return (string)noteAsDictionary[Note.noteTextKey];
		}

		private Dictionary<string, object> GetNoteAsDictionary(byte[] derivedPassword)
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

			Dictionary<string, object> noteAsDictionary = AUDALF_Deserialize.Deserialize<string, object>(decryptedAUDALF);

			return noteAsDictionary;
		}

		#endregion // Common getters


		#region Checksum

		public string GetChecksumAsHex()
		{
			return this.checksum;
		}

		private string CalculateHexChecksum()
		{
			return ChecksumHelper.CalculateHexChecksum(this.audalfData, algorithm.GetSettingsAsBytes());
		}

		private void CalculateAndUpdateChecksum()
		{
			this.checksum = this.CalculateHexChecksum();
		}

		#endregion // Checksum
	}
}