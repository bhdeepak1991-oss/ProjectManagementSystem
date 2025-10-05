using System.Text.Json;

namespace PMS.Features.UserManagement.Services
{
    public static class SessionService
    {
        public static void SetObject<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        /// <summary>
        /// Get an object from session
        /// </summary>
        public static T? GetObject<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonSerializer.Deserialize<T>(value);
        }

        /// <summary>
        /// Remove object from session
        /// </summary>
        public static void Remove(this ISession session, string key)
        {
            session.Remove(key);
        }
    }
}
