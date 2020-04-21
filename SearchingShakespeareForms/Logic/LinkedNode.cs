using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using static SearchingShakespeare.Utility;


namespace SearchingShakespeare
{
    public class LinkedNode : Node
    {
        // Node to represent the continuation of the suffix
        protected internal Node Next;

        public LinkedNode(Key key) : base(key)
        {
            Key = key;
        }

        //Original value is set when calling this method from a KeyNode, to not lose the value.
        public void Add(Key key, int value, int originalValue = -1)
        {
            var matchingChars = key.CountMatchingCharacters(Key);
            var newKeyStartIndex = MathClamp(key.StartIndex + matchingChars, key.StartIndex, key.LastIndex);
            // The new key we want to insert is either the full string or a substring. Subtracting the matching characters of the already
            // existing node, and beginning the new string where the match ends.
            var newKey = new Key(key.WordKey, newKeyStartIndex, key.LastIndex);

            // Create a new Next key, and add the newly inserted key as a More node.
            if (Next is null)
            {
                // If the substring matches the entire key, just add a Next node.
                if (Key.Length == matchingChars)
                {
                    More = new KeyNode(newKey, value);
                    Key.LastIndex = MathClamp(Key.StartIndex + matchingChars - 1, Key.StartIndex, Key.LastIndex);
                }
                // Else create two new key nodes. One with the continuation of the current key as the Next node
                // and one with the continuation of the new key as the Next.More node.
                else
                {
                    var nextStartIndex = MathClamp(Key.StartIndex + matchingChars, Key.StartIndex, Key.LastIndex);
                    var nextKey = new Key(Key.WordKey, nextStartIndex, Key.LastIndex);
                    // Next = new KeyNode(nextKey, nextStartIndex)
                    Next = new KeyNode(nextKey, originalValue)
                    {
                        More = new KeyNode(newKey, value)
                    };
                    Key.LastIndex = MathClamp(Key.StartIndex + matchingChars - 1, Key.StartIndex, Key.LastIndex);
                }
            }
            else
            {
                if (Key.Length > 1)
                {
                    Key.LastIndex = MathClamp(Key.StartIndex + matchingChars - 1, Key.StartIndex, Key.LastIndex);
                    var newNextKey = new Key(Key.WordKey,
                        MathClamp(Key.StartIndex + matchingChars, Key.StartIndex, Key.LastIndex), Key.LastIndex);
                    var tmp = Next;
                    Next = new LinkedNode(newNextKey)
                    {
                        Next = tmp
                    };
                }

                if (Next.Key.StartsWithSameCharacter(newKey))
                {
                    if (Next is LinkedNode linkedNode)
                    {
                        linkedNode.Add(newKey, value);
                    }
                    else if (Next is KeyNode keyNode)
                    {
                        Next = keyNode.Add(newKey, value);
                    }
                    return;
                }

                if (Next.More is null)
                    Next.More = new KeyNode(newKey, value);
                else
                {
                    var temp = Next.More;
                    var prev = Next;
                    while (temp != null)
                    {
                        if (temp.Key.StartsWithSameCharacter(newKey))
                        {
                            if (temp is LinkedNode linkedNode)
                            {
                                linkedNode.Add(newKey, value);
                            }
                            else if (temp is KeyNode keyNode)
                            {
                                prev.More = keyNode.Add(newKey, value);
                            }
                            return;
                        }
                        prev = temp;
                        temp = temp.More;
                    }

                    if (temp == null)
                    {
                        prev.More = new KeyNode(newKey, value);
                    }
                }
            }
        }

        public override Node Locate(Key key)
        {
            throw new NotImplementedException();
        }
    }
}