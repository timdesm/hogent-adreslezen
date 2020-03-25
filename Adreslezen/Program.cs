using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Adreslezen
{
   
    class Program
    {
        public static int loadingTime = 0;

        public static Dictionary<int, AdresLocatie> locatie = new Dictionary<int, AdresLocatie>();
        public static Dictionary<String, Gemeente> gemeentes = new Dictionary<string, Gemeente>();
        public static Dictionary<int, Straatnaam> streets = new Dictionary<int, Straatnaam>();


        public static List<int> addIds = new List<int>();
        public static Dictionary<int, Adres> addresses = new Dictionary<int, Adres>();

        public static List<List<String>> cache = new List<List<String>>(); 

        public static int count = 0;

        static void Main(string[] args)
        {
            // Unzip File
            // FileUtil.UnZip(@"C:\Users\timde\source\repos\Adreslezen\Adreslezen\Data\CRAB_Adressenlijst_GML.zip", @"C:\Users\timde\source\repos\Adreslezen\Adreslezen\Data\Extract");

            // Run the progress thread
            Console.WriteLine("--------------------------");
            Console.WriteLine("Project created by Tim De Smet");
            Console.WriteLine("HoGent Adreslezen");
            Console.WriteLine("--------------------------");
            Console.WriteLine(" ");
            Console.WriteLine("Loading address data...");
            Console.WriteLine(" ");

            var t = Task.Run(() => ProgressThread());
            // var tt = Task.Run(() => BuildCache());

            var watch = System.Diagnostics.Stopwatch.StartNew();

            using (FileStream fs = File.Open(@"C:\Users\timde\source\repos\Data\Extract\GML\CrabAdr.gml", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (BufferedStream bs = new BufferedStream(fs))
            using (StreamReader sr = new StreamReader(bs))
            {
                bool reading = false;
                List<string> obj = new List<string>();

                String line;
                while((line = sr.ReadLine()) != null)
                {
                    // Start logging
                    if(!reading && line.Contains("agiv:CrabAdr"))
                    {
                        obj.Clear();
                        reading = true;
                    }

                    // Add line
                    if(reading)
                    {
                        obj.Add(line);
                    }

                    // Stop logging and build object
                    if (reading && line.Contains("/agiv:CrabAdr"))
                    {
                        reading = false;
                        buildObject(obj);
                    }
                    ++count;
                }
            }

            watch.Stop();
            t.Wait();

            // Search
            while (true)
            {
                Console.Clear();
                Console.WriteLine("[1] Search on City");
                Console.WriteLine("[2] Search on Street");
                Console.WriteLine("[3] City information");
                Console.WriteLine("[4] Stats");
                Console.Write("Search type: ");
                String searchType = Console.ReadLine();

                if (searchType.Equals("1"))
                {
                    Console.Clear();
                    Console.Write("Input a city name: ");
                    String arg1 = Console.ReadLine();
                    if (gemeentes.ContainsKey(arg1))
                    {
                        Console.Clear();
                        Console.WriteLine("--------------------------");
                        Console.WriteLine("(" + arg1 + ") - Found streets:");
                        Console.WriteLine("--------------------------");
                        foreach (Straatnaam straat in streets.Values)
                        {
                            if (straat.Gemeente.Equals(gemeentes[arg1]))
                            {
                                Console.WriteLine(straat.Streetname);
                            }
                        }
                        Console.WriteLine("--------------------------");
                        Console.WriteLine("Press ENTER to continue...");
                        Console.ReadLine();
                    }
                }
                if (searchType.Equals("2"))
                {
                    Console.Clear();
                    Console.Write("Input a streetname: ");
                    String arg1 = Console.ReadLine();
                    List<Gemeente> gemeentes = new List<Gemeente>();
                    foreach(Straatnaam straat in streets.Values)
                    {
                        if(straat.Streetname.Equals(arg1))
                        {
                            if(!gemeentes.Contains(straat.Gemeente))
                            {
                                gemeentes.Add(straat.Gemeente);
                            }
                        }
                    }
                    Console.Clear();
                    Console.WriteLine("--------------------------");
                    Console.WriteLine("(" + arg1 + ") - Found cities:");
                    Console.WriteLine("--------------------------");
                    foreach (Gemeente gemeente in gemeentes)
                    {
                        Console.WriteLine(gemeente.Gemeentenaam);
                    }
                    Console.WriteLine("--------------------------");
                    Console.WriteLine("Press ENTER to continue...");
                    Console.ReadLine();
                }
                if (searchType.Equals("3"))
                {
                    Console.Clear();
                    Console.Write("Input a city name: ");
                    String arg1 = Console.ReadLine();
                    if (gemeentes.ContainsKey(arg1))
                    {
                        Gemeente gemeente = gemeentes[arg1];
                        Console.Clear();
                        Console.WriteLine("--------------------------");
                        Console.WriteLine("(" + arg1 + ") - Found information:");
                        Console.WriteLine("--------------------------");
                        Console.WriteLine("Name: " + gemeente.Gemeentenaam);
                        Console.WriteLine("NisCode: " + gemeente.NIScode);
                        Console.WriteLine("Postcode: N/A" );

                        int strcount = 0;
                        foreach (Straatnaam straat in streets.Values)
                        {
                            if (straat.Gemeente.Equals(gemeentes[arg1]))
                            {
                                ++strcount;
                            }
                        }

                        Console.WriteLine("Total streets: " + strcount);
                        Console.WriteLine("--------------------------");
                        Console.WriteLine("Press ENTER to continue...");
                        Console.ReadLine();

                    }
                }
                if (searchType.Equals("4"))
                {
                    Console.Clear();
                    Console.WriteLine("--------------------------");
                    Console.WriteLine("Statistics:");
                    Console.WriteLine("--------------------------");
                    Console.WriteLine("Total lines: " + count);
                    Console.WriteLine("Total addresses: " + addIds.Count);
                    Console.WriteLine("Total cities: " + gemeentes.Count);
                    Console.WriteLine("Total streets: " + streets.Count);
                    Console.WriteLine("Loading time: " + watch.Elapsed.TotalSeconds + " sec.");
                    Console.WriteLine("--------------------------");
                    Console.WriteLine("Press ENTER to continue...");
                    Console.ReadLine();
                }
            }
        }

        static void BuildCache()
        {
            while(true)
            {
                if (cache[0] != null)
                {
                    List<String> test = new List<String>(cache[0]);
                    buildObject(test);
                    cache.RemoveAt(0);
                }
            }
        }

        public static void buildObject(List<String> lines)
        {
            Dictionary<String, String> data = new Dictionary<String, String>();
            foreach (String line in lines) {
                // Collect data
                if (line.Contains(":ID>"))
                    data.Add("ID", cleanObject(line));

                if (line.Contains(":STRAATNMID"))
                    data.Add("STRAATNMID", cleanObject(line));

                if (line.Contains(":STRAATNM>"))
                    data.Add("STRAATNM", cleanObject(line));

                if (line.Contains(":HUISNR"))
                    data.Add("HUISNR", cleanObject(line));

                if (line.Contains(":APPTNR"))
                    data.Add("APPTNR", cleanObject(line));

                if (line.Contains(":BUSNR"))
                    data.Add("BUSNR", cleanObject(line));

                if (line.Contains(":HNRLABEL"))
                    data.Add("HNRLABEL", cleanObject(line));

                if (line.Contains(":NISCODE"))
                    data.Add("NISCODE", cleanObject(line));

                if (line.Contains(":GEMEENTE"))
                    data.Add("GEMEENTE", cleanObject(line));

                if (line.Contains(":POSTCODE"))
                    data.Add("POSTCODE", cleanObject(line));

                if (line.Contains(":HERKOMST"))
                    data.Add("HERKOMST", cleanObject(line));

                if (line.Contains(":X>"))
                    data.Add("X", cleanObject(line));

                if (line.Contains(":Y>"))
                    data.Add("Y", cleanObject(line));
            }

            // Build classes
            if (!gemeentes.ContainsKey(data["GEMEENTE"]))
            {
                Gemeente temp = new Gemeente(Int32.Parse(data["NISCODE"]), data["GEMEENTE"]);
                gemeentes.Add(data["GEMEENTE"], temp);
            }

            if (!streets.ContainsKey(Int32.Parse(data["STRAATNMID"])))
            {
                Straatnaam temp = new Straatnaam(Int32.Parse(data["STRAATNMID"]), data["STRAATNM"], gemeentes[data["GEMEENTE"]]);
                streets.Add(Int32.Parse(data["STRAATNMID"]), temp);
            }

            Adres adres = new Adres(Int32.Parse(data["ID"]), streets[Int32.Parse(data["STRAATNMID"])], data["APPTNR"], data["BUSNR"], data["HUISNR"], gemeentes[data["GEMEENTE"]], Int32.Parse(data["POSTCODE"]), Double.Parse(data["X"]), Double.Parse(data["Y"]));
            addresses.Add(Int32.Parse(data["ID"]), adres);

            addIds.Add(Int32.Parse(data["ID"]));





            // Remove cache
            if(addresses.Count > 2)
            {
                addresses.Remove(addresses.Keys.First());
            }

            // Console.WriteLine(adres.ToString());
        }

        static void ProgressThread()
        {
            while (true)
            {
                if((Convert.ToDouble((int)count) / 83550866) >= 1)
                {
                    Console.Clear();
                    Console.WriteLine("Loaded all data");
                    Console.WriteLine("Loaded " + addIds.Count + " addresses");
                    Console.WriteLine("Loaded " + gemeentes.Count + " cities");
                    Console.WriteLine("Loaded " + streets.Count + " steets");
                    break;
                }
                else
                {
                    drawTextProgressBar("", count, 83550866);
                }
                Thread.Sleep(20);
            }
        }

        public static String cleanObject(String line)
        {
            String[] clean = line.Split(new[] { '>' }, 2);
            clean = clean[1].Split(new[] { '<' }, 2);
            return clean[0];
        }

        public static void drawTextProgressBar(string stepDescription, int progress, int total)
        {
            int totalChunks = 50;

            Console.CursorLeft = 0;
            Console.Write("["); 
            Console.CursorLeft = totalChunks + 1;
            Console.Write("]"); 
            Console.CursorLeft = 1;

            double pctComplete = Convert.ToDouble((int) progress) / total;
            int numChunksComplete = Convert.ToInt16(totalChunks * pctComplete);

            Console.BackgroundColor = ConsoleColor.Green;
            Console.Write("".PadRight(numChunksComplete));

            Console.BackgroundColor = ConsoleColor.Gray;
            Console.Write("".PadRight(totalChunks - numChunksComplete));

            Console.CursorLeft = totalChunks + 5;
            Console.BackgroundColor = ConsoleColor.Black;

            string output = progress + " of " + total + " (" + string.Format("{0:F1}", pctComplete * 100) + "%)";
            Console.Write(output.PadRight(35) + stepDescription);
        }
    }
}
 