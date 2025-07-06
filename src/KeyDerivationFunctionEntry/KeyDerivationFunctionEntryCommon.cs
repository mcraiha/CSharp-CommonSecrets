
namespace CSCommonSecrets;

/// <summary>
/// Key derivation function algorithms
/// </summary>
public enum KDFAlgorithm
{
	/// <summary>
	/// Password-Based Key Derivation Function 2
	/// </summary>
	PBKDF2 = 0,
}

/// <summary>
/// Key derivation pseudorandom function, should match one in Microsoft.AspNetCore.Cryptography.KeyDerivation
/// </summary>
public enum KeyDerivationPseudoRandomFunction
{
	/// <summary>
	/// HMAC SHA-256
	/// </summary>
	HMACSHA256 = 1,

	/// <summary>
	/// HMAC SHA-512
	/// </summary>
	HMACSHA512 = 2,
}

/// <summary>
/// Class that stores all needed info generating derived passwords from plain text passwords
/// </summary>
public sealed partial class KeyDerivationFunctionEntry
{
	/// <summary>
	/// Minimun salt length in bytes
	/// </summary>
	public static readonly int saltMinLengthInBytes = 16;

	/// <summary>
	/// Minimum amount of iterations
	/// </summary>
	public static readonly int iterationsMin = 4000;

	/// <summary>
	/// Suggested amount of iterations
	/// </summary>
	public static readonly int suggestedMinIterationsCount = 100_000;

	/// <summary>
	/// Algorithm can only be "PBKDF2"
	/// </summary>
	public string algorithm { get; set; }

	/// <summary>
	/// Pseudo-random function can be either "HMAC-SHA256" or "HMAC-SHA512". It is casted to enum.
	/// </summary>
	public string pseudorandomFunction { get; set; }

	/// <summary>
	/// Salt bytes
	/// </summary>
	public byte[] salt { get; set; }

	/// <summary>
	/// How many iterations should be done
	/// </summary>
	public int iterations { get; set; }

	/// <summary>
	/// How many bytes should be returned
	/// </summary>
	public int derivedKeyLengthInBytes { get; set; }

	/// <summary>
	/// Key identifier, e.g. "primary" as UTF-8 byte array
	/// </summary>
	public byte[] keyIdentifier { get; set; }

	/// <summary>
	/// Calculated checksum of Key Derivation Function Entry
	/// </summary>
	public string checksum { get; set; } = string.Empty;

	/// <summary>
	/// For (de)serialization
	/// </summary>
	public KeyDerivationFunctionEntry()
	{

	}

	
	/// <summary>
	/// Get key identifier
	/// </summary>
	/// <returns>Key identifier as string</returns>
	public string GetKeyIdentifier()
	{
		return System.Text.Encoding.UTF8.GetString(this.keyIdentifier);
	}

	#region Checksum

	/// <summary>
	/// Get checksum as hex
	/// </summary>
	/// <returns>Hex string</returns>
	public string GetChecksumAsHex()
	{
		return this.checksum;
	}

	#endregion // Checksum
}