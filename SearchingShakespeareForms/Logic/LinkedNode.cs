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
        public override void Add(Key key, int value)
        {
            var matchingChars = key.CountMatchingCharacters(Key);
            var newKeyStartIndex = MathClamp(key.StartIndex + matchingChars, key.StartIndex, key.LastIndex);
            // The new key we want to insert is either the full string or a substring. Subtracting the matching characters of the already
            // existing node, and beginning the new string where the match ends.
            var newKey = new Key(key.WordKey, newKeyStartIndex, key.LastIndex, key.lowerWord);
            
            if (Key.Length == 1 && newKey.Length == 1 && Key.StartsWithSameCharacter(newKey))
            {
                if (More is null)
                {
                    More = new KeyNode(newKey, value);
                    return;
                }

                var temp = More;
                var prev = (Node) this;

                while (temp != null)
                {
                    prev = temp;
                    temp = temp.More;
                }

                if (temp != null) return;

                prev.More = new KeyNode(newKey, value);
            }
            else
            {
                if (Next.Key.StartsWithSameCharacter(newKey))
                {
                    Key.LastIndex = MathClamp(Key.StartIndex + matchingChars - 1, Key.StartIndex, Key.LastIndex);
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
                {
                    Key.LastIndex = MathClamp(Key.StartIndex + matchingChars - 1, Key.StartIndex, Key.LastIndex);
                    Next.More = new KeyNode(newKey, value);
                }
                else
                {
                    var temp = Next.More;
                    var prev = Next;
                    while (temp != null)
                    {
                        if (temp.Key.StartsWithSameCharacter(newKey))
                        {
                            Key.LastIndex = MathClamp(Key.StartIndex + matchingChars - 1, Key.StartIndex, Key.LastIndex);
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

                    if (temp != null) return;
                    Key.LastIndex = MathClamp(Key.StartIndex + matchingChars - 1, Key.StartIndex, Key.LastIndex);
                    prev.More = new KeyNode(newKey, value);
                }
            }
        }

        public override Node Locate(Key key)
        {
            throw new NotImplementedException();
        }
    }
}