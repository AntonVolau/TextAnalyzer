namespace TextAnalyser.SentenceElements.Implementation
{
    public class SentenceElement : ISentenceElement
    {
        public string Chars { get; set; }

        public SentenceElement(string str)
        {
            Chars = str;
        }
    }
}
