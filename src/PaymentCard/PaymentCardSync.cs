#if !ASYNC_WITH_CUSTOM && !WITH_CUSTOM

using System;
using System.Text;

namespace CSCommonSecrets;

/// <summary>
/// Payment card stores one plaintext (anyone can read) Payment card
/// </summary>
public sealed partial class PaymentCard
{
	/// <summary>
	/// Default constructor for PaymentCard
	/// </summary>
	/// <param name="newTitle">PaymentCard title</param>
	/// <param name="newNameOnCard">PaymentCard name on card</param>
	/// <param name="newCardType">PaymentCard card type</param>
	/// <param name="newNumber">PaymentCard number</param>
	/// <param name="newSecurityCode">PaymentCard security code</param>
	/// <param name="newStartDate">PaymentCard start date</param>
	/// <param name="newExpirationDate">PaymentCard expiration date</param>
	/// <param name="newNotes">PaymentCard notes</param>
	public PaymentCard(string newTitle, string newNameOnCard, string newCardType, string newNumber, string newSecurityCode, string newStartDate, string newExpirationDate, string newNotes) : this (newTitle, newNameOnCard, newCardType, newNumber, newSecurityCode, newStartDate, newExpirationDate, newNotes,  DateTimeOffset.UtcNow)
	{
		
	}

	/// <summary>
	/// Constructor with creation time override
	/// </summary>
	/// <param name="newTitle">PaymentCard title</param>
	/// <param name="newNameOnCard">PaymentCard name on card</param>
	/// <param name="newCardType">PaymentCard card type</param>
	/// <param name="newNumber">PaymentCard number</param>
	/// <param name="newSecurityCode">PaymentCard security code</param>
	/// <param name="newStartDate">PaymentCard start date</param>
	/// <param name="newExpirationDate">PaymentCard expiration date</param>
	/// <param name="newNotes">PaymentCard notes</param>
	/// <param name="time">Creation time</param>
	public PaymentCard(string newTitle, string newNameOnCard, string newCardType, string newNumber, string newSecurityCode, string newStartDate, string newExpirationDate, string newNotes, DateTimeOffset time)
	{
		this.creationTime = time.ToUnixTimeSeconds();
		this.modificationTime = time.ToUnixTimeSeconds();

		this.title = Encoding.UTF8.GetBytes(newTitle);
		this.nameOnCard = Encoding.UTF8.GetBytes(newNameOnCard);
		this.cardType = Encoding.UTF8.GetBytes(newCardType);
		this.number = Encoding.UTF8.GetBytes(newNumber);
		this.securityCode = Encoding.UTF8.GetBytes(newSecurityCode);
		this.startDate = Encoding.UTF8.GetBytes(newStartDate);
		this.expirationDate = Encoding.UTF8.GetBytes(newExpirationDate);
		this.notes = Encoding.UTF8.GetBytes(newNotes);

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
	/// Update name on card
	/// </summary>
	/// <remarks>Will calculate checksum after update</remarks>
	/// <param name="updatedNameOnCard">Updated name on card</param>
	public void UpdateNameOnCard(string updatedNameOnCard)
	{
		this.nameOnCard = Encoding.UTF8.GetBytes(updatedNameOnCard);

		this.UpdateModificationTime();

		this.CalculateAndUpdateChecksum();
	}

	/// <summary>
	/// Update card type
	/// </summary>
	/// <remarks>Will calculate checksum after update</remarks>
	/// <param name="updatedCardType">Updated card type</param>
	public void UpdateCardType(string updatedCardType)
	{
		this.cardType = Encoding.UTF8.GetBytes(updatedCardType);

		this.UpdateModificationTime();

		this.CalculateAndUpdateChecksum();
	}

	/// <summary>
	/// Update number
	/// </summary>
	/// <remarks>Will calculate checksum after update</remarks>
	/// <param name="updatedNumber">Updated number</param>
	public void UpdateNumber(string updatedNumber)
	{
		this.number = Encoding.UTF8.GetBytes(updatedNumber);

		this.UpdateModificationTime();

		this.CalculateAndUpdateChecksum();
	}

	/// <summary>
	/// Update security code
	/// </summary>
	/// <remarks>Will calculate checksum after update</remarks>
	/// <param name="updatedSecurityCode">Updated security code</param>
	public void UpdateSecurityCode(string updatedSecurityCode)
	{
		this.securityCode = Encoding.UTF8.GetBytes(updatedSecurityCode);

		this.UpdateModificationTime();

		this.CalculateAndUpdateChecksum();
	}

	/// <summary>
	/// Update start date
	/// </summary>
	/// <remarks>Will calculate checksum after update</remarks>
	/// <param name="updatedStartDate">Updated start date</param>
	public void UpdateStartDate(string updatedStartDate)
	{
		this.startDate = Encoding.UTF8.GetBytes(updatedStartDate);

		this.UpdateModificationTime();

		this.CalculateAndUpdateChecksum();
	}

	/// <summary>
	/// Update expiration date
	/// </summary>
	/// <remarks>Will calculate checksum after update</remarks>
	/// <param name="updatedExpirationDate">Updated expiration date</param>
	public void UpdateExpirationDate(string updatedExpirationDate)
	{
		this.expirationDate = Encoding.UTF8.GetBytes(updatedExpirationDate);

		this.UpdateModificationTime();

		this.CalculateAndUpdateChecksum();
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
	/// Update modification time from current UTC timestamp
	/// </summary>
	private void UpdateModificationTime()
	{
		this.modificationTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
	}

	#endregion // Updates

	#region Checksum

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
		return ChecksumHelper.CalculateHexChecksum(this.title, this.nameOnCard, this.cardType, this.number, this.securityCode, this.startDate, this.expirationDate, this.notes, BitConverter.GetBytes(this.creationTime),
													BitConverter.GetBytes(this.modificationTime));
	}

	private void CalculateAndUpdateChecksum()
	{
		this.checksum = this.CalculateHexChecksum();
	}

	#endregion // Checksum
}

#endif // !ASYNC_WITH_CUSTOM && !WITH_CUSTOM