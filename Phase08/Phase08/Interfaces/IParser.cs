namespace Phase08.Interfaces
{
    public interface IParser<T>
    {
        T Parse(string text);
    }
}