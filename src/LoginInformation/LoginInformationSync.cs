#if !ASYNC_WITH_CUSTOM && !WITH_CUSTOM

using System;
using System.Text;

namespace CSCommonSecrets;

/// <summary>
/// LoginInformation stores one plaintext (anyone can read) login information
/// </summary>
public sealed partial class LoginInformation
{
	/// <summary>
	/// Default small LoginInformation constructor
	/// </summary>
	/// <param name="newTitle">Title</param>
	/// <param name="newUrl">URL</param>
	/// <param name="newEmail">Email</param>
	/// <param name="newUsername">Username</param>
	/// <param name="newPassword">Password</param>
	public LoginInformation(string newTitle, string newUrl, string newEmail, string newUsername, string newPassword) : this (newTitle, newUrl, newEmail, newUsername, newPassword, DateTimeOffset.UtcNow)
	{

	}

	/// <summary>
	/// Small LoginInformation constructor
	/// </summary>
	/// <param name="newTitle">Title</param>
	/// <param name="newUrl">URL</param>
	/// <param name="newEmail">Email</param>
	/// <param name="newUsername">Username</param>
	/// <param name="newPassword">Password</param>
	/// <param name="time">Creation and modification timestamps</param>
	public LoginInformation(string newTitle, string newUrl, string newEmail, string newUsername, string newPassword, DateTimeOffset time)
	{
		this.title = Encoding.UTF8.GetBytes(newTitle);
		this.url = Encoding.UTF8.GetBytes(newUrl);
		this.email = Encoding.UTF8.GetBytes(newEmail);
		this.username = Encoding.UTF8.GetBytes(newUsername);
		this.password = Encoding.UTF8.GetBytes(newPassword);
		this.creationTime = time.ToUnixTimeSeconds();
		this.modificationTime = time.ToUnixTimeSeconds();

		this.CalculateAndUpdateChecksum();
	}

	/// <summary>
	/// Default full LoginInformation constructor
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
	public LoginInformation(string newTitle, string newUrl, string newEmail, string newUsername, string newPassword, string newNotes, string newMFA,
								byte[] newIcon, string newCategory, string newTags) : this (newTitle, newUrl, newEmail, newUsername, newPassword, newNotes, newMFA,
								newIcon, newCategory, newTags, DateTimeOffset.UtcNow)
	{

	}

	/// <summary>
	/// Full LoginInformation constructor
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
	public LoginInformation(string newTitle, string newUrl, string newEmail, string newUsername, string newPassword, string newNotes, string newMFA, 
								byte[] newIcon, string newCategory, string newTags, DateTimeOffset time)
	{
		this.title = Encoding.UTF8.GetBytes(newTitle);
		this.url = Encoding.UTF8.GetBytes(newUrl);
		this.email = Encoding.UTF8.GetBytes(newEmail);
		this.username = Encoding.UTF8.GetBytes(newUsername);
		this.password = Encoding.UTF8.GetBytes(newPassword);

		this.notes = Encoding.UTF8.GetBytes(newNotes);
		this.mfa = Encoding.UTF8.GetBytes(newMFA);
		this.icon = newIcon;
		this.category = Encoding.UTF8.GetBytes(newCategory);
		this.tags = Encoding.UTF8.GetBytes(newTags);

		this.creationTime = time.ToUnixTimeSeconds();
		this.modificationTime = time.ToUnixTimeSeconds();

		this.CalculateAndUpdateChecksum();
	}

	#region Updates

	/// <summary>
	/// Update title
	/// </summary>
	/// <remarks>Will calculate checksum after update</remarks>
	/// <param name="updatedTitle">Updated title</param>
	public void UpdateTitle(string updatedTitle)
	{
		this.title = Encoding.UTF8.GetBytes(updatedTitle);

		this.UpdateModificationTime();

		this.CalculateAndUpdateChecksum();
	}
	
	/// <summary>
	/// Update URL
	/// </summary>
	/// <remarks>Will calculate checksum after update</remarks>
	/// <param name="updatedUrl">Updated URL</param>
	public void UpdateURL(string updatedUrl)
	{
		this.url = Encoding.UTF8.GetBytes(updatedUrl);

		this.UpdateModificationTime();

		this.CalculateAndUpdateChecksum();
	}

