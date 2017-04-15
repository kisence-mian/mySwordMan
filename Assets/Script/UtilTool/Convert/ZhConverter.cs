using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

    public class ZhConverter
    {
        public enum To
        {
            Traditional,
            Simplified
        }

        private static readonly Dictionary<To, ZhConverter> Converters = new Dictionary<To, ZhConverter>();

        private static readonly Dictionary<To, string> PropertyFiles = new Dictionary<To, string>
            {
                {To.Traditional, "zh2Hant"},
                {To.Simplified, "zh2Hans"},
            };


        private static readonly Regex UnicodeRegex = new Regex(@"\\u([0-9A-Za-z][0-9A-Za-z][0-9A-Za-z][0-9A-Za-z])");

        private readonly Dictionary<string, string> _charMap = new Dictionary<string, string>();
        private readonly HashSet<string> _conflictingSets = new HashSet<string>();

        private ZhConverter(string propertyFile)
        {
            string[] lines = ResourceManager.ReadTextFile(propertyFile).Split('\n');

            foreach (var line in lines)
            {
                if (!line.Contains('='))
                    continue;

                var parts = line.Split('=');
                var key = EscapedUnicodeToString(parts[0]);
                var value = EscapedUnicodeToString(parts[1]);
                if (_charMap.ContainsKey(key))
                    _charMap.Remove(key);
                _charMap.Add(key, value);
            }
            InitializeHelper();
        }

        private static string EscapedUnicodeToString(string escaped)
        {
            return new string(
                UnicodeRegex.Matches(escaped)
                            .OfType<Match>()
                            .Select(e => (char)int.Parse(e.Groups[1].Value, NumberStyles.HexNumber))
                            .ToArray()
                );
        }

        public static ZhConverter GetInstance(To converterType)
        {
            if (!Converters.ContainsKey(converterType))
            {
                Converters.Add(converterType, new ZhConverter(PropertyFiles[converterType]));
            }
            return Converters[converterType];
        }

        public static string Convert(string text, To converterType)
        {
            var instance = GetInstance(converterType);
            return instance.Convert(text);
        }

        private void InitializeHelper()
        {
            var stringPossibilities = new Dictionary<string, int>();
            foreach (var key in _charMap.Keys)
            {
                foreach (var length in Enumerable.Range(1, key.Length))
                {
                    var subKey = key.Substring(0, length);
                    var possibility = 1;
                    if (stringPossibilities.ContainsKey(subKey))
                    {
                        possibility = stringPossibilities[subKey] + 1;
                        stringPossibilities.Remove(subKey);
                    }
                    stringPossibilities.Add(subKey, possibility);
                }
            }

            foreach (var key in stringPossibilities.Where(e => e.Value > 1).Select(e => e.Key))
                _conflictingSets.Add(key);
        }

        public string Convert(string input)
        {
            var outString = new StringBuilder();
            var stackString = new StringBuilder();

            foreach (var ch in input)
            {
                stackString.Append(ch);
                var currentStack = stackString.ToString();

                if (_conflictingSets.Contains(currentStack))
                    continue;

                if (_charMap.ContainsKey(currentStack))
                {
                    outString.Append(_charMap[currentStack]);
                    stackString.Remove(0, stackString.Length);
                }
                else
                {
                    stackString.Remove(0, stackString.Length - 1);
                    FlushStack(outString, new StringBuilder(currentStack.Substring(0, currentStack.Length - 1)));
                }
            }

            FlushStack(outString, stackString);

            return outString.ToString();
        }


        private void FlushStack(StringBuilder outString, StringBuilder stackString)
        {
            while (stackString.Length > 0)
            {
                if (_charMap.ContainsKey(stackString.ToString()))
                {
                    outString.Append(_charMap[stackString.ToString()]);
                    stackString.Remove(0, stackString.Length);
                }
                else
                {
                    outString.Append(ParseOneChar(char.ToString(stackString[0])));
                    stackString.Remove(0, 1);
                }
            }
        }

        public string ParseOneChar(string c)
        {
            return _charMap.ContainsKey(c) ? _charMap[c] : c;
        }
    }