using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Assignment3.Services
{
  public class ReCaptcha
  {
    private readonly HttpClient captchaClient;

    public ReCaptcha(HttpClient captchaClient)
    {
      this.captchaClient = captchaClient;
    }

    public async Task<bool> IsValid(string captcha)
    {
      try
      {
        var postTask = await captchaClient
            .PostAsync($"?secret=6LdIiBQdAAAAAMpJfIqgaNTdcoVMzSwkc_PEfhHL&response={captcha}", new StringContent(""));
        var result = await postTask.Content.ReadAsStringAsync();
        var resultObject = JObject.Parse(result);
        dynamic success = resultObject["success"];
        return (bool)success;
      }
      catch (Exception e)
      {
        return false;
      }
    }
  }
}
