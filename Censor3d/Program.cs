using System;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using v10s_c3ns0r;

namespace Censor3d
{
    class Program
    {
        public static IWordDictionary WordDictionary;
        public static ICensor Censor;

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var assembly = Assembly.GetAssembly(typeof(Censor));
            const string resourceName = "v10s_c3ns0r.Data.censor-library.json";

            using var stream = assembly.GetManifestResourceStream(resourceName);
            using var reader = new StreamReader(stream);
            var result = reader.ReadToEnd();

            WordDictionary = JsonConvert.DeserializeObject<WordDictionary>(result);
            Censor = new Censor(WordDictionary);
            Console.WriteLine("Enter a phrase:");
            while (true)
            {
                Console.Write(": ");
                var x = Console.ReadLine();
                if (!Censor.HasMatch(x))
                {
                    Console.WriteLine("Nothing to do.");
                    continue;
                }
                var y = Censor.ReplaceAll(x);
                Console.WriteLine($"CENSORED> {y}");
            }
        }
    }
}
