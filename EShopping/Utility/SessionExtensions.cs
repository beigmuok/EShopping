using EShopping.Services;
using Newtonsoft.Json;

namespace EShopping.Utility
{
    public static class SessionExtensions
    {

        public static void Set<T>(this ISession session, String Key, T value)
        {
            session.SetString(Key, JsonConvert.SerializeObject(value));
        }


        public static T Get<T>(this ISession session, string Key)
        {
            string value = session.GetString(Key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
		

	}
}
