using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Pluralize.NET;

namespace POCOGenerator.Utils
{
    public static class NameHelper
    {
        #region Singular & Plural

        private static readonly Regex regexCamelCase = new Regex("(?<word>[A-Z]{2,}|[A-Z][^A-Z]*?|^[^A-Z]*?)(?=[A-Z]|$)", RegexOptions.Compiled);
        private static readonly Regex regexDigits = new Regex("\\d+", RegexOptions.Compiled);

        public static string GetSingularName(string name)
        {
            return WordQuantifier(name, word => ToSingular(word));
        }

        public static string GetPluralName(string name)
        {
            return WordQuantifier(name, word => ToPlural(word));
        }

        private static string WordQuantifier(string name, Func<string, string> quantifier)
        {
            if (string.IsNullOrEmpty(name))
                return name;

            string word = name;
            int index = 0;

            if (word.Length > 0 && word[word.Length - 1] != '_')
            {
                int index1 = word.LastIndexOf('_');
                if (index1 != -1)
                {
                    index = index1 + 1;
                    word = word.Substring(index);
                }
            }

            if (regexDigits.IsMatch(word))
                return name + "s";

            Match match = regexCamelCase.Matches(word).Cast<Match>().LastOrDefault();
            if (match != null)
            {
                word = match.Groups["word"].Value;
                index += match.Groups["word"].Index;
            }

            string quantifiedWord = quantifier(word);

            if (quantifiedWord == word)
                return name;

            if (name.Length == word.Length)
                return quantifiedWord;

            return name.Substring(0, index) + quantifiedWord;
        }

        private static string ToSingular(string word)
        {
            return (
                (string.Compare(word, word.ToUpper(), false) == 0) ?
                new Pluralizer().Singularize(word.ToLower()).ToUpper() :
                new Pluralizer().Singularize(word)
            );
        }

        private static string ToPlural(string word)
        {
            return (
                (string.Compare(word, word.ToUpper(), false) == 0) ?
                new Pluralizer().Pluralize(word.ToLower()).ToUpper() :
                new Pluralizer().Pluralize(word)
            );
        }

        #endregion

        #region Transform Name

        public static string TransformName(string name, string wordsSeparator = null, bool isCamelCase = true, bool isUpperCase = false, bool isLowerCase = false)
        {
            List<string> words = GetWords(name);

            name = null;
            int index = 0;
            foreach (string word in words)
            {
                if (isCamelCase)
                    name += word.Substring(0, 1).ToUpper() + word.Substring(1).ToLower();
                else if (isUpperCase)
                    name += word.ToUpper();
                else if (isLowerCase)
                    name += word.ToLower();
                else
                    name += word;

                index++;
                if (index < words.Count && string.IsNullOrEmpty(wordsSeparator) == false)
                    name += wordsSeparator;
            }

            return name;
        }

        public static List<string> GetWords(string name)
        {
            List<string> camelCaseWords = new List<string>();

            string[] words = name.Split('_');
            foreach (string word in words)
            {
                foreach (Match match in regexCamelCase.Matches(word))
                    camelCaseWords.Add(match.Groups["word"].Value);
            }

            return camelCaseWords;
        }

        #endregion

        #region Clean Name

        public static string CleanName(string name)
        {
            name = name.Replace(" ", string.Empty).Replace('-', '_').Trim();
            if (name.Length > 0 && '0' <= name[0] && name[0] <= '9')
                name = "_" + name;
            return name;
        }

        #endregion

        #region Clean Enum Literal

        public static string CleanEnumLiteral(string name)
        {
            name = name.Replace(' ', '_').Replace('-', '_').Replace(',', '_').Trim();
            if (name.Length > 0 && '0' <= name[0] && name[0] <= '9')
                name = "_" + name;
            return name;
        }

        #endregion

        #region Name Prefix

        private static readonly List<string> NamePrefixes = new List<string>() {
            "first",
            "second",
            "third",
            "fourth",
            "fifth",
            "sixth",
            "seventh",
            "eighth",
            "ninth",
            "tenth",
            "eleventh",
            "twelfth",
            "primary",
            "secondary",
            "tertiary",
            "quaternary",
            "quinary",
            "senary",
            "septenary",
            "octonary",
            "novenary",
            "decenary",
            "undenary",
            "duodenary",
            "current",
            "previous",
            "initial",
            "starting",
            "last",
            "ending"
        };

        public static string AddNamePrefix(string name, string columnName)
        {
            string columnNameLower = columnName.ToLower();
            string prefix = NamePrefixes.OrderByDescending(p => p.Length).FirstOrDefault(p => columnNameLower.StartsWith(p));
            if (string.IsNullOrEmpty(prefix) == false)
                name = columnName.Substring(0, prefix.Length) + name;
            return name;
        }

