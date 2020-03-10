using cw2_apbd.models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

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

            var list = new List<Student>();


            //konstruktor lub .... object initializer 
            var st = new Student()
            {
                Imie = "Jan",
                Nazwisko = "Kowalski",
                Email = "kowalski@wp.pl"

            };

            list.Add(st);

            // student.imie = "blbla";  <<< tak na prrawde wykonywana jest metoda set 
            
            //Console.WriteLine(list[0].Imie);

            FileStream writer = new FileStream(@"data.xml",FileMode.Create);
            XmlSerializer serializer = new XmlSerializer(typeof(List<Student>),
                                       new XmlRootAttribute("uczelnia"));
            serializer.Serialize(writer, list); // zapisuje jako plik xml w folderze /debug w projekcie

            writer.Dispose();

            // pgago\studenci -> s16503.txt (w środku link do repozytorium)

            //do końca soboty
            //test

        }
    }
}
