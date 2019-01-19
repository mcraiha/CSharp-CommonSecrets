using System;
using System.Collections.Generic;

namespace CSCommonSecrets
{
	public sealed class CommonSecretsContainer
	{
		public readonly int currentVersionNumber = 1;

		public int version;

		public List<KeyDerivationFunctionEntry> keyDerivationFunctionEntries = new List<KeyDerivationFunctionEntry>();

		public List<LoginInformation> loginInformations = new List<LoginInformation>();
		public List<LoginInformationSecret> loginInformationSecrets = new List<LoginInformationSecret>();

		public List<Note> notes = new List<Note>();
		public List<NoteSecret> noteSecrets = new List<NoteSecret>();

		public CommonSecretsContainer()
		{
			this.version = currentVersionNumber;
		}
	}
}
