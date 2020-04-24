using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using static SearchingShakespeare.Utility;

namespace SearchingShakespeare
{
    public class KeyNode : Node
    {
        public int Value { get; }

        public KeyNode(Key key, int value) : base(key)
        {
            Key = key;
            Value = value;
        }

        public new LinkedNode Add(Key key, int value)
        {
            var tmp = More;
            var matchingChars = Key.CountMatchingCharacters(key);
            var nextKey = new Key(Key.WordKey,
                MathClamp(Key.StartIndex + matchingChars, Key.StartIndex, Key.LastIndex), Key.LastIndex,
                Key.lowerWord);
            var nextMoreKey = new Key(key.WordKey,
                MathClamp(key.StartIndex + matchingChars, key.StartIndex, key.LastIndex), key.LastIndex,
                key.lowerWord);
//            if (Key.Length == 1 && nextMoreKey.Length == 1)
//            {
//                var tmp1 = More;
//                var node = new LinkedNode(Key)
//                {
//                    More = new KeyNode(Key, Value)
//                    {
//                        More = new KeyNode(nextMoreKey, value)
//                        {
//                            More = tmp1
//                        }
//                    },
//                };
//                return node;
//            }

            Key.LastIndex = MathClamp(Key.StartIndex + matchingChars - 1, Key.StartIndex, Key.LastIndex);
            var newNode = new LinkedNode(Key)
            {
                Next = new KeyNode(nextKey, Value)
                {
                    More = new KeyNode(nextMoreKey, value)
                },
                More = tmp
            };
            return newNode;
        }

        public string GetKeyValue(int maxLength = 90)
        {
            var value = $"[{Value:N0}] {Key.WordKey.Substring(Value, maxLength > Key.Length ? Key.Length : maxLength)}";
            var res = Regex.Replace(value, "\\s+", " ", RegexOptions.Multiline);
            return res;
        }
    }
}