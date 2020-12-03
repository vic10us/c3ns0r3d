namespace v10s_c3ns0r
{
    public interface ICensor
    {
        bool HasMatch(string phrase);
        string ReplaceAll(string phrase);
    }
}