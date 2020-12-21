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
                sentenceElement = _sentenceElements[key];
            }
            else
            {
                if (Separators.AllSentenceSeparators.Contains(key))
                {
                    sentenceElement = new Separator(key);
                }
                else
                {
                    sentenceElement = new Word(key);
                }
                _sentenceElements.Add(key, sentenceElement);
            }

            return sentenceElement;
        }
    }
}
