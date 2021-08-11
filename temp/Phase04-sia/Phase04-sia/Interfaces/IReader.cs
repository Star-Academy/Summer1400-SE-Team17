namespace Phase04_sia
{
    public interface IReader<T,K>
    {
        T Read(K filePath);
    }
    
}