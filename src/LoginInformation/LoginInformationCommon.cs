using System;
using System.Text;

namespace CSCommonSecrets;

/// <summary>
/// LoginInformation stores one plaintext (anyone can read) login information
/// </summary>
public sealed partial class LoginInformation
{
	/// <summary>
	/// Title of login information as bytes, in normal case you want to use GetTitle() and UpdateTitle()
	/// </summary>
	public byte[] title { get; set; } = new byte[0];

	/// <summary>
	/// Key for storing title data to AUDALF
	/// </summary>
	public static readonly string titleKey = nameof(title);

	/// <summary>
	/// URL of login information as bytes, in normal case you want to use GetURL() and UpdateURL()
	/// </summary>
	public byte[] url { get; set; } = new byte[0];

	/// <summary>
	/// Key for storing URL data to AUDALF
	/// </summary>
	public static readonly string urlKey = nameof(url);

	/// <summary>
	/// Email of login information as bytes, in normal case you want to use GetEmail() and UpdateEmail()
	/// </summary>
	public byte[] email { get; set; } = new byte[0];

	/// <summary>
	/// Key for storing email data to AUDALF
	/// </summary>
	public static readonly string emailKey = nameof(email);

	/// <summary>
	/// Username of login information as bytes, in normal case you want to use GetUsername() and UpdateUsername()
	/// </summary>
	public byte[] username { get; set; } = new byte[0];

	/// <summary>
	/// Key for storing username data to AUDALF
	/// </summary>
	public static readonly string usernameKey = nameof(username);

	/// <summary>
	/// Password of login information as bytes, in normal case you want to use GetPassword() and UpdatePassword()
	/// </summary>
	public byte[] password { get; set; } = new byte[0];

	/// <summary>
	/// Key for storing password data to AUDALF
	/// </summary>
	public static readonly string passwordKey = nameof(password);

	/// <summary>
	/// Notes of login information as bytes, in normal case you want to use GetNotes() and UpdateNotes()
	/// </summary>
	public byte[] notes { get; set; } = new byte[0];

	/// <summary>
	/// Key for storing notes data to AUDALF
	/// </summary>
	public static readonly string notesKey = nameof(notes);

	/// <summary>
	/// MFA entry (e.g. TOTP URL) as bytes, in normal case you want to use GetMFA() and UpdateMFA()
	/// </summary>
	public byte[] mfa { get; set; } = new byte[0];

	/// <summary>
	/// Ket for storing MFA data to AUDALF
	/// </summary>
	public static readonly string mfaKey = nameof(mfa);

