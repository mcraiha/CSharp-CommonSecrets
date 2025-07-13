using System;
using System.Text;
using System.Collections.Generic;
using CSharp_AUDALF;

using System.Diagnostics.CodeAnalysis;

namespace CSCommonSecrets;

/// <summary>
/// Payment card secret stores stores one encrypted payment card
/// </summary>
public sealed partial class PaymentCardSecret
{
	/// <summary>
	/// Key identifier bytes (this is plaintext information), in normal case it is better to use GetKeyIdentifier()
	/// </summary>
	public required byte[] keyIdentifier { get; set; }

	/// <summary>
	/// AUDALF data as byte array (this is secret/encrypted information)
	/// </summary>
	public required byte[] audalfData { get; set; }

	/// <summary>
	/// Symmetric Key Algorithm for this PaymentCardSecret (this is plaintext information)
	/// </summary>
	public SymmetricKeyAlgorithm algorithm { get; set; }

	/// <summary>
	/// Checksum of the data (this is plaintext information)
	/// </summary>
	public string checksum { get; set; } = string.Empty;

	/// <summary>
	/// For deserialization
	/// </summary>
	public PaymentCardSecret()
	{

	}

	/// <summary>
	/// Deep copy existing PaymentCardSecret
	/// </summary>
	/// <param name="copyThis">Deep copy this</param>
	[SetsRequiredMembers]
	public PaymentCardSecret(PaymentCardSecret copyThis)
	{
		this.keyIdentifier = new byte[copyThis.keyIdentifier.Length];
		Buffer.BlockCopy(copyThis.keyIdentifier, 0, this.keyIdentifier, 0, copyThis.keyIdentifier.Length);

		this.audalfData = new byte[copyThis.audalfData.Length];
		Buffer.BlockCopy(copyThis.audalfData, 0, this.audalfData, 0, copyThis.audalfData.Length);

		this.algorithm = new SymmetricKeyAlgorithm(copyThis.algorithm);
		this.checksum = copyThis.checksum;
	}

	private static readonly SerializationSettings serializationSettings = new SerializationSettings() { dateTimeFormat = DateTimeFormat.UnixInSeconds };

	private static readonly DeserializationSettings deserializationSettings = new DeserializationSettings()
	{
		wantedDateTimeType = typeof(DateTimeOffset)
	};

	/// <summary>
	/// Get key identifer.
	/// </summary>
	/// <returns>Key identifier</returns>
	public string GetKeyIdentifier()
	{
		return System.Text.Encoding.UTF8.GetString(this.keyIdentifier);
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
