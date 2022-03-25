#if ASYNC_WITH_CUSTOM

using System;
using System.Text;

using System.Threading.Tasks;

namespace CSCommonSecrets
{
	/// <summary>
	/// LoginInformation stores one plaintext (anyone can read) login information
	/// </summary>
	public sealed partial class LoginInformation
	{
		/// <summary>
		/// Default small LoginInformation constructor, async
		/// </summary>
		/// <param name="newTitle">Title</param>
		/// <param name="newUrl">URL</param>
		/// <param name="newEmail">Email</param>
		/// <param name="newUsername">Username</param>
		/// <param name="newPassword">Password</param>
		/// <param name="securityFunctions">Security functions</param>
		public static async Task<LoginInformation> CreateLoginInformationAsync(string newTitle, string newUrl, string newEmail, string newUsername, string newPassword, ISecurityAsyncFunctions securityFunctions)
		{
			return await CreateLoginInformationAsync(newTitle, newUrl, newEmail, newUsername, newPassword, DateTimeOffset.UtcNow, securityFunctions);
		}

		/// <summary>
		/// Small LoginInformation constructor, async
		/// </summary>
		/// <param name="newTitle">Title</param>
		/// <param name="newUrl">URL</param>
		/// <param name="newEmail">Email</param>
		/// <param name="newUsername">Username</param>
		/// <param name="newPassword">Password</param>
		/// <param name="time">Creation and modification timestamps</param>
		/// <param name="securityFunctions">Security functions</param>
		public static async Task<LoginInformation> CreateLoginInformationAsync(string newTitle, string newUrl, string newEmail, string newUsername, string newPassword, DateTimeOffset time, ISecurityAsyncFunctions securityFunctions)
		{
			LoginInformation loginInformation = new LoginInformation();
			loginInformation.title = Encoding.UTF8.GetBytes(newTitle);
			loginInformation.url = Encoding.UTF8.GetBytes(newUrl);
			loginInformation.email = Encoding.UTF8.GetBytes(newEmail);
			loginInformation.username = Encoding.UTF8.GetBytes(newUsername);
			loginInformation.password = Encoding.UTF8.GetBytes(newPassword);
			loginInformation.creationTime = time.ToUnixTimeSeconds();
			loginInformation.modificationTime = time.ToUnixTimeSeconds();

			await loginInformation.CalculateAndUpdateChecksumAsync(securityFunctions);

			return loginInformation;
		}

		/// <summary>
		/// Default full LoginInformation constructor, async
		/// </summary>
		/// <param name="newTitle">Title</param>
		/// <param name="newUrl">URL</param>
		/// <param name="newEmail">Email</param>
		/// <param name="newUsername">Username</param>
		/// <param name="newPassword">Password</param>
		/// <param name="newNotes">Notes</param>
		/// <param name="newMFA">MFA</param>
		/// <param name="newIcon">Icon</param>
		/// <param name="newCategory">Category</param>
		/// <param name="newTags">Tags (as tab separated)</param>
		/// <param name="securityFunctions">Security functions</param>
		public static async Task<LoginInformation> CreateLoginInformationAsync(string newTitle, string newUrl, string newEmail, string newUsername, string newPassword, string newNotes, string newMFA,
									byte[] newIcon, string newCategory, string newTags, ISecurityAsyncFunctions securityFunctions)
		{
			return await CreateLoginInformationAsync(newTitle, newUrl, newEmail, newUsername, newPassword, newNotes, newMFA, newIcon, newCategory, newTags, DateTimeOffset.UtcNow, securityFunctions);
		}

