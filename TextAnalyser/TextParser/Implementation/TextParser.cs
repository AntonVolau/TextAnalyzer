using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using TextAnalyser.SentenceElements;
using TextAnalyser.SentenceElements.Implementation;
using TextAnalyser.Sentences.Implementation;
using TextAnalyser.Sentences.SentenceElementFactory;

namespace TextAnalyser.TextParser.Implementation
{
    static class TextParser
    {
        /// <summary>
        /// Parsing strings in our input text and return Text model(List if sentences with certain parameters) 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static Text.Implementation.Text Parse(string input)
        {
            StreamReader streamReader = null; // Creating streamreader object and assign a certain value
            var sentences = new List<Sentence>(); // Initializing list of sentences
            string fileLine; // Creating string that will contain lines from our input TXT file
            string tempLine = null; // Temporary line to store unfinished sentences from the end of line to pass it to the next line
            string textSplitPattern = ConfigurationManager.AppSettings["textSplitter"]; // Declaring regular expression pattern for splitting sentences in text

            try
            {
                streamReader = new StreamReader(input);
                while ((fileLine = streamReader.ReadLine()) != null)
                {
                    fileLine = string.Concat(tempLine, fileLine); // concatenating new line with previous sentence part in case previous line wasn't ended by sentence separator
                    tempLine = null;
                    fileLine = Regex.Replace(fileLine, @"\s+", " "); // replasing all extended white spaces with single white space
                    var splitedSentences = Regex.Split(fileLine, textSplitPattern).Select(x => string.Concat(x, " ")); // splitting line by sentences using pattern
                    foreach (string sentence in splitedSentences)
                    {
                        var newSentence = Regex.Replace(sentence, @"\s+", " ");
                        if (!newSentence.Any(Char.IsLetter)) // if sentence doesn't contain any letters, we don't include it in text
                        {
                            continue;
                        }
                        else if (Separators.SentenceSeparators.Any(x => newSentence.EndsWith(x))) // if sentence ends with sentence separator we declare it as new sentence
                        {
                            var elementsForNewSentence = StringParse(newSentence);
                            sentences.Add(new Sentence(elementsForNewSentence));
                        }
                        else if (Separators.WordSeparators.Any(x => newSentence.EndsWith(x))) // if line ends with incomplete sentence, we save it to concatinate with next line later
                        {
                            tempLine = newSentence;
                        }
                    }
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("File cannot be found"); // Throw message in case invalid file name
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine("Invalid file path"); // Throw message in case invalid directory input
            }
            catch (IOException ioexception)
            {
                Console.WriteLine(ioexception.Message); // Ask mentor wich message will fit here.
            }
            catch (Exception)
            {
                Console.WriteLine("Exception occured"); // Throw regular exception message
            }
            finally
            {
                streamReader?.Dispose(); // Dispose Streamreader object after process is finished
            }
            return new Text.Implementation.Text(sentences); // return method result
        }

        /// <summary>
        /// Method to parse string and assign a type of elements (words or separators)
        /// </summary>
        /// <param name="inputLine"></param>
        /// <returns></returns>
        public static ICollection<ISentenceElement> StringParse(string inputLine)
        {
            var line = string.Concat(inputLine, " ");
            string sentenceSplitPattern = ConfigurationManager.AppSettings["sentenceSplitter"];
            var sentenceElements = new Collection<ISentenceElement>();
            var sentenceElementFactory = new SentenceElementFactory();
            foreach (Match match in Regex.Matches(line, sentenceSplitPattern)) // Splitting our input line by certain pattern and then getting a type of each individual element
            {
                sentenceElements.Add(sentenceElementFactory.GetSentenceElement(match.Groups[1].ToString())); // assign a type of sentence element

                sentenceElements.Add(sentenceElementFactory.GetSentenceElement(match.Groups[2].ToString())); // assign the type of space value after our sentence element
            } // Groups are captured subgroups due to split pattern (done with using of parentheses in pattern)
            return sentenceElements;
        }
    }
}