        #endregion

        #region Name Verbs

        private class ConjugatedVerb
        {
            public string Verb { get; set; }
            public string PastParticipleVerb { get; set; }
            public List<string> VerbVariations { get; set; }

            public ConjugatedVerb(string verb, string pastParticipleVerb)
            {
                this.Verb = verb;
                this.PastParticipleVerb = pastParticipleVerb;
                this.VerbVariations = new List<string>();
            }
        }

        private static readonly List<ConjugatedVerb> ConjugatedVerbs = new List<ConjugatedVerb>()
        {
            new ConjugatedVerb("insert", "inserted"),
            new ConjugatedVerb("update", "updated"),
            new ConjugatedVerb("delete", "deleted"),
            new ConjugatedVerb("create", "created"),
            new ConjugatedVerb("write", "written"),
            new ConjugatedVerb("ship", "shipped"),
            new ConjugatedVerb("send", "sent"),
        };

        private static readonly List<string> VerbVariations = new List<string>()
        {
            "{0}",
            "{0}by",
            "{0}_by",
            "{0}id",
            "{0}_id",
            "user{0}",
            "user_{0}",
            "{0}user",
            "{0}_user",
            "person{0}",
            "person_{0}",
            "{0}person",
            "{0}_person"
        };

        private static List<string> Variations;

        static NameHelper()
        {
            BuildNameVerbsAndVariations();
        }

        private static void BuildNameVerbsAndVariations()
        {
            foreach (var conjugations in ConjugatedVerbs)
            {
                foreach (var variation in VerbVariations)
                {
                    conjugations.VerbVariations.Add(string.Format(variation, conjugations.Verb));
                    conjugations.VerbVariations.Add(string.Format(variation, conjugations.PastParticipleVerb));
                }

                // order length descending
                conjugations.VerbVariations.Sort((x, y) => (x.Length == y.Length ? 0 : (x.Length < y.Length ? 1 : -1)));
            }

            Variations = ConjugatedVerbs.SelectMany(p => p.VerbVariations).ToList();

            // order length descending
            Variations.Sort((x, y) => (x.Length == y.Length ? 0 : (x.Length < y.Length ? 1 : -1)));
        }

        public static bool IsNameVerb(string name)
        {
            bool hasTime = (name.IndexOf("time", StringComparison.OrdinalIgnoreCase) != -1);
            if (hasTime)
                return false;

            bool hasDate = (name.IndexOf("date", StringComparison.OrdinalIgnoreCase) != -1);
            bool hasShip = (name.IndexOf("ship", StringComparison.OrdinalIgnoreCase) != -1);
            if (hasDate == false && hasShip == false)
                return Variations.Any(variation => name.IndexOf(variation, StringComparison.OrdinalIgnoreCase) != -1);

            if (hasDate)
            {
                bool hasUpdate = (name.IndexOf("update", StringComparison.OrdinalIgnoreCase) != -1);
                if (hasUpdate == false)
                    return false;

                int index = name.IndexOf("date", StringComparison.OrdinalIgnoreCase);
                do
                {
                    hasUpdate =
                        (index - 2) >= 0 &&
                        name.IndexOf("update", index - 2, StringComparison.OrdinalIgnoreCase) == (index - 2);

                    if (hasUpdate == false)
                        return false;

                    index = name.IndexOf("date", index + 4, StringComparison.OrdinalIgnoreCase);
                }
                while (index != -1);
            }

            if (hasShip)
            {
                bool hasShipment = (name.IndexOf("shipment", StringComparison.OrdinalIgnoreCase) != -1);
                if (hasShipment)
                    return false;
            }

            return true;
        }

        public static string ConjugateNameVerbToPastParticiple(string name)
        {
            ConjugatedVerb conjugations = ConjugatedVerbs.FirstOrDefault(cv => cv.VerbVariations.Any(variation => name.IndexOf(variation, StringComparison.OrdinalIgnoreCase) != -1));

            if (conjugations == null)
                return name;

            // verb past participle
            if (name.IndexOf(conjugations.PastParticipleVerb, StringComparison.OrdinalIgnoreCase) != -1)
                return name;

            // verb
            int index = name.IndexOf(conjugations.Verb, StringComparison.OrdinalIgnoreCase);
            if (index != -1)
                return conjugations.PastParticipleVerb.Substring(0, 1).ToUpper() + conjugations.PastParticipleVerb.Substring(1).ToLower() + "By";

            return name;
        }

        #endregion

        #region Escape

        public static string Escape(string text)
        {
            return text.Replace("\\", "\\\\").Replace("\"", "\\\"");
        }

        #endregion
    }
}
