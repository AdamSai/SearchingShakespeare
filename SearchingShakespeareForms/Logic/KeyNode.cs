using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SearchingShakespeare
{
    public class KeyNode : Node
    {
        private readonly int _value;

        public KeyNode(Key key, int value) : base(key)
        {
            Key = key;
            _value = value;
        }
        
        public new LinkedNode Add(Key key, int value)
        {
            var tmp = More;
            var newNode = new LinkedNode(Key)
            {
                More = tmp
            };
            newNode.Add(key, value, _value);
            return newNode;
        }

        public string GetKeyValue(int maxLength = 150)
        {
            var value = $"[{_value:N0}] {Key.WordKey.Substring(_value, maxLength > Key.Length ? Key.Length : maxLength)}";
            var res = Regex.Replace(value, "\\s+", " ", RegexOptions.Multiline);
            return res;
        }
      
    }
}