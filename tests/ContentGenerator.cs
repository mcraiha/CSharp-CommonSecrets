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

        private static readonly int asciiPrintableCharsStart = 32;
        private static readonly int asciiPrintableCharsEnd = 126;

        private static string GenerateAsciiCompatibleString(int wantedLength)
        {
            char[] charArray = new char[wantedLength];
            for (int i = 0; i < wantedLength; i++)
            {
                charArray[i] = (char)rng.Next(asciiPrintableCharsStart, asciiPrintableCharsEnd + 1);
            }

            return new string(charArray);
        }

        public static Note GenerateRandomNote()
        {
            int noteTitleLength = rng.Next(6, 20);
            int noteTextLength = rng.Next(3, 4000);

            return new Note(GenerateAsciiCompatibleString(noteTitleLength), GenerateAsciiCompatibleString(noteTextLength));
        }
    }
}