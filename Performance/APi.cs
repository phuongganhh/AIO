using IronOcr;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Performance
{
    public class Api
    {
        public Api()
        {
            var cookieContainer = new CookieContainer();
            var handler = new HttpClientHandler();
            handler.CookieContainer = cookieContainer;
            this.client   = new HttpClient(handler);
            this.client.Timeout = TimeSpan.FromSeconds(10);
        }
        private async Task<string> GetCaptcha()
        {
            var result = await this.client.GetAsync("http://id.volamtongkim.com/captcha/captcha-image.php?rand=623118468");
            var stream = await result.Content.ReadAsStreamAsync();
            string code = string.Empty;
            while(code == string.Empty)
            {
                try
                {
                    var ocr = new AutoOcr();
                    var r = ocr.Read(Image.FromStream(stream));
                    code = r.Text;
                }
                catch (Exception)
                {

                }
            }
            if (code.Length != 3)
            {
                return await this.GetCaptcha();
            }
            return code;
        }
        private HttpClient client { get; set; }
        private async Task Run()
        {
            try
            {
                var dic = new Dictionary<string, string>()
                {
                    ["username"] = Guid.NewGuid().ToString().Split('-').LastOrDefault(),
                    ["password"] = Guid.NewGuid().ToString().Split('-').LastOrDefault(),
                    ["email"] = Guid.NewGuid().ToString().Split('-').LastOrDefault() + "@gmail.com",
                    ["phone"] = Guid.NewGuid().ToString().Split('-').LastOrDefault(),
                };
                var code = await this.GetCaptcha();
                dic["captcha"] = code;
                var data = new FormUrlEncodedContent(dic);
                var result = await this.client.PostAsync("http://id.volamtongkim.com/register.html", data);
                // Console.WriteLine($"{Counter.Count++} {dic["username"]} - {dic["password"]}");

            }
            catch (Exception ex)
            {
                Console.WriteLine(Thread.CurrentThread.ManagedThreadId + " - " + ex.Message);
                await Run();
            }
        }
        public async Task Post()
        {
            await Run();
        }
    }
}
