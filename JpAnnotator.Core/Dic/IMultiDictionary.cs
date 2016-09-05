namespace JpAnnotator.Core.Dic
{
    public interface IMultiDictionary
    {
        void Append(string word, string translation);
        string LookupTranslation(string word);
    }
}
