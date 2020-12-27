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
        /// <summary>
        /// Method to select sentence elements that satisfy certain condition
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sentence"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static ICollection<T> SelectElements<T>(ISentence sentence, Func<T, bool> selector = null)
            where T : SentenceElement
        {
            return selector == null
                ? sentence.SentenceElements.OfType<T>().ToList()
                : sentence.SentenceElements.OfType<T>().Where(selector).ToList();
        }
        /// <summary>
        /// Method to select sentences that satisfy certain condition
        /// </summary>
        /// <param name="text"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static ICollection<Sentence> SelectSentences(IText text, Func<Sentence, bool> selector = null)
        {
            return selector == null ? text.Sentences : text.Sentences.Where(selector).ToList();
        }
        /// <summary>
        /// Method to sort sentences in text by number of words ascending
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="text"></param>
        /// <returns></returns>
        public static IOrderedEnumerable<ISentence> SortSentencesByWordsCountAscending<T>(IText text) where T : Word
        {
            return text.Sentences.OrderBy(x => SelectElements<T>(x).Count);
        }

        public static IEnumerable<Word> GetWordsFromSentencesofCertainType(IText text, SentenceType sentenceType, int wordLength)
        {
            return SelectSentences(text, x => x.SentenceTypes.Contains(sentenceType))
                .SelectMany(y => SelectElements<Word>(y, z => z.Length == wordLength)).Distinct();
        }
        /// <summary>
        /// Method to delete words starting with consonant from sentence
        /// </summary>
        /// <param name="text"></param>
        /// <param name="wordLength"></param>
        /// <returns></returns>
        public static Text.Implementation.Text DeleteWordsStartingWithConsonant(Text.Implementation.Text text, int wordLength)
        {
            var newSentences = text.Sentences
                .Select(x => RemoveWordsFromSentence(x, y => y.Length == wordLength && !y.StartsWithVovel()))
                .Where(x => x.SentenceElements.OfType<IWord>().Any() && x.SentenceElements.Count > 0).ToList();

            return new Text.Implementation.Text(newSentences);
        }
        /// <summary>
        /// Method to remove some words from sentence
        /// </summary>
        /// <param name="sentence"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Method to get a list of sentence elements of certain type 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sentenceElements"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static IList<T> GetMatchingElements<T>(IList<ISentenceElement> sentenceElements, Predicate<T> predicate)
        {
            return sentenceElements.OfType<T>().ToList().FindAll(predicate);
        }
        /// <summary>
        /// Method to replace some words with other words of different length
        /// Predicate checks if word have correct length
        /// </summary>
        /// <param name="sentence"></param>
        /// <param name="predicate"></param>
        /// <param name="sentenceElements"></param>
        /// <returns></returns>
        public static ICollection<ISentenceElement> ReplaceWord(ISentence sentence, Predicate<IWord> predicate, ICollection<ISentenceElement> sentenceElements)
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

        public static List<Sentence> AddSentencesToTextByIndex(IText text, int sentenceIndex, ICollection<Sentence> sentences)
        {
            var newTextSentences = text.Sentences.ToList();

            newTextSentences.InsertRange(sentenceIndex, sentences);

            return new List<Sentence>(newTextSentences);
        }
        /// <summary>
        /// Method to replace words of certain length in choosen centence by substring of any length
        /// </summary>
        /// <param name="text"></param>
        /// <param name="sentenceNumber"></param>
        /// <param name="wordLength"></param>
        /// <param name="sentenceElements"></param>
        /// <returns></returns>
        public static Text.Implementation.Text ReplacesWordsInSentenceWithSubstring(IText text, int sentenceNumber, int wordLength, ICollection<ISentenceElement> sentenceElements)
        {
            var sentenceIndex = sentenceNumber - 1;

            var sentencesForNewText = new List<Sentence>();
            var elementsForNewSentences = new List<ISentenceElement>();

            elementsForNewSentences.AddRange(ReplaceWord(text.Sentences[sentenceIndex], x => x.Length == wordLength, sentenceElements));

            var newSentence = new Sentence(elementsForNewSentences);

            for (int i = 0; i < sentenceIndex; i++) // Adding sentences before changed sentence
            {
                sentencesForNewText.Add(text.Sentences[i]);
            }
            sentencesForNewText.Add(newSentence); // Adding changed sentence 
            for (int i = sentenceIndex + 1; i < text.Sentences.Count; i++) // Adding sentences after changed sentence
            {
                sentencesForNewText.Add(text.Sentences[i]);
            }

            var newText = new Text.Implementation.Text(sentencesForNewText);

            return newText;
        }
    }
}
