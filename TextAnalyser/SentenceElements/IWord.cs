namespace TextAnalyser.SentenceElements
{
    public interface IWord
    {
        int Length { get; }

        bool StartsWithVovel();
    }
}
