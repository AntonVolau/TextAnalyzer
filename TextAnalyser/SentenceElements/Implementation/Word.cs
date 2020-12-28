using System.Linq;

namespace TextAnalyser.SentenceElements.Implementation
{
    public class Word : SentenceElement, IWord
    {
        public int Length => Chars.Length;
        public Word(string str) : base(str)
        {
            Chars = str;
        }

        /// <summary>
        /// Method to recognize if our word starts with vovel letter (needed for task 3)
        /// </summary>
        /// <returns></returns>
        public bool StartsWithVovel()
        {
            char[] vowels = new char[] { 'a', 'e', 'i', 'o', 'u', 'y' };
            return vowels.Any(vowel => vowel == Chars.ToLower().First());
        }
        /// <summary>
        /// Overriding method ToString so our Word class will represent needed values
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Chars;
        }
    }
}
