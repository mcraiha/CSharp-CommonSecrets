#if ASYNC_WITH_CUSTOM

using System;
using System.Text;

using System.Threading.Tasks;

namespace CSCommonSecrets
{
	/// <summary>
	/// Payment card stores one plaintext (anyone can read) Payment card
	/// </summary>
	public sealed partial class PaymentCard
	{
		/// <summary>
		/// Default constructor for PaymentCard, async
		/// </summary>
		/// <param name="newTitle">PaymentCard title</param>
		/// <param name="newNameOnCard">PaymentCard name on card</param>
		/// <param name="newCardType">PaymentCard card type</param>
		/// <param name="newNumber">PaymentCard number</param>
		/// <param name="newSecurityCode">PaymentCard security code</param>
		/// <param name="newStartDate">PaymentCard start date</param>
		/// <param name="newExpirationDate">PaymentCard expiration date</param>
		/// <param name="newNotes">PaymentCard notes</param>
		/// <param name="securityFunctions">Security functions</param>
		public static async Task<PaymentCard> CreatePaymentCardAsync(string newTitle, string newNameOnCard, string newCardType, string newNumber, string newSecurityCode, string newStartDate, string newExpirationDate, string newNotes, ISecurityAsyncFunctions securityFunctions)
		{
			return await CreatePaymentCardAsync(newTitle, newNameOnCard, newCardType, newNumber, newSecurityCode, newStartDate, newExpirationDate, newNotes,  DateTimeOffset.UtcNow, securityFunctions);
		}

		/// <summary>
		/// Constructor with creation time override, async
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
		/// <param name="securityFunctions">Security functions</param>
		public static async Task<PaymentCard> CreatePaymentCardAsync(string newTitle, string newNameOnCard, string newCardType, string newNumber, string newSecurityCode, string newStartDate, string newExpirationDate, string newNotes, DateTimeOffset time, ISecurityAsyncFunctions securityFunctions)
		{
			PaymentCard paymentCard = new PaymentCard();

			paymentCard.creationTime = time.ToUnixTimeSeconds();
			paymentCard.modificationTime = time.ToUnixTimeSeconds();

			paymentCard.title = Encoding.UTF8.GetBytes(newTitle);
			paymentCard.nameOnCard = Encoding.UTF8.GetBytes(newNameOnCard);
			paymentCard.cardType = Encoding.UTF8.GetBytes(newCardType);
			paymentCard.number = Encoding.UTF8.GetBytes(newNumber);
			paymentCard.securityCode = Encoding.UTF8.GetBytes(newSecurityCode);
			paymentCard.startDate = Encoding.UTF8.GetBytes(newStartDate);
			paymentCard.expirationDate = Encoding.UTF8.GetBytes(newExpirationDate);
			paymentCard.notes = Encoding.UTF8.GetBytes(newNotes);

			await paymentCard.CalculateAndUpdateChecksumAsync(securityFunctions);

			return paymentCard;
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
		/// Update name on card, async
		/// </summary>
		/// <remarks>Will calculate checksum after update</remarks>
		/// <param name="updatedNameOnCard">Updated name on card</param>
		/// <param name="securityFunctions">Security functions</param>
		public async Task UpdateNameOnCardAsync(string updatedNameOnCard, ISecurityAsyncFunctions securityFunctions)
		{
			this.nameOnCard = Encoding.UTF8.GetBytes(updatedNameOnCard);

			this.UpdateModificationTime();

			await this.CalculateAndUpdateChecksumAsync(securityFunctions);
		}

		/// <summary>
		/// Update card type, async
		/// </summary>
		/// <remarks>Will calculate checksum after update</remarks>
		/// <param name="updatedCardType">Updated card type</param>
		/// <param name="securityFunctions">Security functions</param>
		public async Task UpdateCardTypeAsync(string updatedCardType, ISecurityAsyncFunctions securityFunctions)
		{
			this.cardType = Encoding.UTF8.GetBytes(updatedCardType);

			this.UpdateModificationTime();

			await this.CalculateAndUpdateChecksumAsync(securityFunctions);
		}

		/// <summary>
		/// Update number, async
		/// </summary>
		/// <remarks>Will calculate checksum after update</remarks>
		/// <param name="updatedNumber">Updated number</param>
		/// <param name="securityFunctions">Security functions</param>
		public async Task UpdateNumberAsync(string updatedNumber, ISecurityAsyncFunctions securityFunctions)
		{
			this.number = Encoding.UTF8.GetBytes(updatedNumber);

			this.UpdateModificationTime();

			await this.CalculateAndUpdateChecksumAsync(securityFunctions);
		}

		/// <summary>
		/// Update security code, async
		/// </summary>
		/// <remarks>Will calculate checksum after update</remarks>
		/// <param name="updatedSecurityCode">Updated security code</param>
		/// <param name="securityFunctions">Security functions</param>
		public async Task UpdateSecurityCodeAsync(string updatedSecurityCode, ISecurityAsyncFunctions securityFunctions)
		{
			this.securityCode = Encoding.UTF8.GetBytes(updatedSecurityCode);

			this.UpdateModificationTime();

			await this.CalculateAndUpdateChecksumAsync(securityFunctions);
		}

		/// <summary>
		/// Update start date, async
		/// </summary>
		/// <remarks>Will calculate checksum after update</remarks>
		/// <param name="updatedStartDate">Updated start date</param>
		/// <param name="securityFunctions">Security functions</param>
		public async Task UpdateStartDateAsync(string updatedStartDate, ISecurityAsyncFunctions securityFunctions)
		{
			this.startDate = Encoding.UTF8.GetBytes(updatedStartDate);

			this.UpdateModificationTime();

			await this.CalculateAndUpdateChecksumAsync(securityFunctions);
		}

		/// <summary>
		/// Update expiration date, async
		/// </summary>
		/// <remarks>Will calculate checksum after update</remarks>
		/// <param name="updatedExpirationDate">Updated expiration date</param>
		/// <param name="securityFunctions">Security functions</param>
		public async Task UpdateExpirationDateAsync(string updatedExpirationDate, ISecurityAsyncFunctions securityFunctions)
		{
			this.expirationDate = Encoding.UTF8.GetBytes(updatedExpirationDate);

			this.UpdateModificationTime();

			await this.CalculateAndUpdateChecksumAsync(securityFunctions);
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
		/// Update modification time from current UTC timestamp
		/// </summary>
		private void UpdateModificationTime()
		{
			this.modificationTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
		}

		#endregion // Updates

		/// <summary>
		/// Check if checksum matches content
		/// </summary>
		/// <param name="securityFunctions">Security functions</param>
		/// <returns>True if matches; False otherwise</returns>
		public async Task<bool> CheckIfChecksumMatchesContentAsync(ISecurityAsyncFunctions securityFunctions)
		{
			return checksum == await CalculateHexChecksumAsync(securityFunctions);
		}

		private async Task<string> CalculateHexChecksumAsync(ISecurityAsyncFunctions securityFunctions)
		{
			return await ChecksumHelper.CalculateHexChecksumAsync(securityFunctions, this.title, this.nameOnCard, this.cardType, this.number, this.securityCode, this.startDate, this.expirationDate, this.notes, BitConverter.GetBytes(this.creationTime),
														BitConverter.GetBytes(this.modificationTime));
		}

		private async Task CalculateAndUpdateChecksumAsync(ISecurityAsyncFunctions securityFunctions)
		{
			this.checksum = await this.CalculateHexChecksumAsync(securityFunctions);
		}
	}
}

#endif // ASYNC_WITH_CUSTOM