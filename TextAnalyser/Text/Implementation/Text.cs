using System;
using System.Collections.Generic;
using System.Text;
using TextAnalyser.Sentences.Implementation;

namespace TextAnalyser.Text.Implementation
{
    public class Text : IText
    {
        private List<Sentence> _sentences;
        public List<Sentence> Sentences
        {
            get => _sentences;
            private set
            {
                if (value.Count == 0)
                {
                    throw new ArgumentException("Sentence can't be empty", nameof(value));
                }

                _sentences = value;
            }
        }
        public Text(List<Sentence> sentences)
        {
            Sentences = sentences;
        }
        /// <summary>
        /// Overriding method ToStrong so our Text class will represent needed values
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var stringBuilder = new StringBuilder();

            foreach (var item in Sentences)
            {
                stringBuilder.Append(item);
            }

            return stringBuilder.ToString();
        }
    }
}
