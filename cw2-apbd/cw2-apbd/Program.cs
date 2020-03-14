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

           // FileStream logWriter = new FileStream("log.txt", FileMode.Create);

            Dictionary<string, Student> dict = new Dictionary<string, Student>();


          

            //wczytywanie 
            var fi = new FileInfo(source);

            try
            {
            using (var stream = new StreamReader(fi.OpenRead()))    //blok using gdy mamy metode Dispose , zwalnianie zasobów
            {
                string line = null;

                while ((line = stream.ReadLine()) != null)
                {
                        Console.WriteLine(line);
                        string[] student = line.Split(',');

                        bool pustaWart = false;

                        foreach(string s in student)
                        {
                            if (s.Equals(""))
                                pustaWart = true;

                        }

                        if(student.Length != 9)
                        {
                            System.IO.File.WriteAllText(@"log.txt", "Błąd danych(niepoprawna liczba kolumn) dla:  " + line);
                        }
                        else if(pustaWart)
                        {
                            System.IO.File.WriteAllText(@"log.txt", "Błąd danych(pusta kolumna) dla:  " + line);
                        }
                        else if(!dict.ContainsKey(student[4]))
                        {
                            dict.Add(student[4], new Student()
                            {
                                index = student[4],
                                imie = student[0],
                                Nazwisko = student[1],
                                studies = new Studies()
                                {
                                    name = student[2].Split(" ")[0],
                                    mode = student[3]
                                },
                                dataUr = student[5],
                                email = student[6],
                                imieMatki = student[7],
                                imieOjca = student[8]


                            }
                                    );
                        }else
                        {
                            System.IO.File.WriteAllText(@"log.txt", "Dupilkat dla:  " + line);
                        }


                }
            }

            }
            catch(FileNotFoundException ex)
            {
                Console.WriteLine("Plik o nazwie " + source + " nie istnieje");
                System.IO.File.WriteAllText(@"log.txt", ex.Message);
            }
            catch(DirectoryNotFoundException ex2)
            {
                Console.WriteLine("Podana ścieżka jest niepoprawna");
                System.IO.File.WriteAllText(@"log.txt", ex2.Message);
            }


            var list = new Context();
            Student[] s = new Student[dict.Values.Count];
            dict.Values.CopyTo(s,0);

            foreach (Student ss in s)
                Console.WriteLine(ss.index);


            //stream.Dispose();


            //XML
            // - komunikacja i przesylanie informacji miedzy problemami
            // [XmlElement(elementName: "studies")]


           

            ////konstruktor lub .... object initializer 
            //var st = new Student()
            //{
            //    index = "s5555",
            //    imie = "Jan",
            //    Nazwisko = "Kowalski",
            //    email = "kowalski@wp.pl",
            //    dataUr = "02.03.1997",
            //    imieMatki = "Alicja",
            //    imieOjca = "Andrzej",
            //    studies = new Studies()
            //    {
            //        name = "Informatyka",
            //        mode = "dzienne"
            //    }


            //};


            //list.Students.Add(st);

            // student.imie = "blbla";  <<< tak na prrawde wykonywana jest metoda set 

            //Console.WriteLine(list[0].Imie);

            FileStream writer = new FileStream(outPath,FileMode.Create);
            XmlSerializer serializer = new XmlSerializer(typeof(Context),
                                       new XmlRootAttribute("uczelnia"));


              serializer.Serialize(writer, list); // zapisuje jako plik xml w folderze /debug w projekcie

         
            writer.Dispose();

            // pgago\studenci -> s16503.txt (w środku link do repozytorium)

            //do końca soboty
            //test2

        }
    }


    [XmlRoot("Context")]
    public class Context
    {

        public Context() { this.Students = new Students(); }

        [XmlArray("studenci")]
        [XmlArrayItem("student")]
        public Students Students { get; set; }
    }

    public class Students : List<Student>
    {
    }
}
