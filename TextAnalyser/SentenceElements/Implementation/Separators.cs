namespace TextAnalyser.SentenceElements.Implementation
{
    /// <summary>
    /// Possible types of separators in sentence
    /// </summary>
    static class Separators
    {
        public static string Space { get; } = " ";

        public static string[] WordSeparators { get; } = { ", ", "; ", ": " };

        public static string[] SentenceSeparators { get; } = { "... ", "! ", ". ", "? " };

        public static string[] AllSentenceSeparators { get; } = { 
            ", ", ". ", "! ", "? ", "— ", "- ", "' ", "( ", ") ",
            "< ", "> ", ": ", "; ", "“ ", "« ", "» ", "‘ ", "’ ", "... ",
            "* ", "/ ", "= ", "== ", "!= ", "+ ", " "
        };
    }
}
