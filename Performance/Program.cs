using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Performance
{
    class Program
    {
        static List<Thread> CreateTheads(int count)
        {
            var l = new List<Thread>();
            Parallel.For(0, count, i =>
              {
                  l.Add(new Thread(() =>
                  {
                      try
                      {
                          var api = new Api();
                          var r = api.Post();
                          r.Wait();
                      }
                      catch (Exception ex)
                      {
                          Console.WriteLine(ex.Message);
                      }
                  }));
              });
            return l;
        }
        static void Run(List<Thread> l = null)
        {
            Parallel.For(0, 10000, i =>
            {
                try
                {
                    var api = new Api();
                    var r = api.Post();
                    r.Wait();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            });
            Counter.Count = 0;
            //Console.WriteLine("Reset");
            //Run(l);
        }
        static void Run()
        {
            using (var api = new Api())
            {
                var acc = api.Register();
                acc.Wait();
                var r = api.Login(acc.Result);
                r.Wait();
                for (int j = 0; j < 1000; j++)
                {
                    try
                    {
                        r = api.Payment();
                        r.Wait();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }

        }
        static void Main(string[] args)
        {
            var l = new List<Thread>();
            for (int i = 0; i < 1; i++)
            {
                var t = new Thread(() =>
                {
                    Run();
                });
                t.Start();
                l.Add(t);
            }
            Parallel.ForEach(l, item =>
            {
                item.Join();
            });
            Console.WriteLine("done!");
            Process.Start(Assembly.GetExecutingAssembly().Location);
        }
    }
}
