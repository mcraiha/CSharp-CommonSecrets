using System;
using System.Collections.Generic;

namespace CSCommonSecrets
{
	public sealed class CommonSecretsContainer
	{
		public static readonly int currentVersionNumber = 1;

		public int version;

		public List<KeyDerivationFunctionEntry> keyDerivationFunctionEntries = new List<KeyDerivationFunctionEntry>();

		// Login informations
		public List<LoginInformation> loginInformations = new List<LoginInformation>();
		public List<LoginInformationSecret> loginInformationSecrets = new List<LoginInformationSecret>();

		// Notes
		public List<Note> notes = new List<Note>();
		public List<NoteSecret> noteSecrets = new List<NoteSecret>();

		// Files
		public List<FileEntry> files = new List<FileEntry>();
		public List<FileEntrySecret> fileSecrets = new List<FileEntrySecret>();

		public CommonSecretsContainer()
		{
			this.version = currentVersionNumber;
		}
	}
}
