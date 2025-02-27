using Newtonsoft.Json;

namespace Api.Models
{
    public abstract class IParameters
    {
        public string ToString() => JsonConvert.SerializeObject(this, new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore,
            Formatting = Formatting.None
        });
    }
}