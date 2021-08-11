using Newtonsoft.Json;

namespace Phase04
{
    public class JsonManager<T> : IJsonManage<T>
    {
        public T Deserialize(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}