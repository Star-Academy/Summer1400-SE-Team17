using System.Text;

namespace Phase11.model.parser
{
    public class PorterStemmer
    {
        public string StemWord(string word)
        {
            string stem = word.ToLower();
            if (stem.Length < 3) return stem;
            stem = StemStep1a(stem);
            stem = StemStep1b(stem);
            stem = StemStep1c(stem);
            stem = StemStep2(stem);
            stem = StemStep3(stem);
            stem = StemStep4(stem);
            stem = StemStep5a(stem);
            stem = StemStep5b(stem);
            return stem;
        }

        private string StemStep1a(string input)
        {
            if (input.EndsWith("sses"))
            {
                return RemoveLastCharacters(input, 2);
            }
            if (input.EndsWith("ies"))
            {
                return RemoveLastCharacters(input, 2);
            }
            if (input.EndsWith("ss"))
            {
                return input;
            }
            if (input.EndsWith("s"))
            {
                return RemoveLastCharacters(input, 1);
            }
            return input;
        }

        private string RemoveLastCharacters(string input, int count)
        {
            return input.Substring(0, input.Length - count);
        }

        private string StemStep1b(string input)
        {
            if (input.EndsWith("eed"))
            {
                string stem = RemoveLastCharacters(input, 1);
                string letterTypes = GetLetterTypes(stem);
                int m = GetM(letterTypes);
                if (m > 0) return stem;
                return input;
            }
            if (input.EndsWith("ed"))
            {
                string stem = RemoveLastCharacters(input, 2);
                string letterTypes = GetLetterTypes(stem);
                if (letterTypes.Contains("v"))
                {
                    return Step1b2(stem);
                }
                return input;
            }
            if (input.EndsWith("ing"))
            {
                string stem = RemoveLastCharacters(input, 3);
                string letterTypes = GetLetterTypes(stem);
                if (letterTypes.Contains("v"))
                {
                    return Step1b2(stem);
                }
                return input;
            }
            return input;
        }

        private string Step1b2(string input)
        {
            if (input.EndsWith("at"))
            {
                return input + "e";
            }
            else if (input.EndsWith("bl"))
            {
                return input + "e";
            }
            else if (input.EndsWith("iz"))
            {
                return input + "e";
            }
            else
            {
                char lastDoubleConsonant = GetLastDoubleConsonant(input);
                if (lastDoubleConsonant != 0 && lastDoubleConsonant != 'l' && lastDoubleConsonant != 's' && lastDoubleConsonant != 'z')
                {
                    return RemoveLastCharacters(input, 1);
                }
                else
                {
                    string letterTypes = GetLetterTypes(input);
                    int m = GetM(letterTypes);
                    if (m == 1 && IsStarO(input))
                    {
                        return input + "e";
                    }

                }
            }
            return input;
        }

        private string StemStep1c(string input)
        {
            if (input.EndsWith("y"))
            {
                string stem = RemoveLastCharacters(input, 1);
                string letterTypes = GetLetterTypes(stem);
                if (letterTypes.Contains("v")) return stem + "i";
            }
            return input;
        }

        private string StemStep2(string input)
        {
            string[] s1 = new string[]{"ational", "tional", "enci", "anci", "izer", "bli", "alli", "entli", "eli", "ousli",
                "ization", "ation", "ator", "alism", "iveness", "fulness", "ousness", "aliti", "iviti", "biliti", "logi",
        };
            string[] s2 = new string[]{
                "ate", "tion", "ence", "ance", "ize", "ble", "al", "ent", "e", "ous", "ize", "ate", "ate", "al", "ive",
                "ful", "ous", "al", "ive", "ble", "log"
        };
            return ReplaceSuffix(input, s1, s2);
        }

        private string ReplaceSuffix(string input, string[] s1, string[] s2)
        {
            for (int i = 0; i < s1.Length; i++)
            {
                if (input.EndsWith(s1[i]))
                {
                    string stem = RemoveLastCharacters(input, s1[i].Length);
                    string letterTypes = GetLetterTypes(stem);
                    int m = GetM(letterTypes);
                    if (m > 0) return stem + s2[i];
                    return input;
                }
            }
            return input;
        }

