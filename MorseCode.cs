using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;

namespace MorseCodeEncDec
{
    public static class MorseCode
    {
        private static Dictionary<string, string> dict = new Dictionary<string, string>();
        private static Dictionary<string, string> reverseDict = new Dictionary<string, string>();


        public static IDictionary<string, string> ASCIIToMorseMapping => new ReadOnlyDictionary<string, string>(dict);
        public static IDictionary<string, string> MorseToASCIIMapping => new ReadOnlyDictionary<string, string>(reverseDict);

        private static string SanitizeString(string text)
        {
            string normalizedText = text.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new();
            foreach (var x in normalizedText)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(x) != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(x);
                }
            }

            return sb.ToString().Normalize(NormalizationForm.FormC).ToUpper();
        }


        public static string Encode(string text)
        {
            string sanitized = SanitizeString(text);

            List<string> words = new();

            foreach (var word in sanitized.Split(" "))
            {

                List<string> codes = new();

                char[] chr = word.ToCharArray();

                for (var i = 0; i < chr.Length; i++)
                {
                    char? prev = (i - 1) < 0 ? null : chr[i - 1];
                    char toEncode = chr[i];

                    if (prev == 'C' && toEncode == 'H')
                    {
                        // Slava jazyku ceskemu.

                        codes.RemoveAt(codes.Count - 1);
                        codes.Add(dict["CH"]);
                        continue;
                    }

                    try
                    {
                        codes.Add(dict[toEncode.ToString()]);
                    }
                    catch
                    {
                        throw new InvalidDataException("Invalid input");
                    }
                }

                words.Add(string.Join(" / ", codes));

            }

            return string.Join(" // ", words);
        }

        public static string Decode(string code) {
            var codes = code.Split("//").Select(x => x.Trim());

            StringBuilder sbEnd = new();

            foreach(var word in codes) {
                StringBuilder sb = new();

                foreach(var singleCode in word.Split("/").Select(x => x.Trim())) {
                    try {
                        sb.Append(reverseDict[singleCode]);
                    } catch {
                        throw new InvalidDataException("Invalid input");
                    }
                }

                sbEnd.Append(sb.ToString());
                sbEnd.Append(" ");
            }

            return sbEnd.ToString();
        }




        static MorseCode()
        {
            dict.Add("A", ".-");
            dict.Add("B", "-...");
            dict.Add("C", "-.-.");
            dict.Add("D", "-..");
            dict.Add("E", ".");
            dict.Add("F", "..-.");
            dict.Add("G", "--.");
            dict.Add("H", "....");
            dict.Add("CH", "----");
            dict.Add("I", "..");
            dict.Add("J", ".---");
            dict.Add("K", "-.-");
            dict.Add("L", ".-..");
            dict.Add("M", "--");
            dict.Add("N", "-.");
            dict.Add("O", "---");
            dict.Add("P", ".--.");
            dict.Add("Q", "--.-");
            dict.Add("R", ".-.");
            dict.Add("S", "...");
            dict.Add("T", "-");
            dict.Add("U", "..-");
            dict.Add("V", "...-");
            dict.Add("W", ".--");
            dict.Add("X", "-..-");
            dict.Add("Y", "-.--");
            dict.Add("Z", "--..");

            dict.Add(".", ".-.-.-.");
            dict.Add(",", "--..--");
            dict.Add("?", "..--..");
            dict.Add("!", "--..-");
            dict.Add(";", "-.-.-.");
            dict.Add(":", "---...");
            //dict.Add("(", "--...");
            dict.Add("(", "-.--.");
            dict.Add(")", "-.--.-");
            dict.Add("\"", ".-..-.");
            dict.Add("-", "-....-");
            dict.Add("_", "..--.-");
            dict.Add("@", ".--.-.");

            dict.Add("1", ".----");
            dict.Add("2", "..---");
            dict.Add("3", "...--");
            dict.Add("4", "....-");
            dict.Add("5", ".....");
            dict.Add("6", "-....");
            dict.Add("7", "--...");
            dict.Add("8", "---..");
            dict.Add("9", "----.");
            dict.Add("0", "-----");

            foreach (var key in dict.Keys)
            {
                reverseDict.Add(dict[key], key);
            }
        }
    }
}