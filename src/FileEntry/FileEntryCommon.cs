using System;

namespace CSCommonSecrets;

/// <summary>
/// FileEntry stores one plaintext (anyone can read) file
/// </summary>
public sealed partial class FileEntry
{
	/// <summary>
	/// Filename as byte array so that special characters won't cause issues. For normal use case use GetFilename()
	/// </summary>
	public byte[] filename { get; set; } = new byte[0];

	/// <summary>
	/// Key for storing filename data to AUDALF
	/// </summary>
	public static readonly string filenameKey = nameof(filename);

	/// <summary>
	/// File content as byte array
	/// </summary>
	public byte[] fileContent { get; set; } = new byte[0];

	/// <summary>
	/// Key for storing file content (actual bytes of file) to AUDALF
	/// </summary>
	public static readonly string fileContentKey = nameof(fileContent);

	/// <summary>
	/// Creation time of file entry, in Unix seconds since epoch
	/// </summary>
	public long creationTime { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

	/// <summary>
	/// Key for storing file entry creation time to AUDALF
	/// </summary>
	public static readonly string creationTimeKey = nameof(creationTime);

	/// <summary>
	/// Last modification time of file entry, in Unix seconds since epoch
	/// </summary>
	public long modificationTime { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

	/// <summary>
	/// Key for storing file entry last modification time to AUDALF
	/// </summary>
	public static readonly string modificationTimeKey = nameof(modificationTime);

	/// <summary>
	/// Calculated checksum of File entry
	/// </summary>
	public string checksum { get; set; } = string.Empty;

	/// <summary>
	/// For deserialization purposes
	/// </summary>
	public FileEntry()
	{
		
	}

	/// <summary>
	/// Deep copy existing FileEntry to new FileEntry
	/// </summary>
	/// <param name="copyThis">FileEntry to copy</param>
	public FileEntry(FileEntry copyThis)
	{
		this.filename = new byte[copyThis.filename.Length];
		Buffer.BlockCopy(copyThis.filename, 0, this.filename, 0, copyThis.filename.Length);

		this.fileContent = new byte[copyThis.fileContent.Length];
		Buffer.BlockCopy(copyThis.fileContent, 0, this.fileContent, 0, copyThis.fileContent.Length);

		this.creationTime = copyThis.creationTime;
		this.modificationTime = copyThis.modificationTime;

		this.checksum = copyThis.checksum;
	}

	/// <summary>
	/// Create shallow copy, mostly for testing purposes
	/// </summary>
	/// <returns>Shallow copy of FileEntry</returns>
	public FileEntry ShallowCopy()
	{
		return (FileEntry) this.MemberwiseClone();
	}

	/// <summary>
	/// Get filename
	/// </summary>
	/// <returns>Filename as string</returns>
	public string GetFilename()
	{
		return System.Text.Encoding.UTF8.GetString(this.filename);
	}

	/// <summary>
	/// Get file content
	/// </summary>
	/// <returns>File content as byte array</returns>
	public byte[] GetFileContent()
	{
		return fileContent;
	}

	/// <summary>
	/// Get file content lenght in bytes
	/// </summary>
	/// <returns>Lenght in bytes</returns>
	public long GetFileContentLengthInBytes()
	{
		return fileContent.LongLength;
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

	/// <summary>
	/// Get checksum as hex
	/// </summary>
	/// <returns>Hex string</returns>
	public string GetChecksumAsHex()
	{
		return this.checksum;
	}
}