	/// <summary>
	/// Creation time of login information, in Unix seconds since epoch
	/// </summary>
	public long creationTime { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

	/// <summary>
	/// Key for storing login information creation time to AUDALF
	/// </summary>
	public static readonly string creationTimeKey = nameof(creationTime);

	/// <summary>
	/// Last modification time of login information, in Unix seconds since epoch
	/// </summary>
	public long modificationTime { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

	/// <summary>
	/// Key for storing login information last modification time to AUDALF
	/// </summary>
	public static readonly string modificationTimeKey = nameof(modificationTime);

	/// <summary>
	/// Icon of login information as bytes, in normal case you want to use GetIcon() and UpdateIcon()
	/// </summary>
	public byte[] icon { get; set; } = new byte[0];

	/// <summary>
	/// Key for storing login information icon to AUDALF
	/// </summary>
	public static readonly string iconKey = nameof(icon);

	/// <summary>
	/// Category of login information as bytes, in normal case you want to use GetCategory() and UpdateCategory()
	/// </summary>
	public byte[] category { get; set; } = new byte[0];

	/// <summary>
	/// Key for storing login information category to AUDALF
	/// </summary>
	public static readonly string categoryKey = nameof(category);

	/// <summary>
	/// Tags of login information as bytes, in normal case you want to use GetTags() and UpdateTags()
	/// </summary>
	public byte[] tags { get; set; } = new byte[0];

	/// <summary>
	/// Key for storing login information tags to AUDALF
	/// </summary>
	public static readonly string tagsKey = nameof(tags);

	/// <summary>
	/// Calculated checksum of Login information
	/// </summary>
	public string checksum { get; set; } = string.Empty;

	/// <summary>
	/// For deserialization purposes
	/// </summary>
	public LoginInformation()
	{

	}

	/// <summary>
	/// Deep copy existing LoginInformation to new LoginInformation
	/// </summary>
	/// <param name="copyThis">LoginInformation to copy</param>
	public LoginInformation(LoginInformation copyThis)
	{
		this.title = new byte[copyThis.title.Length];
		Buffer.BlockCopy(copyThis.title, 0, this.title, 0, copyThis.title.Length);

		this.url = new byte[copyThis.url.Length];
		Buffer.BlockCopy(copyThis.url, 0, this.url, 0, copyThis.url.Length);

		this.email =  new byte[copyThis.email.Length];
		Buffer.BlockCopy(copyThis.email, 0, this.email, 0, copyThis.email.Length);

		this.username = new byte[copyThis.username.Length];
		Buffer.BlockCopy(copyThis.username, 0, this.username, 0, copyThis.username.Length);

		this.password = new byte[copyThis.password.Length];
		Buffer.BlockCopy(copyThis.password, 0, this.password, 0, copyThis.password.Length);

		this.notes = new byte[copyThis.notes.Length];
		Buffer.BlockCopy(copyThis.notes, 0, this.notes, 0, copyThis.notes.Length);

		this.mfa = new byte[copyThis.mfa.Length];
		Buffer.BlockCopy(copyThis.mfa, 0, this.mfa, 0, copyThis.mfa.Length);

		this.icon = new byte[copyThis.icon.Length];
		Buffer.BlockCopy(copyThis.icon, 0, this.icon, 0, copyThis.icon.Length);

		this.category = new byte[copyThis.category.Length];
		Buffer.BlockCopy(copyThis.category, 0, this.category, 0, copyThis.category.Length);

		this.tags = new byte[copyThis.tags.Length];
		Buffer.BlockCopy(copyThis.tags, 0, this.tags, 0, copyThis.tags.Length);

		this.creationTime = copyThis.creationTime;
		this.modificationTime = copyThis.modificationTime;
		
		this.checksum = copyThis.checksum;
	}

	/// <summary>
	/// Creat shallow copy, mostly for testing purposes
	/// </summary>
	/// <returns>Shallow copy of LoginInformation</returns>
	public LoginInformation ShallowCopy()
	{
		return (LoginInformation) this.MemberwiseClone();
	}

	#region Getters

	/// <summary>
	/// Get title
	/// </summary>
	/// <returns>Title as string</returns>
	public string GetTitle()
	{
		return System.Text.Encoding.UTF8.GetString(this.title);
	}

	/// <summary>
	/// Get URL
	/// </summary>
	/// <returns>URL as string</returns>
	public string GetURL()
	{
		return System.Text.Encoding.UTF8.GetString(this.url);
	}

	/// <summary>
	/// Get email
	/// </summary>
	/// <returns>Email as string</returns>
	public string GetEmail()
	{
		return System.Text.Encoding.UTF8.GetString(this.email);
	}

	/// <summary>
	/// Get username
	/// </summary>
	/// <returns>Username as string</returns>
	public string GetUsername()
	{
		return System.Text.Encoding.UTF8.GetString(this.username);
	}

	/// <summary>
	/// Get password
	/// </summary>
	/// <returns>Password as string</returns>
	public string GetPassword()
	{
		return System.Text.Encoding.UTF8.GetString(this.password);
	}

	/// <summary>
	/// Get notes
	/// </summary>
	/// <returns>Notes as string</returns>
	public string GetNotes()
	{
		return System.Text.Encoding.UTF8.GetString(this.notes);
	}

	/// <summary>
	/// Get MFA
	/// </summary>
	/// <returns>MFA as string</returns>
	public string GetMFA()
	{
		return System.Text.Encoding.UTF8.GetString(this.mfa);
	}

	/// <summary>
	/// Get icon (small image file)
	/// </summary>
	/// <returns>Icon as byte array</returns>
	public byte[] GetIcon()
	{
		return this.icon;
	}

	/// <summary>
	/// Get category
	/// </summary>
	/// <returns>Category as string</returns>
	public string GetCategory()
	{
		return System.Text.Encoding.UTF8.GetString(this.category);
	}

	/// <summary>
	/// Get tags
	/// </summary>
	/// <returns>Tags as string (tab separated)</returns>
	public string GetTags()
	{
		return System.Text.Encoding.UTF8.GetString(this.tags);
	}

	/// <summary>
	/// Get creation time
	/// </summary>
	/// <returns>Creation time as DateTimeOffset</returns>
	public DateTimeOffset GetCreationTime()
	{
		return DateTimeOffset.FromUnixTimeSeconds(this.creationTime);
	}

	/// <summary>
	/// Get modification time
	/// </summary>
	/// <returns>Modification time as DateTimeOffset</returns>
	public DateTimeOffset GetModificationTime()
	{
		return DateTimeOffset.FromUnixTimeSeconds(this.modificationTime);
	}

	#endregion // Getters

	/// <summary>
	/// Get checksum as hex
	/// </summary>
	/// <returns>Hex string</returns>
	public string GetChecksumAsHex()
	{
		return this.checksum;
	}
}
