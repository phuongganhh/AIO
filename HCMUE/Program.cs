using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HCMUE
{
    public class Result
    {
        public int Status { get; set; }
    }
    static class Program
    {
        private static bool Login()
        {
            try
            {
                if (Http != null)
                {
                    var dic = new Dictionary<string, string>
                    {
                        ["username"] = "41.01.104.023",
                        ["password"] = "mupiu123"
                    };
                    var data = new FormUrlEncodedContent(dic);
                    var result = Http.PostAsync("/Login", data);
                    result.Wait();
                    if (result.Result.IsSuccessStatusCode)
                    {
                        var content = result.Result.Content.ReadAsStringAsync();
                        content.Wait();
                        Console.Title = dic["username"];
                        return !content.Result.Contains("Đăng nhập");
                    }
                    return false;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        private static bool Register(this string link)
        {
            if(Http != null)
            {
                var result = Http.PostAsync(link,null);
                result.Wait();
                if (result.Result.IsSuccessStatusCode)
                {
                    var content = result.Result.Content.ReadAsStringAsync();
                    content.Wait();
                    var data = JsonConvert.DeserializeObject<Result>(content.Result);
                    return data.Status == 1;
                }
                return false;
            }
            return false;
        }
        private static HttpClient Http { get; set; }
        private static DateTime Timer { get; set; }
        static void Main(string[] args)
        {
            try
            {
                Http = new HttpClient();
                Http.BaseAddress = new Uri("https://dkhp.hcmue.edu.vn");
                var isLogin = Login();
                Console.Write("Login\t");
                if (isLogin)
                {
                    Console.WriteLine("[OK]");
                    var links = File.ReadAllLines("hcmue.link").ToList();
                    Timer = DateTime.Now;
                    while ((DateTime.Now - Timer).TotalMinutes <= 10)
                    {
                        foreach (var link in links)
                        {
                            var result = link.Register();
                            Console.Write(link);
                            if (result)
                            {
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.WriteLine("\t[OK]");
                            }
                            else
                            {
                                Console.ResetColor();
                                Console.WriteLine("\t[FAILED]");
                            }
                        }
                    }
                    Process.Start(Assembly.GetExecutingAssembly().Location);
                }
                else
                {
                    Console.WriteLine("[FAILED]");
                    Console.ReadLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
