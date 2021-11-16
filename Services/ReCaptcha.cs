using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace PizzaCore.Services {
  public class ReCaptcha {
    private readonly HttpClient captchaClient;
    public ReCaptchaOptions Options { get; set; }

    public ReCaptcha(HttpClient captchaClient, IOptions<ReCaptchaOptions> options) {
      this.captchaClient = captchaClient;
      this.Options = options.Value;
    }

    public async Task<bool> IsValid(string captcha) {
      try {
        var postTask = await captchaClient
            .PostAsync($"?secret={Options.ApiKey}&response={captcha}", new StringContent(""));
        var result = await postTask.Content.ReadAsStringAsync();
        var resultObject = JObject.Parse(result);
        dynamic success = resultObject["success"];
        return (bool)success;
      }
      catch (Exception e) {
        return false;
      }
    }
  }
}
