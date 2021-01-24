using CSCommonSecrets;
using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace Tests
{
	// Simple class for checking that two things are equal
	public static class ComparisonHelper
	{
		public static bool AreKeyDerivationFunctionEntriesEqual(KeyDerivationFunctionEntry kdfe1, KeyDerivationFunctionEntry kdfe2)
		{
			return kdfe1.algorithm == kdfe2.algorithm && kdfe1.pseudorandomFunction == kdfe2.pseudorandomFunction &&
					StructuralComparisons.StructuralEqualityComparer.Equals(kdfe1.salt, kdfe2.salt) &&
					kdfe1.iterations == kdfe2.iterations &&
					kdfe1.derivedKeyLengthInBytes == kdfe2.derivedKeyLengthInBytes &&
					StructuralComparisons.StructuralEqualityComparer.Equals(kdfe1.keyIdentifier, kdfe2.keyIdentifier);
		}

		public static bool AreLoginInformationsEqual(LoginInformation login1, LoginInformation login2)
		{
			return StructuralComparisons.StructuralEqualityComparer.Equals(login1.title, login2.title) &&
					StructuralComparisons.StructuralEqualityComparer.Equals(login1.url, login2.url) &&
					StructuralComparisons.StructuralEqualityComparer.Equals(login1.username, login2.username) &&
					StructuralComparisons.StructuralEqualityComparer.Equals(login1.password, login2.password);
					// Missing code here!!!
		}

		public static bool AreLoginInformationSecretsEqual(LoginInformationSecret loginSecret1, LoginInformationSecret loginSecret2)
		{
			return StructuralComparisons.StructuralEqualityComparer.Equals(loginSecret1.audalfData, loginSecret2.audalfData) && AreSymmetricKeyAlgorithmsEqual(loginSecret1.algorithm, loginSecret2.algorithm);
		}

		public static bool AreNotesEqual(Note note1, Note note2)
		{
			return StructuralComparisons.StructuralEqualityComparer.Equals(note1.noteTitle, note2.noteTitle) && StructuralComparisons.StructuralEqualityComparer.Equals(note1.noteText, note2.noteText);
		}

		public static bool AreNotesSecretEqual(NoteSecret noteSecret1, NoteSecret noteSecret2)
		{
			return StructuralComparisons.StructuralEqualityComparer.Equals(noteSecret1.audalfData, noteSecret2.audalfData) && AreSymmetricKeyAlgorithmsEqual(noteSecret1.algorithm, noteSecret2.algorithm);
		}

		public static bool AreFileEntriesEqual(FileEntry fileEntry1, FileEntry fileEntry2)
		{
			return StructuralComparisons.StructuralEqualityComparer.Equals(fileEntry1.filename, fileEntry2.filename) && StructuralComparisons.StructuralEqualityComparer.Equals(fileEntry1.fileContent, fileEntry2.fileContent);
		}

		public static bool AreFileEntrySecretsEqual(FileEntrySecret fileEntrySecret1, FileEntrySecret fileEntrySecret2)
		{
			return StructuralComparisons.StructuralEqualityComparer.Equals(fileEntrySecret1.audalfData, fileEntrySecret2.audalfData) && AreSymmetricKeyAlgorithmsEqual(fileEntrySecret1.algorithm, fileEntrySecret2.algorithm);
		}

		public static bool AreContactsEqual(Contact contact1, Contact contact2)
		{
			return StructuralComparisons.StructuralEqualityComparer.Equals(contact1.firstName, contact2.firstName) &&
					StructuralComparisons.StructuralEqualityComparer.Equals(contact1.lastName, contact2.lastName) &&
					StructuralComparisons.StructuralEqualityComparer.Equals(contact1.middleName, contact2.middleName) &&
					StructuralComparisons.StructuralEqualityComparer.Equals(contact1.namePrefix, contact2.namePrefix) &&
					StructuralComparisons.StructuralEqualityComparer.Equals(contact1.nameSuffix, contact2.nameSuffix) &&
					StructuralComparisons.StructuralEqualityComparer.Equals(contact1.nickname, contact2.nickname) &&
					StructuralComparisons.StructuralEqualityComparer.Equals(contact1.company, contact2.company) &&
					StructuralComparisons.StructuralEqualityComparer.Equals(contact1.jobTitle, contact2.jobTitle) &&
					StructuralComparisons.StructuralEqualityComparer.Equals(contact1.department, contact2.department) &&
					StructuralComparisons.StructuralEqualityComparer.Equals(contact1.emails, contact2.emails) &&
					StructuralComparisons.StructuralEqualityComparer.Equals(contact1.emailDescriptions, contact2.emailDescriptions) &&
					StructuralComparisons.StructuralEqualityComparer.Equals(contact1.phoneNumbers, contact2.phoneNumbers) &&
					StructuralComparisons.StructuralEqualityComparer.Equals(contact1.phoneNumberDescriptions, contact2.phoneNumberDescriptions) &&
					StructuralComparisons.StructuralEqualityComparer.Equals(contact1.country, contact2.country) &&
					StructuralComparisons.StructuralEqualityComparer.Equals(contact1.streetAddress, contact2.streetAddress) &&
					StructuralComparisons.StructuralEqualityComparer.Equals(contact1.streetAddressAdditional, contact2.streetAddressAdditional) &&
					StructuralComparisons.StructuralEqualityComparer.Equals(contact1.postalCode, contact2.postalCode) &&
					StructuralComparisons.StructuralEqualityComparer.Equals(contact1.city, contact2.city) &&
					StructuralComparisons.StructuralEqualityComparer.Equals(contact1.poBox, contact2.poBox) &&
					StructuralComparisons.StructuralEqualityComparer.Equals(contact1.birthday, contact2.birthday) &&
					StructuralComparisons.StructuralEqualityComparer.Equals(contact1.websites, contact2.websites) &&
					StructuralComparisons.StructuralEqualityComparer.Equals(contact1.relationship, contact2.relationship) &&
					StructuralComparisons.StructuralEqualityComparer.Equals(contact1.notes, contact2.notes);
		}

		public static bool AreContactSecretsEqual(ContactSecret contactSecret1, ContactSecret contactSecret2)
		{
			return StructuralComparisons.StructuralEqualityComparer.Equals(contactSecret1.audalfData, contactSecret2.audalfData) && AreSymmetricKeyAlgorithmsEqual(contactSecret1.algorithm, contactSecret2.algorithm);
		}

		public static bool AreSymmetricKeyAlgorithmsEqual(SymmetricKeyAlgorithm symmetricKeyAlgorithm1, SymmetricKeyAlgorithm symmetricKeyAlgorithm2)
		{
			return StructuralComparisons.StructuralEqualityComparer.Equals(symmetricKeyAlgorithm1.GetSettingsAsBytes(), symmetricKeyAlgorithm2.GetSettingsAsBytes());
		}
	}

	public static class CalculateEntropy
	{
		public static double ShannonEntropy(byte[] byteArray)
		{
			var dictionary = new Dictionary<byte, int>();

			foreach (byte b in byteArray)
			{
				if (!dictionary.ContainsKey(b))
				{
					dictionary.Add(b, 1);
				}
				else
				{
					dictionary[b] += 1;
				}
			}

			double result = 0.0;
			int length = byteArray.Length;
			
			foreach (var item in dictionary)
			{
				var frequency = (double)item.Value / length;
				result -= frequency * (Math.Log(frequency) / Math.Log(2));
			}

			return result;
		}
	}
}