		/// <summary>
		/// Full LoginInformation constructor, async
		/// </summary>
		/// <param name="newTitle">Title</param>
		/// <param name="newUrl">URL</param>
		/// <param name="newEmail">Email</param>
		/// <param name="newUsername">Username</param>
		/// <param name="newPassword">Password</param>
		/// <param name="newNotes">Notes</param>
		/// <param name="newMFA">MFA</param>
		/// <param name="newIcon">Icon</param>
		/// <param name="newCategory">Category</param>
		/// <param name="newTags">Tags (as tab separated)</param>
		/// <param name="time">Creation and modification timestamps</param>
		/// <param name="securityFunctions">Security functions</param>
		public static async Task<LoginInformation> CreateLoginInformationAsync(string newTitle, string newUrl, string newEmail, string newUsername, string newPassword, string newNotes, string newMFA, 
									byte[] newIcon, string newCategory, string newTags, DateTimeOffset time, ISecurityAsyncFunctions securityFunctions)
		{
			LoginInformation loginInformation = new LoginInformation();
			
			loginInformation.title = Encoding.UTF8.GetBytes(newTitle);
			loginInformation.url = Encoding.UTF8.GetBytes(newUrl);
			loginInformation.email = Encoding.UTF8.GetBytes(newEmail);
			loginInformation.username = Encoding.UTF8.GetBytes(newUsername);
			loginInformation.password = Encoding.UTF8.GetBytes(newPassword);

			loginInformation.notes = Encoding.UTF8.GetBytes(newNotes);
			loginInformation.mfa = Encoding.UTF8.GetBytes(newMFA);
			loginInformation.icon = newIcon;
			loginInformation.category = Encoding.UTF8.GetBytes(newCategory);
			loginInformation.tags = Encoding.UTF8.GetBytes(newTags);

			loginInformation.creationTime = time.ToUnixTimeSeconds();
			loginInformation.modificationTime = time.ToUnixTimeSeconds();

			await loginInformation.CalculateAndUpdateChecksumAsync(securityFunctions);

			return loginInformation;
		}

		#region Updates

		/// <summary>
		/// Update title, async
		/// </summary>
		/// <remarks>Will calculate checksum after update</remarks>
		/// <param name="updatedTitle">Updated title</param>
		/// <param name="securityFunctions">Security functions</param>
		public async Task UpdateTitleAsync(string updatedTitle, ISecurityAsyncFunctions securityFunctions)
		{
			this.title = Encoding.UTF8.GetBytes(updatedTitle);

			this.UpdateModificationTime();

			await this.CalculateAndUpdateChecksumAsync(securityFunctions);
		}
		
		/// <summary>
		/// Update URL, async
		/// </summary>
		/// <remarks>Will calculate checksum after update</remarks>
		/// <param name="updatedUrl">Updated URL</param>
		/// <param name="securityFunctions">Security functions</param>
		public async Task UpdateURLAsync(string updatedUrl, ISecurityAsyncFunctions securityFunctions)
		{
			this.url = Encoding.UTF8.GetBytes(updatedUrl);

			this.UpdateModificationTime();

			await this.CalculateAndUpdateChecksumAsync(securityFunctions);
		}

		/// <summary>
		/// Update email, async
		/// </summary>
		/// <remarks>Will calculate checksum after update</remarks>
		/// <param name="updatedEmail">Updated email</param>
		/// <param name="securityFunctions">Security functions</param>
		public async Task UpdateEmailAsync(string updatedEmail, ISecurityAsyncFunctions securityFunctions)
		{
			this.email = Encoding.UTF8.GetBytes(updatedEmail);

			this.UpdateModificationTime();

			await this.CalculateAndUpdateChecksumAsync(securityFunctions);
		}

		/// <summary>
		/// Update username, async
		/// </summary>
		/// <remarks>Will calculate checksum after update</remarks>
		/// <param name="updatedUsername">Updated username</param>
		/// <param name="securityFunctions">Security functions</param>
		public async Task UpdateUsernameAsync(string updatedUsername, ISecurityAsyncFunctions securityFunctions)
		{
			this.username = Encoding.UTF8.GetBytes(updatedUsername);

			this.UpdateModificationTime();

			await this.CalculateAndUpdateChecksumAsync(securityFunctions);
		}

		/// <summary>
		/// Update password, async
		/// </summary>
		/// <remarks>Will calculate checksum after update</remarks>
		/// <param name="updatedPassword">Updated password</param>
		/// <param name="securityFunctions">Security functions</param>
		public async Task UpdatePasswordAsync(string updatedPassword, ISecurityAsyncFunctions securityFunctions)
		{
			this.password = Encoding.UTF8.GetBytes(updatedPassword);

			this.UpdateModificationTime();

			await this.CalculateAndUpdateChecksumAsync(securityFunctions);
		}

