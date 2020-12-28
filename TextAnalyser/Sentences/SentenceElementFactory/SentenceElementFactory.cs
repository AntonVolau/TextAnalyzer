using System.Collections.Generic;
using System.Linq;
using TextAnalyser.SentenceElements;
using TextAnalyser.SentenceElements.Implementation;

namespace TextAnalyser.Sentences.SentenceElementFactory
{
    /// <summary>
    /// This static class separates elements by separator and word types
    /// </summary>
    public class SentenceElementFactory
    {
        private readonly IDictionary<string, ISentenceElement> _sentenceElements =
            new Dictionary<string, ISentenceElement>();

        public ISentenceElement GetSentenceElement(string key)
        {
            ISentenceElement sentenceElement;
            if (_sentenceElements.ContainsKey(key))
            {
                sentenceElement = _sentenceElements[key]; // if our sentence already contains such element, we don't need to reassign it's type
            }
            else
            {
                if (Separators.AllSentenceSeparators.Contains(key)) // if our sentence element is sentence separator, we assign it type as separator
                {
                    sentenceElement = new Separator(key);
                }
                else // else we assign it type as word
                {
                    sentenceElement = new Word(key);
                }
                _sentenceElements.Add(key, sentenceElement);
            }

            return sentenceElement;
        }
    }
}
