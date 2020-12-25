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
                    fileLine = string.Concat(tempLine, fileLine);
                    tempLine = null;
                    fileLine = Regex.Replace(fileLine, @"\s+", " ");
                    var splitedSentences = Regex.Split(fileLine, textSplitPattern).Select(x => string.Concat(x, " "));
                    foreach (string sentence in splitedSentences)
                    {
                        var newSentence = Regex.Replace(sentence, @"\s+", " ");
                        if (!newSentence.Any(Char.IsLetter))
                        {
                            continue;
                        }
                        else if (Separators.SentenceSeparators.Any(x => newSentence.EndsWith(x)))
                        {
                            var elementsForNewSentence = StringParse(newSentence);
                            sentences.Add(new Sentence(elementsForNewSentence));
                        }
                        else if (Separators.WordSeparators.Any(x => newSentence.EndsWith(x)))
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
        /// Method to parse string and separate it's elements by words and separators
        /// </summary>
        /// <param name="inputLine"></param>
        /// <returns></returns>
        public static ICollection<ISentenceElement> StringParse(string inputLine)
        {
            var line = string.Concat(inputLine, " ");
            string sentenceSplitPattern = ConfigurationManager.AppSettings["sentenceSplitter"];
            var sentenceElements = new Collection<ISentenceElement>();
            var sentenceElementFactory = new SentenceElementFactory();
            foreach (Match match in Regex.Matches(line, sentenceSplitPattern))
            {
                sentenceElements.Add(sentenceElementFactory.GetSentenceElement(match.Groups[1].ToString()));

                sentenceElements.Add(sentenceElementFactory.GetSentenceElement(match.Groups[2].ToString()));
            }
            return sentenceElements;
        }
    }
}