		/// <summary>
		/// Update modification time from current UTC timestamp
		/// </summary>
		private void UpdateModificationTime()
		{
			this.modificationTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
		}

		/// <summary>
		/// Update notes, async
		/// </summary>
		/// <remarks>Will calculate checksum after update</remarks>
		/// <param name="updatedNotes">Updated notes</param>
		/// <param name="securityFunctions">Security functions</param>
		public async Task UpdateNotesAsync(string updatedNotes, ISecurityAsyncFunctions securityFunctions)
		{
			this.notes = Encoding.UTF8.GetBytes(updatedNotes);

			this.UpdateModificationTime();

			await this.CalculateAndUpdateChecksumAsync(securityFunctions);
		}

		/// <summary>
		/// Update MFA, async
		/// </summary>
		/// <remarks>Will calculate checksum after update</remarks>
		/// <param name="updatedMFA">Updated MFA</param>
		/// <param name="securityFunctions">Security functions</param>
		public async Task UpdateMFAAsync(string updatedMFA, ISecurityAsyncFunctions securityFunctions)
		{
			this.mfa = Encoding.UTF8.GetBytes(updatedMFA);

			this.UpdateModificationTime();

			await this.CalculateAndUpdateChecksumAsync(securityFunctions);
		}

		/// <summary>
		/// Update icon, async
		/// </summary>
		/// <remarks>Will calculate checksum after update</remarks>
		/// <param name="updatedIcon">Updated icon</param>
		/// <param name="securityFunctions">Security functions</param>
		public async Task UpdateIconAsync(byte[] updatedIcon, ISecurityAsyncFunctions securityFunctions)
		{
			this.icon = updatedIcon;

			this.UpdateModificationTime();

			await this.CalculateAndUpdateChecksumAsync(securityFunctions);
		}

		/// <summary>
		/// Update category, async
		/// </summary>
		/// <remarks>Will calculate checksum after update</remarks>
		/// <param name="updatedCategory">Updated category</param>
		/// <param name="securityFunctions">Security functions</param>
		public async Task UpdateCategoryAsync(string updatedCategory, ISecurityAsyncFunctions securityFunctions)
		{
			this.category = Encoding.UTF8.GetBytes(updatedCategory);

			this.UpdateModificationTime();

			await this.CalculateAndUpdateChecksumAsync(securityFunctions);
		}

		/// <summary>
		/// Update tags, async
		/// </summary>
		/// <remarks>Will calculate checksum after update</remarks>
		/// <param name="updatedTags">Updated tags (as tab separated)</param>
		/// <param name="securityFunctions">Security functions</param>
		public async Task UpdateTagsAsync(string updatedTags, ISecurityAsyncFunctions securityFunctions)
		{
			this.tags = Encoding.UTF8.GetBytes(updatedTags);

			this.UpdateModificationTime();

			await this.CalculateAndUpdateChecksumAsync(securityFunctions);
		}

		#endregion // Updates

		/// <summary>
		/// Check if checksum matches content, async
		/// </summary>
		/// <param name="securityFunctions">Security functions</param>
		/// <returns>True if matches; False otherwise</returns>
		public async Task<bool> CheckIfChecksumMatchesContentAsync(ISecurityAsyncFunctions securityFunctions)
		{
			return checksum == await CalculateHexChecksumAsync(securityFunctions);
		}

		private async Task<string> CalculateHexChecksumAsync(ISecurityAsyncFunctions securityFunctions)
		{
			return await ChecksumHelper.CalculateHexChecksumAsync(securityFunctions, this.title, this.url, this.email, this.username, this.password, this.notes, this.mfa, BitConverter.GetBytes(this.creationTime), BitConverter.GetBytes(this.modificationTime),
														this.icon, this.category, this.tags);
		}

		private async Task CalculateAndUpdateChecksumAsync(ISecurityAsyncFunctions securityFunctions)
		{
			this.checksum = await this.CalculateHexChecksumAsync(securityFunctions);
		}
	}

}

#endif // ASYNC_WITH_CUSTOM