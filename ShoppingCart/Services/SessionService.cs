namespace ShoppingCart.Services;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using System.Text;

public static class SessionService
{
    // Serialize object to JSON and store it in session
    public static void SetObjectAsJson(this ISession session, string key, object value)
    {
        var json = JsonConvert.SerializeObject(value, new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore // Ignore self-referencing loops
        });
        session.SetString(key, json); // Store the JSON string in session
    }

    // Retrieve object from session and deserialize it
    public static T? GetObjectFromJson<T>(this ISession session, string key)
    {
        var json = session.GetString(key);
        return json == null ? default : JsonConvert.DeserializeObject<T>(json);
    }
}