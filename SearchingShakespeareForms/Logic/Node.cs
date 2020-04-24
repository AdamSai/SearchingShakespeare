using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;

namespace SearchingShakespeare
{
    public abstract class Node
    {
        /// <summary>
        /// This is a node with a reference to the next node continuing the string
        /// </summary>
        protected internal Node More;

        protected internal Key Key;

        public Node(Key key)
        {
            Key = key;
        }


        public virtual void Add(Key key, int value)
        {
            if (More is null)
            {
                More = new KeyNode(key, value);
                return;
            }

            var previous = this;
            Node node;
            (previous, node) = FindPreviousAndMoreNodeWhere(previous, (a) => a.Key.StartsWithSameCharacter(key));
            switch (node)
            {
                case LinkedNode ln:
                    ln.Add(key, value);
                    break;
                case KeyNode kn:
                {
                    previous.More = kn.Add(key, value);
                    break;
                }
                default:
                {
                    previous.More = new KeyNode(key, value);
                    break;
                }
            }
        }

        private (Node, Node) FindPreviousAndMoreNodeWhere(Node previous, Func<Node, bool> func)
        {
            var node = previous.More;
            while (node != null)
            {
                if (func(node))
                {
                    return (previous, node);
                }

                previous = node;
                node = node.More;
            }

            return (previous, null);
        }

        public int Find(Key key)
        {
            throw new NotImplementedException();
        }

        public virtual Node Locate(Key key)
        {
            var node = More;
            var currKey = key;
            var matchedChars = 0;
            while (node != null)
            {

                if (node.Key.StartsWithSameCharacter(currKey))
                {
                    if (node is LinkedNode ln)
                    {
                        var matchingChars = currKey.CountMatchingCharacters(node.Key);
                        matchedChars += matchingChars;
                        // If all chars are matching return.
                        if (matchedChars == key.Length)
                        {
                            return node;
                        }

                        var newCurrKey = new Key(key.WordKey, currKey.StartIndex + matchingChars, key.LastIndex, key.lowerWord);

                        // Look through the LinkedNode Next More nodes to find a matching key
                        var tmp = ln.Next;
                        var foundMatching = false;
                        while (tmp != null)
                        {
                            if (tmp.Key.StartsWithSameCharacter(newCurrKey))
                            {
                                currKey = newCurrKey;
                                node = tmp;
                                foundMatching = true;
                                break;
                            }

                            tmp = tmp.More;
                        }

                        if (foundMatching) continue;

                        currKey = newCurrKey;
                        node = ln.Next.More;
                    }

                    else if (node is KeyNode kn)
                    {
                        return kn;
                    }
                }
                else
                {
                    if (node.More == null)
                    {
                        return null;
                    }
                    node = node.More;
                }
            }

            return node;
        }

        public List<string> GetValues(int limit = 20)
        {
            var count = 0;
            List<string> results = new List<string>();
            Node node = this;
            while (node != null)
            {
                var more = node.More;
                // Go through all More nodes and find values.
                while (more != null)
                {
                    if (more is KeyNode keyNode)
                    {
                        results.Add(keyNode.GetKeyValue());
                        if (results.Count > limit)
                        {
                            results.RemoveRange(limit, results.Count - limit - 1);
                            return results;
                        }
                        count++;
                    }
                    else if (more is LinkedNode linkedNode)
                    {
                        linkedNode.Next.GetValues().ForEach(str =>
                        {
                            if (results.Count < limit)
                            {
                                results.Add(str);
                            }
                        });
                        if (results.Count == limit) return results;
                    }

                    more = more.More;
                }
                
                if (node is KeyNode kn)
                {
                    results.Add(kn.GetKeyValue());
                    
                    if (results.Count > limit)
                    {
                        results.RemoveRange(limit, results.Count - limit - 1);
                        return results;
                    }
                    break;
                }
                if (node is LinkedNode ln)
                {
                    node = ln.Next;
                }
            }

            return results;
        }
        
        public override string ToString()
        {
            return Key?.ToString();
        }
    }
}