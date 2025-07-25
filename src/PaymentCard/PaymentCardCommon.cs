using System;
using System.Text;

namespace CSCommonSecrets;

/// <summary>
/// Payment card stores one plaintext (anyone can read) Payment card
/// </summary>
public sealed partial class PaymentCard
{
	/// <summary>
	/// Title of payment card as bytes, in normal case you want to use GetTitle() and UpdateTitle()
	/// </summary>
	public byte[] title { get; set; } = new byte[0];

	/// <summary>
	/// Key for storing title data to AUDALF
	/// </summary>
	public static readonly string titleKey = nameof(title);

	/// <summary>
	/// Name on card of payment card as bytes, in normal case you want to use GetNameOnCard() and UpdateNameOnCard()
	/// </summary>
	public byte[] nameOnCard { get; set; } = new byte[0];

	/// <summary>
	/// Key for storing Name on card data to AUDALF
	/// </summary>
	public static readonly string nameOnCardKey = nameof(nameOnCard);

	/// <summary>
	/// Card type of payment card as bytes, in normal case you want to use GetCardType() and UpdateCardType()
	/// </summary>
	public byte[] cardType { get; set; } = new byte[0];

	/// <summary>
	/// Key for card type on card data to AUDALF
	/// </summary>
	public static readonly string cardTypeKey = nameof(cardType);

	/// <summary>
	/// Number of payment card as bytes, in normal case you want to use GetNumber() and UpdateNumber()
	/// </summary>
	public byte[] number { get; set; } = new byte[0];

	/// <summary>
	/// Key for storing number data to AUDALF
	/// </summary>
	public static readonly string numberKey = nameof(number);

	/// <summary>
	/// Security code of payment card as bytes, in normal case you want to use GetSecurityCode() and UpdateSecurityCode()
	/// </summary>
	public byte[] securityCode { get; set; } = new byte[0];

	/// <summary>
	/// Key for storing security code data to AUDALF
	/// </summary>
	public static readonly string securityCodeKey = nameof(securityCode);

	/// <summary>
	/// Start date of payment card as bytes, in normal case you want to use GetStartDate() and UpdateStartDate()
	/// </summary>
	public byte[] startDate { get; set; } = new byte[0];

	/// <summary>
	/// Key for start date code data to AUDALF
	/// </summary>
	public static readonly string startDateKey = nameof(startDate);

	/// <summary>
	/// Expiration date of payment card as bytes, in normal case you want to use GetExpirationDate() and UpdateExpirationDate()
	/// </summary>
	public byte[] expirationDate { get; set; } = new byte[0];

	/// <summary>
	/// Key for expiration date code data to AUDALF
	/// </summary>
	public static readonly string expirationDateKey = nameof(expirationDate);

	/// <summary>
	/// Notes of payment card as bytes, in normal case you want to use GetNotes() and UpdateNotes()
	/// </summary>
	public byte[] notes { get; set; } = new byte[0];

	/// <summary>
	/// Key for storing notes data to AUDALF
	/// </summary>
	public static readonly string notesKey = nameof(notes);

	/// <summary>
	/// Creation time of payment card, in Unix seconds since epoch
	/// </summary>
	public long creationTime { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

	/// <summary>
	/// Key for storing payment card creation time to AUDALF
	/// </summary>
	public static readonly string creationTimeKey = nameof(creationTime);

	/// <summary>
	/// Last modification time of payment card, in Unix seconds since epoch
	/// </summary>
	public long modificationTime { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

	/// <summary>
	/// Key for storing payment card last modification time to AUDALF
	/// </summary>
	public static readonly string modificationTimeKey = nameof(modificationTime);

	/// <summary>
	/// Calculated checksum of payment card
	/// </summary>
	public string checksum { get; set; } = string.Empty;

	/// <summary>
	/// For deserialization purposes
	/// </summary>
	public PaymentCard()
	{
		
	}

	/// <summary>
	/// Deep copy existing PaymentCard to new PaymentCard
	/// </summary>
	/// <param name="copyThis">PaymentCard to copy</param>
	public PaymentCard(PaymentCard copyThis)
	{
		this.title = new byte[copyThis.title.Length];
		Buffer.BlockCopy(copyThis.title, 0, this.title, 0, copyThis.title.Length);

		this.nameOnCard = new byte[copyThis.nameOnCard.Length];
		Buffer.BlockCopy(copyThis.nameOnCard, 0, this.nameOnCard, 0, copyThis.nameOnCard.Length);

		this.cardType = new byte[copyThis.cardType.Length];
		Buffer.BlockCopy(copyThis.cardType, 0, this.cardType, 0, copyThis.cardType.Length);

		this.number = new byte[copyThis.number.Length];
		Buffer.BlockCopy(copyThis.number, 0, this.number, 0, copyThis.number.Length);

		this.securityCode = new byte[copyThis.securityCode.Length];
		Buffer.BlockCopy(copyThis.securityCode, 0, this.securityCode, 0, copyThis.securityCode.Length);

		this.startDate = new byte[copyThis.startDate.Length];
		Buffer.BlockCopy(copyThis.startDate, 0, this.startDate, 0, copyThis.startDate.Length);

		this.expirationDate = new byte[copyThis.expirationDate.Length];
		Buffer.BlockCopy(copyThis.expirationDate, 0, this.expirationDate, 0, copyThis.expirationDate.Length);

		this.notes = new byte[copyThis.notes.Length];
		Buffer.BlockCopy(copyThis.notes, 0, this.notes, 0, copyThis.notes.Length);

		this.creationTime = copyThis.creationTime;
		this.modificationTime = copyThis.modificationTime;

		this.checksum = copyThis.checksum;
	}

	/// <summary>
	/// Create shallow copy, mostly for testing purposes
	/// </summary>
	/// <returns>Shallow copy of PaymentCard</returns>
	public PaymentCard ShallowCopy()
	{
		return (PaymentCard) this.MemberwiseClone();
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
	/// Get name on card
	/// </summary>
	/// <returns>Name on card as string</returns>
	public string GetNameOnCard()
	{
		return System.Text.Encoding.UTF8.GetString(this.nameOnCard);
	}

	/// <summary>
	/// Get card type
	/// </summary>
	/// <returns>Card type as string</returns>
	public string GetCardType()
	{
		return System.Text.Encoding.UTF8.GetString(this.cardType);
	}

	/// <summary>
	/// Get number
	/// </summary>
	/// <returns>Number as string</returns>
	public string GetNumber()
	{
		return System.Text.Encoding.UTF8.GetString(this.number);
	}

	/// <summary>
	/// Get security code
	/// </summary>
	/// <returns>Security code as string</returns>
	public string GetSecurityCode()
	{
		return System.Text.Encoding.UTF8.GetString(this.securityCode);
	}

	/// <summary>
	/// Get start date
	/// </summary>
	/// <returns>Start date as string</returns>
	public string GetStartDate()
	{
		return System.Text.Encoding.UTF8.GetString(this.startDate);
	}

	/// <summary>
	/// Get expiration date
	/// </summary>
	/// <returns>Expiration date as string</returns>
	public string GetExpirationDate()
	{
		return System.Text.Encoding.UTF8.GetString(this.expirationDate);
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
