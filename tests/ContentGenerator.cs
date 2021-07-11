using CSCommonSecrets;
using System;
using System.IO;
using System.Text;

namespace Tests
{
	// Simple class for generating content
	public static class ContentGenerator
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

		public static LoginInformation GenerateRandomLoginInformation()
		{
			return new LoginInformation(Path.GetRandomFileName(), GenerateWebsiteAddress(), GenerateEmailAddress(), Path.GetRandomFileName(), Path.GetRandomFileName());
		}

		public static Note GenerateRandomNote()
		{
			int noteTitleLength = 0;
			int noteTextLength = 0;
			lock (rngLock)
			{
				noteTitleLength = rng.Next(6, 20);
				noteTextLength = rng.Next(3, 4000);
			}

			return new Note(GenerateAsciiCompatibleString(noteTitleLength), GenerateAsciiCompatibleString(noteTextLength));
		}

		public static FileEntry GenerateRandomFileEntry()
		{
			int contentLength = 0;
			lock (rngLock)
			{
				contentLength = rng.Next(20, 1022);
			}
			byte[] content = new byte[contentLength];
			rng.NextBytes(content);
			return new FileEntry(Path.GetRandomFileName(), content);
		}

		public static Contact GenerateRandomContact()
		{
			Contact returnValue = null;
			lock (rngLock)
			{
				string firstName = GenerateAsciiCompatibleString(rng.Next(4, 20));
				string lastName = GenerateAsciiCompatibleString(rng.Next(4, 20));
				string middleName = GenerateAsciiCompatibleString(rng.Next(4, 20));
				string namePrefix = GenerateAsciiCompatibleString(rng.Next(0, 3));
				string nameSuffix = GenerateAsciiCompatibleString(rng.Next(0, 3));
				string nickname = GenerateAsciiCompatibleString(rng.Next(3, 15));
				string company = GenerateAsciiCompatibleString(rng.Next(3, 15));
				string jobTitle = GenerateAsciiCompatibleString(rng.Next(3, 15));
				string department = GenerateAsciiCompatibleString(rng.Next(3, 15));
				string[] emails = { GenerateEmailAddress(), GenerateEmailAddress() };
				string[] emailDescriptions = { GenerateAsciiCompatibleString(rng.Next(3, 15)),  GenerateAsciiCompatibleString(rng.Next(3, 15)) };
				string[] phoneNumbers = { GenerateAsciiCompatibleString(rng.Next(6, 15)),  GenerateAsciiCompatibleString(rng.Next(6, 15)) };
				string[] phoneNumberDescriptions = { GenerateAsciiCompatibleString(rng.Next(3, 15)),  GenerateAsciiCompatibleString(rng.Next(3, 15)) };
				string country = GenerateAsciiCompatibleString(rng.Next(4, 20));
				string streetAddress = GenerateAsciiCompatibleString(rng.Next(4, 20));
				string streetAddressAdditional = GenerateAsciiCompatibleString(rng.Next(0, 20));
				string postalCode = GenerateAsciiCompatibleString(rng.Next(5, 6));
				string city = GenerateAsciiCompatibleString(rng.Next(4, 20));
				string poBox = GenerateAsciiCompatibleString(rng.Next(4, 20));
				string birthday = GenerateAsciiCompatibleString(rng.Next(8, 9));
				string relationship = GenerateAsciiCompatibleString(rng.Next(4, 20));;
				string notes = GenerateAsciiCompatibleString(rng.Next(0, 200));;
				string[] websites = { GenerateWebsiteAddress(), GenerateWebsiteAddress() };

				returnValue = new Contact(firstName, lastName, middleName, namePrefix, nameSuffix, nickname, company, jobTitle, department, 
										emails, emailDescriptions, phoneNumbers, phoneNumberDescriptions, 
										country, streetAddress, streetAddressAdditional, postalCode, city, poBox, birthday,
										websites, relationship, notes);
			}

			return returnValue;        
		}

		public static PaymentCard GenerateRandomPaymentCard()
		{
			PaymentCard returnValue = null;
			lock (rngLock)
			{
				returnValue = new PaymentCard(GenerateAsciiCompatibleString(rng.Next(4, 20)), GenerateAsciiCompatibleString(rng.Next(4, 20)), 
												GenerateAsciiCompatibleString(rng.Next(4, 8)), GenerateAsciiCompatibleNumberString(16), GenerateAsciiCompatibleNumberString(3),
												GenerateAsciiCompatibleMonthSlashYear(), GenerateAsciiCompatibleMonthSlashYear(), GenerateAsciiCompatibleString(rng.Next(0, 200)));
			}

			return returnValue;     
		}
	}
}