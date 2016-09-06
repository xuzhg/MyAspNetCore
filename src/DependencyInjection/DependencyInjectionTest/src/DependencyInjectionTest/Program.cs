using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace DependencyInjectionTest
{
    public class Program
    {
        private static int num = 0;

        private int nonStatic = 0;

        public Program()
        {
            num++;
            nonStatic = 999;
        }

        public static void Main(string[] args)
        {
            IServiceCollection services = new ServiceCollection();
            services.AddSingleton<Program>();

            IServiceProvider provider = services.BuildServiceProvider();
            Program p = provider.GetRequiredService<Program>();

            Console.WriteLine("" + Program.num);
            Console.WriteLine("" + p.nonStatic);

            p = provider.GetRequiredService<Program>();
            Console.WriteLine("" + Program.num);
            Console.WriteLine("" + p.nonStatic);

            services.AddScoped<Program>();

            provider = services.BuildServiceProvider();
            p = provider.GetRequiredService<Program>();

            Console.WriteLine("" + Program.num);
            Console.WriteLine("" + p.nonStatic);

            p = provider.GetRequiredService<Program>();
            Console.WriteLine("" + Program.num);
            Console.WriteLine("" + p.nonStatic);
        }
    }
}
