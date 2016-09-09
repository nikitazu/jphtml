namespace JpAnnotator.Common.Portable.Configuration
{
    public interface IOptionProviderEpub
    {
        string Author { get; }
        string BookId { get; }
        string Publisher { get; }
        string OutputFile { get; }
    }
}

