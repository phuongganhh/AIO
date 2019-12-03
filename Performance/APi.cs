using IronOcr;
using Newtonsoft.Json;
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
    public class Api : IDisposable
    {
        public Api()
        {
            var cookieContainer = new CookieContainer();
            var handler = new HttpClientHandler();
            handler.CookieContainer = cookieContainer;
            this.client   = new HttpClient(handler);
            this.client.DefaultRequestHeaders.Add("Referer", "http://id.volamtongkim.com/payment");
            //this.client.Timeout = TimeSpan.FromSeconds(10);
        }
        private async Task<string> GetCaptcha()
        {
            var result = await this.client.GetAsync("http://id.volamtongkim.com/captcha/captcha-image.php?rand=1733386359");
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
            if (code.Length != 5)
            {
                return await this.GetCaptcha();
            }
            return code;
        }
        private HttpClient client { get; set; }
        private static Random Rand { get; set; } = new Random();
        private string cmnd
        {
            get
            {
                var s = "";
                for (int i = 0; i < 9; i++)
                {
                    s += Rand.Next(0, 9);
                }
                return s;
            }
        }
        private string token
        {
            get
            {
                return Guid.NewGuid().ToString().Split('-').LastOrDefault();
            }
        }
        private string sdt
        {
            get
            {
                var s = "0";
                for (int i = 0; i < 10; i++)
                {
                    s += Rand.Next(0, 9);
                }
                return s;
            }
        }
        public async Task<Dictionary<string,string>> Register()
        {
            var dic = new Dictionary<string, string>()
            {
                ["username"] = this.token,
                ["password"] = this.token,
                ["email"] = this.token + "@gmail.com",
                ["phone"] = this.sdt,
                ["captcha"] = await this.GetCaptcha(),
            };
            var data = new FormUrlEncodedContent(dic);
            var result = await this.client.PostAsync("http://id.volamtongkim.com/register.html", data);
            var content = await result.Content.ReadAsStringAsync();
            var r =  JsonConvert.DeserializeObject<Result>(content);
            if(r.status == 0)
            {
                Console.WriteLine($"{dic["username"]} - {dic["password"]}");
                return dic;
            }
            return await this.Register();
        }
        private async Task Run()
        {
            try
            {
                var pass = Guid.NewGuid().ToString().Split('-').LastOrDefault();
                var dic = new Dictionary<string, string>()
                {
                    ["txtusername"] = Guid.NewGuid().ToString().Split('-').LastOrDefault(),
                    ["txtpassword"] = pass,
                    ["txtrepassword"] = pass,
                    ["txtemail"] = Guid.NewGuid().ToString().Split('-').LastOrDefault() + "@gmail.com",
                    ["txtcmnd"] = this.cmnd,
                    ["txtphone"] = this.sdt,
                    ["txthoten"] = Guid.NewGuid().ToString().Split('-').LastOrDefault(),
                    ["txtcauhoi"] = "convat",
                    ["txttraloi"] = Guid.NewGuid().ToString().Split('-').LastOrDefault(),
                };
                var code = await this.GetCaptcha();
                dic["txtcode"] = code;
                var data = new FormUrlEncodedContent(dic);
                var result = await this.client.PostAsync("http://id.vl2.vn/form/register.php", data);
                var content = await result.Content.ReadAsStringAsync();
                Console.WriteLine($"{Counter.Count++} {content}");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await Run();
            }
        }
        public async Task Post()
        {
            await Run();
        }
        public async Task Login(Dictionary<string,string> acc)
        {
            //Console.WriteLine("Login...");
            acc["captcha"] = await this.GetCaptcha();
            var data = new FormUrlEncodedContent(acc);
            var result = await client.PostAsync("http://id.volamtongkim.com/login.html", data);
            var content = await result.Content.ReadAsStringAsync();
            var r = JsonConvert.DeserializeObject<Result>(content);
            if(r.status == 1)
            {
                await Login(acc);
            }
        }
        private bool TypeCard
        {
            get
            {
                return 1 == 1;
                //return Rand.Next(1, 2) % 2 == 0;
            }
        }
        private string Price
        {
            get
            {
                var l = new List<int>()
                {
                    50000,100000,200000,300000,500000
                };
                return l[Rand.Next(0, l.Count)].ToString();
            }
        }
        private string Gen(int number = 0)
        {
            var s = "";
            for (int i = 0; i < number; i++)
            {
                s += Rand.Next(0, 9);
            }
            return s;
        }
        private async Task<Dictionary<string, string>> Vina()
        {
            var captcha = await this.GetCaptcha();
            return new Dictionary<string, string>()
            {
                ["cardtype"] = "vinaphone",
                ["serial"] = this.Gen(15),
                ["pin"] = this.Gen(12),
                ["priceguest"] = this.Price,
                ["captcha"] = captcha,
            };
        }
        private async Task<Dictionary<string, string>> Gate()
        {
            var captcha = await this.GetCaptcha();
            return new Dictionary<string, string>()
            {
                ["cardtype"] = "gate",
                ["serial"] = "CK" + this.Gen(8),
                ["pin"] = this.Gen(10),
                ["priceguest"] = this.Price,
                ["captcha"] = captcha,
            };
        }
        public async Task Payment()
        {
            Dictionary<string, string> dic;
            if (TypeCard)
            {
                dic =  await this.Vina();
            }
            else
            {
                dic = await this.Gate();
            }
            var data = new FormUrlEncodedContent(dic);
            var result = await this.client.PostAsync("http://id.volamtongkim.com/payment.html", data);
            var content = await result.Content.ReadAsStringAsync();
            var r = JsonConvert.DeserializeObject<Result>(content);
            if(r != null && r.status == 0)
            {
                Console.Title = Counter.Count++.ToString();
            }
        }
        public async Task Check()
        {
            var result = await this.client.PostAsync("http://id.volamtongkim.com/loadinfo.html",null);
            var content = await result.Content.ReadAsStringAsync();
        }

        public void Dispose()
        {
            GC.Collect();
        }
    }
}
public class Result
{
    public int status { get; set; }
}