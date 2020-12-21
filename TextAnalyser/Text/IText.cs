using System.Collections.Generic;
using TextAnalyser.Sentences;
using TextAnalyser.Sentences.Implementation;

namespace TextAnalyser.Text
{
    public interface IText
    {
        List<Sentence> Sentences { get; }
    }
}
