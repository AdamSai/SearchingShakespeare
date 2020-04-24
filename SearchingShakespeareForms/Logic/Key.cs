using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace SearchingShakespeare
{
    public class Key
    {
        public string WordKey;
        public int StartIndex, LastIndex;
        public int Length => LastIndex - StartIndex + 1;
        private char charStart;
        private char charEnd;
        public string lowerWord;

        public Key(string wordKey, int startIndex, int lastIndex, string lowerWord)
        {
            WordKey = wordKey;
            StartIndex = startIndex;
            LastIndex = lastIndex;
            charStart = WordKey[StartIndex];
            charEnd = WordKey[LastIndex];
            this.lowerWord = lowerWord;
        }

        public int CompareTo(Key other)
        {
            var originalChar = (lowerWord[StartIndex]);
            var otherChar = (other.lowerWord[other.StartIndex]);
            if (originalChar > otherChar) return 1;
            if (originalChar < otherChar) return -1;
            return 0;
        }

        public bool StartsWithSameCharacter(Key other)
        {
            var originalChar = (lowerWord[StartIndex]);
            var otherChar = (other.lowerWord[other.StartIndex]);
            return originalChar == otherChar;
        }

        public int CountMatchingCharacters(Key other)
        {
            var matchingChars = 0;
            var otherLength = other.Length;
            var shortest = Length < otherLength ? Length : otherLength;

            for (var i = 0; i < shortest; i++)
            {
                if ((lowerWord[StartIndex + i]) == (other.lowerWord[other.StartIndex + i]))
                {
                    ++matchingChars;
                    continue;
                }
                break;
            }

            return matchingChars;
        }

        public override string ToString()
        {
            var d = lowerWord[StartIndex];
            d = lowerWord[LastIndex];

            return $"Start: [{StartIndex:N0}] '{WordKey[StartIndex]}' Last: [{LastIndex}] '{WordKey[LastIndex]}'";
        }
    }
}