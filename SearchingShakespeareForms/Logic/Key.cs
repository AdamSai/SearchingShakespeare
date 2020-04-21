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

        public Key(string wordKey, int startIndex, int lastIndex)
        {
            WordKey = wordKey;
            StartIndex = startIndex;
            LastIndex = lastIndex;
            charStart = WordKey[StartIndex];
            charEnd = WordKey[LastIndex];
        }

        public int CompareTo(Key other)
        {
            var originalChar = char.ToLower(WordKey[StartIndex]);
            var otherChar = char.ToLower(other.WordKey[other.StartIndex]);
            if (originalChar > otherChar) return 1;
            if (originalChar < otherChar) return -1;
            return 0;
        }

        public bool StartsWithSameCharacter(Key other)
        {
            var originalChar = char.ToLower(WordKey[StartIndex]);
            var otherChar = char.ToLower(other.WordKey[other.StartIndex]);
            return originalChar == otherChar;
        }

        public int CountMatchingCharacters(Key other)
        {
            var matchingChars = 0;
            var otherLength = other.Length;
            var shortest = Length < otherLength ? Length : otherLength;

            for (var i = 0; i < shortest; i++)
            {
                if (char.ToLower(WordKey[StartIndex + i]) == char.ToLower(other.WordKey[other.StartIndex + i]))
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
            var d = WordKey[StartIndex];
            d = WordKey[LastIndex];

            return $"Start: [{StartIndex:N0}] '{WordKey[StartIndex]}' Last: [{LastIndex}] '{WordKey[LastIndex]}'";
        }
    }
}