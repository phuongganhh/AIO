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
        public string Msg { get; set; }
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
                    var result = Http.PostAsync("/Login/index", data);
                    result.Wait();

                    var content = result.Result.Content.ReadAsStringAsync();
                    content.Wait();
                    Console.Title = dic["username"];
                    return content.Result.Contains("dkhp.cntp.edu.vn");
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        private static string Register(this string link)
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
                    return data.Msg;
                }
                return null;
            }
            return null;
        }
        private static HttpClient Http { get; set; }
        private static DateTime Timer { get; set; }
        static void Main(string[] args)
        {
            if(args.Length > 0)
            {
                var process = new Process();
                process.StartInfo.FileName = Assembly.GetExecutingAssembly().Location;
                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                process.Start();
                Environment.Exit(1);
            }
            try
            {
                Console.OutputEncoding = Encoding.UTF8;
                Http = new HttpClient();
                Http.BaseAddress = new Uri("https://dkhp.hcmue.edu.vn");
                var isLogin = Login();
                Console.Write("Login\t");
                if (isLogin)
                {
                    Console.WriteLine("[OK]");
                    var links = File.ReadAllLines("hcmue.link").ToList();
                    Timer = DateTime.Now;
                    while ((DateTime.Now - Timer).TotalMinutes <= 1)
                    {
                        foreach (var link in links)
                        {
                            var result = link.Register();
                            Console.Write(link);
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("\t" + result);
                            Console.ResetColor();
                        }
                    }
                    var process = new Process();
                    process.StartInfo.FileName = Assembly.GetExecutingAssembly().Location;
                    process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    process.Start();
                    Environment.Exit(1);
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
