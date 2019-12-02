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
                          api.Post();
                      }
                      catch (Exception ex)
                      {
                          Console.WriteLine(ex.Message);
                      }
                  }));
              });
            return l;
        }
        static void Run(List<Thread> l)
        {
            if(l == null && l.Count == 0)
            {
                return;
            }
            l.ForEach(item =>
            {
                item.Start();
            });
            while (Counter.Count != l.Count)
            {

            }
            Counter.Count = 0;
            Console.WriteLine("Reset");
            Run(l);
        }
        static void Main(string[] args)
        {
            if(args != null && args.Length > 0)
            {
                Process p = new Process();
                p.StartInfo.FileName = Assembly.GetExecutingAssembly().Location;
                p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                p.Start();
                Console.WriteLine("OK");
                Console.ReadKey();
                Environment.Exit(1);
            }
            var count = 100;
            Run(CreateTheads(count));
            Console.WriteLine("WATING...");
            Console.ReadLine();
        }
    }
}
