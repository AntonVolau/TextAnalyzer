using System;
using System.Collections.Generic;
using System.Text;

namespace TextAnalyser.SentenceElements
{
    interface ISeparator
    {
        bool IsSpaceMark();
        bool IsWordSeparator();
        bool IsSentenceSeparator();
        bool IsQuestionMark();
    }
}
