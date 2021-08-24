namespace Phase11.model.parser
{
    public interface IParser<T>
    {
        T Parse(string text);
    }
}