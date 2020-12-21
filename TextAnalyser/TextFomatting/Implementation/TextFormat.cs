using System;
using System.Collections.Generic;
using System.Linq;
using TextAnalyser.Enums;
using TextAnalyser.SentenceElements;
using TextAnalyser.SentenceElements.Implementation;
using TextAnalyser.Sentences;
using TextAnalyser.Sentences.Implementation;
using TextAnalyser.Text;

namespace TextAnalyser.TextFomatting.Implementation
{
    public static class TextFormat
    {
        public static ICollection<T> SelectElements<T>(ISentence sentence, Func<T, bool> selector = null)
            where T : SentenceElement
        {
            return selector == null
                ? sentence.SentenceElements.OfType<T>().ToList()
                : sentence.SentenceElements.OfType<T>().Where(selector).ToList();
        }
        public static ICollection<Sentence> SelectSentences(IText text, Func<Sentence, bool> selector = null)
        {
            return selector == null ? text.Sentences : text.Sentences.Where(selector).ToList();
        }
        public static IOrderedEnumerable<ISentence> SortSentencesAscending<T>(IText text) where T : SentenceElement
        {
            return text.Sentences.OrderBy(x => SelectElements<T>(x).Count);
        }

        public static IEnumerable<Word> GetWordsFromSentences(IText text, SentenceType sentenceType, int wordLength)
        {
            return SelectSentences(text, x => x.SentenceTypes.Contains(sentenceType))
                .SelectMany(y => SelectElements<Word>(y, z => z.Length == wordLength)).Distinct();
        }

        public static Text.Implementation.Text DeleteWordsStartingWithConsonant(Text.Implementation.Text text, int wordLength)
        {
            var newSentences = text.Sentences
                .Select(x => RemoveWordsFromSentence(x, y => y.Length == wordLength && y!.StartsWithVovel()))
                .Where(x => x.SentenceElements.OfType<IWord>().Any() && x.SentenceElements.Count > 0).ToList();

            return new Text.Implementation.Text(newSentences);
        }

        public static Sentence RemoveWordsFromSentence(ISentence sentence, Predicate<Word> predicate)
        {
            var newSentenceElements = sentence.SentenceElements.ToList();
            var matchingWords = GetMatchingElements(newSentenceElements, predicate);
            if (matchingWords.Any())
            {
                foreach (var element in matchingWords)
                {
                    var index = newSentenceElements.IndexOf(element);

                    if (index == newSentenceElements.Count - 2 && index > 0) index--;

                    newSentenceElements.Remove(element);

                    if (newSentenceElements.Count > 1) newSentenceElements.RemoveAt(index);
                }
            }

            return new Sentence(newSentenceElements);
        }

        public static IList<T> GetMatchingElements<T>(IList<ISentenceElement> sentenceElements, Predicate<T> predicate)
        {
            return sentenceElements.OfType<T>().ToList().FindAll(predicate);
        }

        public static ICollection<ISentenceElement> ReplaceWord(ISentence sentence, Predicate<IWord> predicate,
            ICollection<ISentenceElement> sentenceElements)
        {
            var newSentenceElements = sentence.SentenceElements.ToList();
            var matchingWords = GetMatchingElements(newSentenceElements, predicate);
            if (matchingWords.Any())
            {
                foreach (var element in matchingWords)
                {
                    var index = newSentenceElements.IndexOf((ISentenceElement)element);

                    newSentenceElements.Remove((ISentenceElement)element);

                    newSentenceElements.RemoveAt(index);

                    newSentenceElements.InsertRange(index, sentenceElements);
                }
            }

            return newSentenceElements.Count != 0 ? new List<ISentenceElement>(newSentenceElements) : null;
        }

        public static List<Sentence> AddSentencesToTextByIndex(IText text, int sentenceIndex,
           ICollection<Sentence> sentences)
        {
            var newTextSentences = text.Sentences.ToList();

            newTextSentences.InsertRange(sentenceIndex, sentences);

            return new List<Sentence>(newTextSentences);
        }

        public static Text.Implementation.Text ReplacesWordsInSentenceWithSubstring(IText text, int sentenceNumber, int wordLength,
            ICollection<ISentenceElement> sentenceElements)
        {
            var sentenceIndex = sentenceNumber - 1;

            var sentencesForNewText = new List<Sentence>();
            var elementsForNewSentences = new List<ISentenceElement>();
            var elementsForOneNewSentence = new List<ISentenceElement>();

            elementsForNewSentences.AddRange(ReplaceWord(text.Sentences[sentenceIndex],
                x => x.Length == wordLength, sentenceElements));

            var newSentence = new Sentence(elementsForNewSentences);

            for (int i = 0; i < sentenceIndex; i++)
            {
                sentencesForNewText.Add(text.Sentences[i]);
            }
            sentencesForNewText.Add(newSentence);
            for (int i = sentenceIndex + 1; i < text.Sentences.Count; i++)
            {
                sentencesForNewText.Add(text.Sentences[i]);
            }

            var newText = new Text.Implementation.Text(sentencesForNewText);

            return newText;
        }
    }
}
