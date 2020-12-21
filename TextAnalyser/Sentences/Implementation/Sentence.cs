using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using TextAnalyser.Enums;
using TextAnalyser.SentenceElements;
using TextAnalyser.SentenceElements.Implementation;

namespace TextAnalyser.Sentences.Implementation
{
    public class Sentence : ISentence
    {
        private ICollection<ISentenceElement> _sentenceElements;
        public ICollection<ISentenceElement> SentenceElements
        {
            get
            {
                GetSentenceTypes();

                return _sentenceElements;
            }
            set
            {
                if (value.Count == 0)
                {
                    throw new ArgumentException("Sentence element can't be empty");
                }
                _sentenceElements = value;
            }
        }

        public IEnumerable<SentenceType> SentenceTypes { get; set; }

        public Sentence()
        {
            _sentenceElements = new Collection<ISentenceElement>();
            SentenceTypes = new Collection<SentenceType>();
        }
        public Sentence(ICollection<ISentenceElement> sentenceElements)
        {
            SentenceElements = sentenceElements;
        }

        /// <summary>
        /// Method to get type of sertain sentence
        /// </summary>
        public void GetSentenceTypes()
        {
            ICollection<SentenceType> sentenceTypes = new Collection<SentenceType>();

            if (_sentenceElements.Last() is Separator lastSeparator && lastSeparator.IsQuestionMark())
            {
                sentenceTypes.Add(SentenceType.Interrogative);
            }
            else
            {
                sentenceTypes.Add(SentenceType.Simple);
            }

            SentenceTypes = sentenceTypes;
        }

        /// <summary>
        /// Overriding method ToStrong so our Sentence class will represent needed values
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var stringBuilder = new StringBuilder();

            foreach (var element in SentenceElements)
            {
                stringBuilder.Append(element);
            }

            return stringBuilder.ToString();
        }
    }
}
