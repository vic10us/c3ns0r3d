namespace v10s_c3ns0r
{
    public interface IWordDictionary
    {
        string[] WhiteList { get; set; }
        string[] BlackList { get; set; }
        string[] Replacements { get; set; }
    }
}