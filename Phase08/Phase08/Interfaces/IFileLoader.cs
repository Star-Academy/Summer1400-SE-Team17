namespace Phase08.Interfaces
{
    public interface IFileLoader<T>
    {
        T Load(string path);
    }
}