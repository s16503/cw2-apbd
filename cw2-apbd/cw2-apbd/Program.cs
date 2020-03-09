using System;
using System.IO;

namespace cw2_apbd
{
    class Program
    {
        static void Main(string[] args)
        {

            
            Console.WriteLine("Hello World!");


            string path = @"data\dane.csv";
            //wczytywanie 
            var fi = new FileInfo(path);
            using (var stream = new StreamReader(fi.OpenRead()))    //blok using gdy ammy metode Dispose , zwalnianie zasobów
            {

                string line = null;

                while ((line = stream.ReadLine()) != null)
                {
                    Console.WriteLine(line);
                }
            }

            //stream.Dispose();


            //XML
            // - komunikacja i przesylanie informacji miedzy problemami
            // - 



        }
    }
}
