using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using TextAnalyser.SentenceElements;
using TextAnalyser.SentenceElements.Implementation;
using TextAnalyser.Sentences.Implementation;
using TextAnalyser.Sentences.SentenceElementFactory;
using TextAnalyser.TextFomatting.Implementation;

namespace TextAnalyser.TextParser.Implementation
{
    static class TextParser
    {
        /// <summary>
        /// Parsing strings in our input text and return Text model(List if sentences with sertain parameters) 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static Text.Implementation.Text Parse(string input)
        {
            StreamReader streamReader = null; // Creating streamreader object and assign a sertain value
            var sentences = new List<Sentence>(); // Initializing list of sentences
            string fileLine; // Creating string that will contain lines from our input TXT file
            string tempLine = null; // Temporary line to store unfinished sentences from the end of line to pass it to the next line

            try
            {
                streamReader = new StreamReader(input);
                while ((fileLine = streamReader.ReadLine()) != null)
                {
                    fileLine = string.Concat(tempLine, fileLine);
                    tempLine = null;
                    fileLine = Regex.Replace(fileLine, @"\s+", " ");
                    var sent = Regex.Split(fileLine, @"(?<=[\.!?])").Select(x => string.Concat(x, " "));
                    foreach (string senten in sent)
                    {
                        var newSent = Regex.Replace(senten, @"\s+", " ");
                        if (!newSent.Any(Char.IsLetter))
                        {
                            continue;
                        }
                        else if (Separators.SentenceSeparators.Any(x => newSent.EndsWith(x)))
                        {
                            var elementsForNewSentence = StringParse(newSent);
                            sentences.Add(new Sentence(elementsForNewSentence));
                        }
                        else if (Separators.WordSeparators.Any(x => newSent.EndsWith(x)))
                        {
                            tempLine = newSent;
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

        public static ICollection<ISentenceElement> StringParse(string inputLine)
        {
            var line = string.Concat(inputLine, " ");

            var sentenceElements = new Collection<ISentenceElement>();
            var sentenceElementFactory = new SentenceElementFactory();
            foreach (Match match in Regex.Matches(line, @"\b(\w+)((\p{P}{0,3})\s?)"))
            {
                sentenceElements.Add(sentenceElementFactory.GetSentenceElement(match.Groups[1].ToString()));

                sentenceElements.Add(sentenceElementFactory.GetSentenceElement(match.Groups[2].ToString()));
            }
            return sentenceElements;
        }
    }
}
