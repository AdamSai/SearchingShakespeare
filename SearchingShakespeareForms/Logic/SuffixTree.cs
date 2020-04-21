using System;
using System.Collections.Generic;

namespace SearchingShakespeare
{
    public class SuffixTree
    {
        private Node root;
        public bool HasLoadedText { get; private set; } = false;

        public SuffixTree(string text)
        {
            // We only use the root node to search and insert values. It does not require any
            // values itself.
            root = new KeyNode(null, 0);
            for (var i = 0; i < text.Length; i++)
            {
                if (text[i] == '\n' || text[i] == '\r') continue;
                root.Add(new Key(text, i, text.Length - 1), i);
            }

            HasLoadedText = true;
        }

        public List<string> Search(string searchString, int resultsMax = 20)
        {
            var searchKey = new Key(searchString, 0, searchString.Length - 1);
            var resultRoot = root.Locate(searchKey);
            List<string> results;
            if (resultRoot == null)
            {
                results = new List<string>
                {
                    "No results found"
                };
            }
            else
            {
                if(resultRoot is LinkedNode linkedNode)
                    results = linkedNode.Next.GetValues(resultsMax);
                else 
                    results = new List<string> {((KeyNode) resultRoot).GetKeyValue()};

            }

            return results;
        }
    }
}