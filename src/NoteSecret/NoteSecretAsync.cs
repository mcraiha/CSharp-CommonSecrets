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

		#region Common getters

		public async Task<string> GetNoteTitleAsync(byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
		{
			return (string)await Helpers.GetSingleValueAsync(this.audalfData, this.algorithm, derivedPassword, Note.noteTitleKey, deserializationSettings, securityFunctions);
		}

		#endregion // Common getters


		#region Common setters


		#endregion // Common setters


		#region Checksum

		private async Task<string> CalculateHexChecksumAsync(ISecurityAsyncFunctions securityFunctions)
		{
			return await ChecksumHelper.CalculateHexChecksumAsync(securityFunctions, this.keyIdentifier, this.audalfData, algorithm.GetSettingsAsBytes());
		}

		private async Task CalculateAndUpdateChecksumASync(ISecurityAsyncFunctions securityFunctions)
		{
			this.checksum = await this.CalculateHexChecksumAsync(securityFunctions);
		}

		#endregion // Checksum
	}
}

#endif // ASYNC_WITH_CUSTOM