using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace PizzaCore.Services {
  public class ReCaptcha {
    private readonly HttpClient captchaClient;
    public GoogleServicesOptions options { get; set; }

    public ReCaptcha(HttpClient captchaClient, IOptions<GoogleServicesOptions> options) {
      this.captchaClient = captchaClient;
      this.options = options.Value;
    }

    public async Task<bool> IsValid(string captcha) {
      try {
        var postTask = await captchaClient.PostAsync($"?secret={options.ReCaptchaApiKey}&response={captcha}", new StringContent(""));
        var result = await postTask.Content.ReadAsStringAsync();
        var resultObject = JObject.Parse(result);
        dynamic success = resultObject["success"];
        return (bool)success;
      } catch {
        return false;
      }
    }
  }
}