        private string StemStep3(string input)
        {
            string[] s1 = new string[] { "icate", "ative", "alize", "iciti", "ical", "ful", "ness" };
            string[] s2 = new string[] { "ic", "", "al", "ic", "ic", "", "" };
            return ReplaceSuffix(input, s1, s2);

        }

        private string StemStep4(string input)
        {
            string[] suffixes = new string[]{
                "al", "ance", "ence", "er", "ic", "able", "ible", "ant", "ement", "ment", "ent", "ion", "ou", "ism",
                "ate", "iti", "ous", "ive", "ize"};
            foreach (string suffix in suffixes)
            {
                if (input.EndsWith(suffix))
                {
                    string stem = RemoveLastCharacters(input, suffix.Length);
                    string letterTypes = GetLetterTypes(stem);
                    int m = GetM(letterTypes);
                    if (m > 1)
                    {
                        if (suffix.Equals("ion"))
                        {
                            if (stem[stem.Length - 1] == 's' || stem[stem.Length - 1] == 't')
                            {
                                return stem;
                            }
                        }
                        else
                        {
                            return stem;
                        }
                    }
                    return input;
                }
            }
            return input;
        }

        private string StemStep5a(string input)
        {
            if (input.EndsWith("e"))
            {
                string stem = RemoveLastCharacters(input, 1);
                string letterTypes = GetLetterTypes(stem);
                int m = GetM(letterTypes);
                if (m > 1)
                {
                    return stem;
                }
                if (m == 1 && !IsStarO(stem))
                {
                    return stem;
                }
            }
            return input;
        }

        private string StemStep5b(string input)
        {
            string letterTypes = GetLetterTypes(input);
            int m = GetM(letterTypes);
            if (m > 1 && input.EndsWith("ll"))
            {
                return RemoveLastCharacters(input, 1);
            }
            return input;
        }

        private char GetLastDoubleConsonant(string input)
        {
            if (input.Length < 2) return (char)0;
            char lastLetter = input[input.Length - 1];
            char penultimateLetter = input[input.Length - 2];
            if (lastLetter == penultimateLetter && GetLetterType((char)0, lastLetter) == 'c')
            {
                return lastLetter;
            }
            return (char)0;
        }

        private bool IsStarO(string input)
        {
            if (input.Length < 3) return false;

            char lastLetter = input[input.Length - 1];
            if (lastLetter == 'w' || lastLetter == 'x' || lastLetter == 'y') return false;

            char secondToLastLetter = input[input.Length - 2];
            char thirdToLastLetter = input[input.Length - 3];
            char fourthToLastLetter = (char)(input.Length == 3 ? 0 : input[input.Length - 4]);
            return GetLetterType(secondToLastLetter, lastLetter) == 'c'
                    && GetLetterType(thirdToLastLetter, secondToLastLetter) == 'v'
                    && GetLetterType(fourthToLastLetter, thirdToLastLetter) == 'c';
        }

        private string GetLetterTypes(string input)
        {
            StringBuilder letterTypes = new StringBuilder(input.Length);
            for (int i = 0; i < input.Length; i++)
            {
                char letter = input[i];
                char previousLetter = (char)(i == 0 ? 0 : input[i - 1]);
                char letterType = GetLetterType(previousLetter, letter);
                if (letterTypes.Length == 0 || letterTypes[letterTypes.Length - 1] != letterType)
                {
                    letterTypes.Append(letterType);
                }
            }
            return letterTypes.ToString();
        }

        int GetM(string letterTypes)
        {
            if (letterTypes.Length < 2) return 0;
            if (letterTypes[0] == 'c') return (letterTypes.Length - 1) / 2;
            return letterTypes.Length / 2;
        }

        private char GetLetterType(char previousLetter, char letter)
        {
            switch (letter)
            {
                case 'a':
                case 'e':
                case 'i':
                case 'o':
                case 'u':
                    return 'v';
                case 'y':
                    if (previousLetter == 0 || GetLetterType((char)0, previousLetter) == 'v')
                    {
                        return 'c';
                    }
                    return 'v';
                default:
                    return 'c';
            }
        }
    }
}

    
