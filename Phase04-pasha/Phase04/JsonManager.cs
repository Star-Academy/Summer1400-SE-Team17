
using Newtonsoft.Json;
namespace Phase04
{
    public class JsonManager
    {
        public static T Deserialize<T>(string json)
        {

            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}