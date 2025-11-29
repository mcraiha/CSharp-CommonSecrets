using System;

using System.Diagnostics.CodeAnalysis;

namespace CSCommonSecrets;

/// <summary>
/// What kind of history event happened
/// </summary>
public enum HistoryEventType
{
	/// <summary>
    /// Entry created
    /// </summary>
    Create = 0,
	/// <summary>
    /// Entry read
    /// </summary>
	Read,
	/// <summary>
    /// Entry updated
    /// </summary>
	Update,
	/// <summary>
    /// Etnry deleted
    /// </summary>
	Delete
}

/// <summary>
/// History stores one plaintext (anyone can read) Common Secrets modification entry
/// </summary>
/// <remarks>History entries are not mandatory to functionality of the Common Secrets format</remarks>
public sealed partial class History
{
	/// <summary>
	/// What kind of history event this is
	/// </summary>
	public required string eventType { get; set; }

	/// <summary>
	/// Key for storing history event to AUDALF
	/// </summary>
	public static readonly string eventTypeKey = nameof(eventType);

	/// <summary>
	/// Description text as byte array
	/// </summary>
	public byte[] descriptionText { get; set; } = new byte[0];

	/// <summary>
	/// Description text key
	/// </summary>
	public static readonly string descriptionTextKey = nameof(descriptionText);

	/// <summary>
	/// Occurence time of modification, in Unix seconds since epoch
	/// </summary>
	public long occurenceTime { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

	/// <summary>
	/// Key for storing occurence time
	/// </summary>
	public static readonly string occurenceTimeKey = nameof(occurenceTime);

	/// <summary>
	/// Calculated checksum of history event
	/// </summary>
	public string checksum { get; set; } = string.Empty;

	/// <summary>
	/// For deserialization purposes
	/// </summary>
	public History()
	{

	}

	/// <summary>
	/// Deep copy existing History entry to new History entry
	/// </summary>
	/// <param name="copyThis">History entry to copy</param>
	[SetsRequiredMembers]
	public History(History copyThis)
	{
		this.eventType = new string(copyThis.eventType);

		this.descriptionText = new byte[copyThis.descriptionText.Length];
		Buffer.BlockCopy(copyThis.descriptionText, 0, this.descriptionText, 0, copyThis.descriptionText.Length);

		this.occurenceTime = copyThis.occurenceTime;

		this.checksum = copyThis.checksum;
	}

	/// <summary>
	/// Create shallow copy, mostly for testing purposes
	/// </summary>
	/// <returns>Shallow copy of History</returns>
	public History ShallowCopy()
	{
		return (History)this.MemberwiseClone();
	}

	/// <summary>
	/// Get event type
	/// </summary>
	/// <returns>Event type</returns>
	public HistoryEventType GetEventType()
	{
		if (Enum.TryParse(this.eventType, out HistoryEventType parsedEventType))
		{
			return parsedEventType;
		}

		throw new Exception("Cannot parse history event type");
	}

	/// <summary>
	/// Get description text
	/// </summary>
	/// <returns>Description text as string</returns>
	public string GetDescription()
	{
		return System.Text.Encoding.UTF8.GetString(this.descriptionText);
	}


	/// <summary>
	/// Get occurence time
	/// </summary>
	/// <returns>Occurence time as DateTimeOffset</returns>
	public DateTimeOffset GetOccurenceTime()
	{
		return DateTimeOffset.FromUnixTimeSeconds(this.occurenceTime);
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
