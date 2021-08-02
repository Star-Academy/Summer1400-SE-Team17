namespace Phase04_sia
{
    public interface IReader<T,K>
    {
        T Read(K filePath);
    }

    public interface IWriter<T>
    {
        void Write(T data);
    }
}