using System.Linq;

namespace TextAnalyser.SentenceElements.Implementation
{
    public class Separator : SentenceElement, ISeparator
    {
        public Separator(string str) : base(str)
        {
            Chars = str;
        }
        /// <summary>
        /// Method to recognize if our separator starts with vovel letter (needed for task 3)
        /// </summary>
        /// <returns></returns>
        public bool IsQuestionMark()
        {
            return Chars.Contains('?');
        }

        public bool IsSentenceSeparator()
        {
            return Separators.SentenceSeparators.Any(x => Chars.Equals(x));
        }

        public bool IsSpaceMark()
        {
            return Chars.Equals(Separators.Space);
        }

        public bool IsWordSeparator()
        {
            return Separators.WordSeparators.Any(x => Chars.Equals(x));
        }
        public override string ToString()
        {
            return Chars;
        }
    }
}