	/// <summary>
	/// Update email
	/// </summary>
	/// <remarks>Will calculate checksum after update</remarks>
	/// <param name="updatedEmail">Updated email</param>
	public void UpdateEmail(string updatedEmail)
	{
		this.email = Encoding.UTF8.GetBytes(updatedEmail);

		this.UpdateModificationTime();

		this.CalculateAndUpdateChecksum();
	}

	/// <summary>
	/// Update username
	/// </summary>
	/// <remarks>Will calculate checksum after update</remarks>
	/// <param name="updatedUsername">Updated username</param>
	public void UpdateUsername(string updatedUsername)
	{
		this.username = Encoding.UTF8.GetBytes(updatedUsername);

		this.UpdateModificationTime();

		this.CalculateAndUpdateChecksum();
	}

	/// <summary>
	/// Update password
	/// </summary>
	/// <remarks>Will calculate checksum after update</remarks>
	/// <param name="updatedPassword">Updated password</param>
	public void UpdatePassword(string updatedPassword)
	{
		this.password = Encoding.UTF8.GetBytes(updatedPassword);

		this.UpdateModificationTime();

		this.CalculateAndUpdateChecksum();
	}

	/// <summary>
	/// Update modification time from current UTC timestamp
	/// </summary>
	private void UpdateModificationTime()
	{
		this.modificationTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
	}

	/// <summary>
	/// Update notes
	/// </summary>
	/// <remarks>Will calculate checksum after update</remarks>
	/// <param name="updatedNotes">Updated notes</param>
	public void UpdateNotes(string updatedNotes)
	{
		this.notes = Encoding.UTF8.GetBytes(updatedNotes);

		this.UpdateModificationTime();

		this.CalculateAndUpdateChecksum();
	}

	/// <summary>
	/// Update MFA
	/// </summary>
	/// <remarks>Will calculate checksum after update</remarks>
	/// <param name="updatedMFA">Updated MFA</param>
	public void UpdateMFA(string updatedMFA)
	{
		this.mfa = Encoding.UTF8.GetBytes(updatedMFA);

		this.UpdateModificationTime();

		this.CalculateAndUpdateChecksum();
	}

	/// <summary>
	/// Update icon
	/// </summary>
	/// <remarks>Will calculate checksum after update</remarks>
	/// <param name="updatedIcon">Updated icon</param>
	public void UpdateIcon(byte[] updatedIcon)
	{
		this.icon = updatedIcon;

		this.UpdateModificationTime();

		this.CalculateAndUpdateChecksum();
	}

	/// <summary>
	/// Update category
	/// </summary>
	/// <remarks>Will calculate checksum after update</remarks>
	/// <param name="updatedCategory">Updated category</param>
	public void UpdateCategory(string updatedCategory)
	{
		this.category = Encoding.UTF8.GetBytes(updatedCategory);

		this.UpdateModificationTime();

		this.CalculateAndUpdateChecksum();
	}

	/// <summary>
	/// Update tags
	/// </summary>
	/// <remarks>Will calculate checksum after update</remarks>
	/// <param name="updatedTags">Updated tags (as tab separated)</param>
	public void UpdateTags(string updatedTags)
	{
		this.tags = Encoding.UTF8.GetBytes(updatedTags);

		this.UpdateModificationTime();

		this.CalculateAndUpdateChecksum();
	}

	#endregion // Updates

	/// <summary>
	/// Check if checksum matches content
	/// </summary>
	/// <returns>True if matches; False otherwise</returns>
	public bool CheckIfChecksumMatchesContent()
	{
		return checksum == this.CalculateHexChecksum();
	}

	private string CalculateHexChecksum()
	{
		return ChecksumHelper.CalculateHexChecksum(this.title, this.url, this.email, this.username, this.password, this.notes, this.mfa, BitConverter.GetBytes(this.creationTime), BitConverter.GetBytes(this.modificationTime),
													this.icon, this.category, this.tags);
	}

	private void CalculateAndUpdateChecksum()
	{
		this.checksum = this.CalculateHexChecksum();
	}
}

#endif // !ASYNC_WITH_CUSTOM && !WITH_CUSTOM