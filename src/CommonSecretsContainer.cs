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

		/// <summary>
		/// Constructor without parameters for creating empty CommonSecretsContainer
		/// </summary>
		public CommonSecretsContainer()
		{
			this.version = currentVersionNumber;
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

		#endregion // Helpers
	}
}
