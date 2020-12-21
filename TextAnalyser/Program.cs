using System;
using System.IO;
using TextAnalyser.Enums;
using TextAnalyser.SentenceElements.Implementation;
using TextAnalyser.TextFomatting.Implementation;

namespace TextAnalyser
{
    class Program
    {
        private static void Main()
        {
            var text = TextParser.Implementation.TextParser.Parse(Path.Combine(Environment.CurrentDirectory, "TextFiles/TestSentence.txt"));

            Console.WriteLine("_______________________ Initial Text _______________________");
            Console.WriteLine(text);
            Console.WriteLine("_______________________ Task1 _______________________");
            Console.WriteLine("Task 1: Print all sentences ordered by count of words ascending");

            var sortedText = TextFormat.SortSentencesAscending<Word>(text);

            foreach (var sentence in sortedText)
            {
                Console.WriteLine(sentence);
            }

            Console.WriteLine("_______________________ Task2 _______________________");
            Console.WriteLine("Task 2: In every interrogative sentence print all words with sertain number of characters");
            Console.WriteLine("Please, enter length of words for second task");
            int secondTaskWordLength;
            startTask2:
            try
            {
                secondTaskWordLength = Int32.Parse(Console.ReadLine());
            }
            catch
            {
                Console.WriteLine("Invalid input, try again");
                goto startTask2;
            }
            var secondTaskWords = TextFormat.GetWordsFromSentences(text, SentenceType.Interrogative, secondTaskWordLength);

            foreach (var word in secondTaskWords)
            {
                Console.WriteLine(word);
            }

            Console.WriteLine("_______________________ Task3 _______________________");
            Console.WriteLine("Task 3: Delete all words of sertain length starting with consonant from text");
            Console.WriteLine("Please, enter length of words for third task");
            int thirdTaskWordLength;
            startTask3:
            try
            {
                thirdTaskWordLength = Int32.Parse(Console.ReadLine());
            }
            catch
            {
                Console.WriteLine("Invalid input, try again");
                goto startTask3;
            }
            var textWithDeletedWords = TextFormat.DeleteWordsStartingWithConsonant(text, thirdTaskWordLength);

            foreach (var sentence in textWithDeletedWords.Sentences)
            {
                Console.WriteLine(sentence);
            }

            Console.WriteLine("_______________________ Task4 _______________________");
            Console.WriteLine("Task 4: In certain sentence replace all words of given length with substring");
            Console.WriteLine("Please, enter number of sentece");
            int fourthTaskWordLength;
            string substringForReplacement;
            int fourthTaskSentenceNumber;
            startTask4:
            try
            {
                fourthTaskSentenceNumber = Int32.Parse(Console.ReadLine());
                Console.WriteLine("Please, enter length of words for fourth task");
                fourthTaskWordLength = Int32.Parse(Console.ReadLine());
                Console.WriteLine("Please, enter length of words for fourth task");
                substringForReplacement = Console.ReadLine();
            }
            catch
            {
                Console.WriteLine("Invalid input, try again");
                goto startTask4;
            }

            text = TextFormat.ReplacesWordsInSentenceWithSubstring(text, fourthTaskSentenceNumber,
                fourthTaskWordLength, TextParser.Implementation.TextParser.StringParse(substringForReplacement));

            foreach (var sentence in text.Sentences)
            {
                Console.WriteLine(sentence);
            }

            Console.ReadLine();
        }
    }
}
