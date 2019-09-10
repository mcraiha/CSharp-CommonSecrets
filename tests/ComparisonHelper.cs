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
		public static bool AreNotesEqual(Note note1, Note note2)
		{
			return note1.noteTitle == note2.noteTitle && note1.noteText == note2.noteText;
		}

		public static bool AreNotesSecretEqual(NoteSecret noteSecret1, NoteSecret noteSecret2)
		{
			return StructuralComparisons.StructuralEqualityComparer.Equals(noteSecret1.audalfData, noteSecret2.audalfData) && AreSymmetricKeyAlgorithmsEqual(noteSecret1.algorithm, noteSecret2.algorithm);
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