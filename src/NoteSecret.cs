using System;
using System.Collections.Generic;
using System.Text;
using CSharp_AUDALF;

namespace CSCommonSecrets
{
	public sealed class NoteSecret
	{
		public byte[] keyIdentifier { get; set; }

		public byte[] audalfData { get; set; } = new byte[0];

		public SymmetricKeyAlgorithm algorithm { get; set; }
		public string checksum { get; set; } = string.Empty;

		/// <summary>
		/// For deserialization
		/// </summary>
		public NoteSecret()
		{

		}

		public NoteSecret(Note note, string keyIdentifier, SymmetricKeyAlgorithm algorithm, byte[] derivedPassword)
		{
			Dictionary<string, object> dictionaryForAUDALF = new Dictionary<string, object>()
			{
				{ Note.noteTitleKey, note.GetNoteTitle() },
				{ Note.noteTextKey, note.GetNoteText() },
				{ Note.creationTimeKey, DateTimeOffset.FromUnixTimeSeconds(note.creationTime) },
				{ Note.modificationTimeKey, DateTimeOffset.FromUnixTimeSeconds(note.modificationTime) },
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

		private static readonly SerializationSettings serializationSettings = new SerializationSettings() { dateTimeFormat = DateTimeFormat.UnixInSeconds };

		public NoteSecret(Dictionary<string, object> noteAsDictionary, string keyIdentifier, SymmetricKeyAlgorithm algorithm, byte[] derivedPassword)
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

		public DateTimeOffset GetCreationTime(byte[] derivedPassword)
		{
			Dictionary<string, object> noteAsDictionary = this.GetNoteAsDictionary(derivedPassword);
			return (DateTimeOffset)noteAsDictionary[Note.creationTimeKey];
		}

		public DateTimeOffset GetModificationTime(byte[] derivedPassword)
		{
			Dictionary<string, object> noteAsDictionary = this.GetNoteAsDictionary(derivedPassword);
			return (DateTimeOffset)noteAsDictionary[Note.modificationTimeKey];
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

			Dictionary<string, object> noteAsDictionary = AUDALF_Deserialize.Deserialize<string, object>(decryptedAUDALF, settings: new DeserializationSettings() { wantedDateTimeType = typeof(DateTimeOffset) });

			return noteAsDictionary;
		}

		public string GetKeyIdentifier()
		{
			return System.Text.Encoding.UTF8.GetString(this.keyIdentifier);
		}

		#endregion // Common getters


		#region Common setters

		public void SetNoteTitle(byte[] derivedPassword, string newNoteTitle)
		{
			Dictionary<string, object> noteAsDictionary = this.GetNoteAsDictionary(derivedPassword);
			noteAsDictionary[Note.noteTitleKey] = newNoteTitle;
			noteAsDictionary[Note.modificationTimeKey] = DateTimeOffset.UtcNow;

			// Create AUDALF payload from dictionary
			byte[] serializedBytes = AUDALF_Serialize.Serialize(noteAsDictionary, valueTypes: null, serializationSettings: serializationSettings );

			// Encrypt the AUDALF payload with given algorithm
			this.audalfData = algorithm.EncryptBytes(serializedBytes, derivedPassword);

			// Calculate new checksum
			this.CalculateAndUpdateChecksum();
		}

		#endregion // Common setters


		#region Checksum

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