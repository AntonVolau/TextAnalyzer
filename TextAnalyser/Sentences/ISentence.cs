using System.Collections.Generic;
using TextAnalyser.Enums;
using TextAnalyser.SentenceElements;

namespace TextAnalyser.Sentences
{
    public interface ISentence
    {
        ICollection<ISentenceElement> SentenceElements { get; set; }

        IEnumerable<SentenceType> SentenceTypes { get; }
    }
}
