#if ASYNC_WITH_CUSTOM

using CSCommonSecrets;
using System;
using System.IO;
using System.Text;

using System.Threading.Tasks;

namespace Tests
{
	// Simple class for generating content
	public static class ContentGeneratorAsync
	{
		private static readonly Random rng = new Random(Seed: 1337);
		private static readonly object rngLock = new object();

		private static readonly int asciiPrintableCharsStart = 32;
		private static readonly int asciiPrintableCharsEnd = 126;

		private static readonly int asciiPrintableNumberCharsStart = 48;
		private static readonly int asciiPrintableNumberCharsEnd = 57;

		private static string GenerateAsciiCompatibleString(int wantedLength)
		{
			char[] charArray = new char[wantedLength];
			for (int i = 0; i < wantedLength; i++)
			{
				lock (rngLock)
				{
					charArray[i] = (char)rng.Next(asciiPrintableCharsStart, asciiPrintableCharsEnd + 1);
				}
			}

			return new string(charArray);
		}

		private static string GenerateAsciiCompatibleNumberString(int wantedLength)
		{
			char[] charArray = new char[wantedLength];
			for (int i = 0; i < wantedLength; i++)
			{
				lock (rngLock)
				{
					charArray[i] = (char)rng.Next(asciiPrintableNumberCharsStart, asciiPrintableNumberCharsEnd + 1);
				}
			}

			return new string(charArray);
		}

		private static string GenerateEmailAddress()
		{
			return $"{Path.GetRandomFileName()}@{Path.GetRandomFileName()}";
		}

		private static string GenerateWebsiteAddress()
		{
			return $"https://{Path.GetRandomFileName()}";
		}

		private static string GenerateAsciiCompatibleMonthSlashYear()
		{
			int month = 0;
			int year = 0;
			lock (rngLock)
			{
				month = rng.Next(1, 13);
				year = rng.Next(0, 100);
			}
			return $"{month.ToString("D2")}/{year.ToString("D2")}";
		}

		public static async Task<LoginInformation> GenerateRandomLoginInformationAsync(ISecurityAsyncFunctions securityAsyncFunctions)
		{
			return await LoginInformation.CreateLoginInformation(Path.GetRandomFileName(), GenerateWebsiteAddress(), GenerateEmailAddress(), Path.GetRandomFileName(), Path.GetRandomFileName(), securityAsyncFunctions);
		}

		public static async Task<Note> GenerateRandomNote(ISecurityAsyncFunctions securityAsyncFunctions)
		{
			int noteTitleLength = 0;
			int noteTextLength = 0;
			lock (rngLock)
			{
				noteTitleLength = rng.Next(6, 20);
				noteTextLength = rng.Next(3, 4000);
			}

			return await Note.CreateNoteAsync(GenerateAsciiCompatibleString(noteTitleLength), GenerateAsciiCompatibleString(noteTextLength), securityAsyncFunctions);
		}

		public static async Task<FileEntry> GenerateRandomFileEntryAsync(ISecurityAsyncFunctions securityAsyncFunctions)
		{
			int contentLength = 0;
			lock (rngLock)
			{
				contentLength = rng.Next(20, 1022);
			}
			byte[] content = new byte[contentLength];
			rng.NextBytes(content);
			return await FileEntry.CreateFileEntryAsync(Path.GetRandomFileName(), content, securityAsyncFunctions);
		}

		public static async Task<Contact> GenerateRandomContactAsync(ISecurityAsyncFunctions securityAsyncFunctions)
		{
			// Needed because "Cannot await in the body of a lock statement"
			string firstName;
			string lastName;
			string middleName;
			string namePrefix;
			string nameSuffix;
			string nickname;
			string company;
			string jobTitle;
			string department;
			string[] emails;
			string[] emailDescriptions;
			string[] phoneNumbers;
			string[] phoneNumberDescriptions;
			string country;
			string streetAddress;
			string streetAddressAdditional;
			string postalCode;
			string city;
			string poBox;
			string birthday;
			string relationship;
			string notes;
			string[] websites;

			lock (rngLock)
			{
				firstName = GenerateAsciiCompatibleString(rng.Next(4, 20));
				lastName = GenerateAsciiCompatibleString(rng.Next(4, 20));
				middleName = GenerateAsciiCompatibleString(rng.Next(4, 20));
				namePrefix = GenerateAsciiCompatibleString(rng.Next(0, 3));
				nameSuffix = GenerateAsciiCompatibleString(rng.Next(0, 3));
				nickname = GenerateAsciiCompatibleString(rng.Next(3, 15));
				company = GenerateAsciiCompatibleString(rng.Next(3, 15));
				jobTitle = GenerateAsciiCompatibleString(rng.Next(3, 15));
				department = GenerateAsciiCompatibleString(rng.Next(3, 15));
				emails = new string[] { GenerateEmailAddress(), GenerateEmailAddress() };
				emailDescriptions = new string[] { GenerateAsciiCompatibleString(rng.Next(3, 15)), GenerateAsciiCompatibleString(rng.Next(3, 15)) };
				phoneNumbers = new string[] { GenerateAsciiCompatibleString(rng.Next(6, 15)), GenerateAsciiCompatibleString(rng.Next(6, 15)) };
				phoneNumberDescriptions = new string[] { GenerateAsciiCompatibleString(rng.Next(3, 15)), GenerateAsciiCompatibleString(rng.Next(3, 15)) };
				country = GenerateAsciiCompatibleString(rng.Next(4, 20));
				streetAddress = GenerateAsciiCompatibleString(rng.Next(4, 20));
				streetAddressAdditional = GenerateAsciiCompatibleString(rng.Next(0, 20));
				postalCode = GenerateAsciiCompatibleString(rng.Next(5, 6));
				city = GenerateAsciiCompatibleString(rng.Next(4, 20));
				poBox = GenerateAsciiCompatibleString(rng.Next(4, 20));
				birthday = GenerateAsciiCompatibleString(rng.Next(8, 9));
				relationship = GenerateAsciiCompatibleString(rng.Next(4, 20));
				notes = GenerateAsciiCompatibleString(rng.Next(0, 200));
				websites = new string[] { GenerateWebsiteAddress(), GenerateWebsiteAddress() };	
			}

			return await Contact.CreateContactAsync(firstName, lastName, middleName, namePrefix, nameSuffix, nickname, company, jobTitle, department, 
										emails, emailDescriptions, phoneNumbers, phoneNumberDescriptions, 
										country, streetAddress, streetAddressAdditional, postalCode, city, poBox, birthday,
										websites, relationship, notes, securityAsyncFunctions);    
		}

		/*public static async Task<PaymentCard> GenerateRandomPaymentCardAsync(ISecurityAsyncFunctions securityAsyncFunctions)
		{
			PaymentCard returnValue = null;
			lock (rngLock)
			{
				returnValue = await PaymentCard.CreatePaymentCardAsync(GenerateAsciiCompatibleString(rng.Next(4, 20)), GenerateAsciiCompatibleString(rng.Next(4, 20)), 
												GenerateAsciiCompatibleString(rng.Next(4, 8)), GenerateAsciiCompatibleNumberString(16), GenerateAsciiCompatibleNumberString(3),
												GenerateAsciiCompatibleMonthSlashYear(), GenerateAsciiCompatibleMonthSlashYear(), GenerateAsciiCompatibleString(rng.Next(0, 200)), securityAsyncFunctions);
			}

			return returnValue;     
		}*/
	}
}

#endif // ASYNC_WITH_CUSTOM