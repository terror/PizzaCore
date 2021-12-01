using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaCore.Helpers
{
  public static class SessionHelper
  {
    // Extension methods
    public static void SetObjectAsJson(this ISession session, string key, object value)
    {
      session.SetString(key, JsonConvert.SerializeObject(value, Formatting.Indented, new JsonSerializerSettings
      {
        PreserveReferencesHandling = PreserveReferencesHandling.Objects
      }));
    }

    public static T GetObjectFromJson<T>(this ISession session, string key)
    {
      var value = session.GetString(key);
      return value == null ? default : JsonConvert.DeserializeObject<T>(value);
    }
  }
}
