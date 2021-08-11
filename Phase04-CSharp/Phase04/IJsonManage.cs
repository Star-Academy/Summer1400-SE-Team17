namespace Phase04
{
    public interface IJsonManage<T>
    {
        public T Deserialize(string json);
    }
}