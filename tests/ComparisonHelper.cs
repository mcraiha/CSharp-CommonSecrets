using CSCommonSecrets;
using System;
using System.IO;
using System.Text;

namespace Tests
{
    // Simple class for checking that two things are equal
    public static class ComparisonHelper
    {
        public static bool AreNotesEqual(Note note1, Note note2)
        {
            return note1.noteTitle == note2.noteTitle && note1.noteText == note2.noteText;
        }
    }
}