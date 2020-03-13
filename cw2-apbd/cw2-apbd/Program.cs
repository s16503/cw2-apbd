using cw2_apbd.models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace cw2_apbd
{
    class Program
    {
        static void Main(string[] args)
        {

            string source;
            string outPath;
            string format;

            // ściezki z argumentów lub domyślne
            if(args.Length<3)
            {
                source = @"data.csv";
                outPath = @"result.xml";
                format = "xml";
            }
            else
            {
                source = args[0];
                outPath = args[1];
                format = args[2];
            }


            Dictionary<string, int> dict = new Dictionary<string, int>();

            dict.Add("A", 55);
            dict.Add("B", 43);
            dict.Add("C", 22);

            foreach(int i in dict.Values)
            Console.WriteLine(">>>>> " + i); 



            //wczytywanie 
            var fi = new FileInfo(source);
            using (var stream = new StreamReader(fi.OpenRead()))    //blok using gdy mamy metode Dispose , zwalnianie zasobów
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
    
            var list = new List<Student>();

            //konstruktor lub .... object initializer 
            var st = new Student()
            {
                index = "s5555",
                imie = "Jan",
                Nazwisko = "Kowalski",
                email = "kowalski@wp.pl",
                dataUr = "02.03.1997",
                imieMatki = "Alicja",
                imieOjca = "Andrzej",
                S = new Studies()
                {
                    name = "Informatyka",
                    mode = "dzienne"
                }
                

            };

            list.Add(st);

            // student.imie = "blbla";  <<< tak na prrawde wykonywana jest metoda set 
            
            //Console.WriteLine(list[0].Imie);

            FileStream writer = new FileStream(outPath,FileMode.Create);
            XmlSerializer serializer = new XmlSerializer(typeof(List<Student>),
                                       new XmlRootAttribute("uczelnia"));

            
            serializer.Serialize(writer, list); // zapisuje jako plik xml w folderze /debug w projekcie

            writer.Dispose();

            // pgago\studenci -> s16503.txt (w środku link do repozytorium)

            //do końca soboty
            //test2

        }
    }
}
