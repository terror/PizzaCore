using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace PizzaCore.Helpers {
  public static class SessionHelper {
    public static string EMPLOYEE_SIGN_IN_KEY = "isEmployeeSignIn";

    // Extension methods
    public static void SetObjectAsJson(this ISession session, string key, object value) {
      session.SetString(key, JsonConvert.SerializeObject(value, Formatting.Indented, new JsonSerializerSettings {
        PreserveReferencesHandling = PreserveReferencesHandling.Objects
      }));
    }

    public static T GetObjectFromJson<T>(this ISession session, string key) {
      var value = session.GetString(key);
      return value == null ? default : JsonConvert.DeserializeObject<T>(value);
    }
  }
}
