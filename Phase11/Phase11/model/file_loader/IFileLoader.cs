namespace Phase11
{
    public interface IFileLoader<T>
    {
        T Load(string path);
    }
}