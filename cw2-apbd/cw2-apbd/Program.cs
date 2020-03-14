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

            Context list = new Context();   // listę studentów i kierunków przechowuje w obj Context, którego potem uzyję do serializacji

            Dictionary<string, Student> dict = new Dictionary<string, Student>();
            //Dictionary<String, int> kierunki = new Dictionary<string, int>();
            System.IO.File.WriteAllText(@"log.txt",String.Empty);

            //wczytywanie 
            var fi = new FileInfo(source);

            try
            {
            using (var stream = new StreamReader(fi.OpenRead()))    //blok using gdy mamy metode Dispose , zwalnianie zasobów
            {
                string line = null;

                while ((line = stream.ReadLine()) != null)      // odczyt z pliku
                {
                       // Console.WriteLine(line);
                        string[] student = line.Split(',');

                        bool pustaWart = false;

                        foreach(string stud in student) // szukam pustych kolumn
                        {
                            if (stud.Equals(""))
                                pustaWart = true;

                        }

                        if(student.Length != 9) // jesli liczba kolumn sie nie zgadza
                        {
                            System.IO.File.AppendAllText(@"log.txt", "Błąd danych(niepoprawna liczba kolumn) dla:  " + line + "\n");
                            Console.WriteLine("Błąd danych(niepoprawna liczba kolumn) dla:  " + line + "\n");
                        }
                        else if(pustaWart)  // jesli któraś kolumna jest pusta
                        {
                            System.IO.File.AppendAllText(@"log.txt", "Błąd danych(pusta kolumna) dla:  " + line + "\n");
                            Console.WriteLine("Błąd danych(pusta kolumna) dla:  " + line + "\n");
                        }
                        else if(!dict.ContainsKey(student[4]))
                        {
                     
                            dict.Add(student[4], new Student()
                            {

                                index = student[4],
                                imie = student[0],
                                nazwisko = student[1],
                                studies = new Studies()
                                {

                                    name = student[2],
                                    mode = student[3]
                                },
                                dataUr = student[5],
                                email = student[6],
                                imieMatki = student[7],
                                imieOjca = student[8]


                            });

                            Console.WriteLine("DODANO: " + line);

                            bool istn = false;
                            foreach(ActiveStudies aS in list.As)
                            {
                                if (aS.name.Equals(student[2]))
                                {
                                    aS.numberOfStudents++;
                                    istn = true;
                                }
                                   
                               
                            }

                            if(!istn)
                            {
                               list.As.Add(new ActiveStudies()
                                {
                                    name = student[2],
                                    numberOfStudents = 1
                                });

                            }
       
                        }else
                        {
                            System.IO.File.AppendAllText(@"log.txt", "Dupilkat dla:  " + line + "\n");
                            Console.WriteLine("Dupilkat dla:  " + line + "\n");
                        }

                }
            }

            }
            catch(FileNotFoundException ex)
            {
                Console.WriteLine("Plik o nazwie " + source + " nie istnieje");
                System.IO.File.AppendAllText(@"log.txt", ex.Message + "\n");
            }
            catch(DirectoryNotFoundException ex2)
            {
                Console.WriteLine("Podana ścieżka jest niepoprawna");
                System.IO.File.AppendAllText(@"log.txt", ex2.Message + "\n");
            }


            //var list = new Context();
            Student[] s = new Student[dict.Values.Count];
            dict.Values.CopyTo(s,0);
            

            foreach (Student st in s)
                list.Students.Add(st);


            //stream.Dispose();
            //XML
            // - komunikacja i przesylanie informacji miedzy problemami
            // [XmlElement(elementName: "studies")]
            // student.imie = "blbla";  <<< tak na prrawde wykonywana jest metoda set 
            //Console.WriteLine(list[0].Imie);

            FileStream writer = new FileStream(outPath,FileMode.Create);
            XmlSerializer serializer = new XmlSerializer(typeof(Context));
            XmlSerializerNamespaces xmlnsEmpty = new XmlSerializerNamespaces();
            xmlnsEmpty.Add(string.Empty, string.Empty);


            serializer.Serialize(writer, list,xmlnsEmpty); // zapisuje jako plik xml w folderze /debug w projekciesera     
            writer.Dispose();

            // pgago\studenci -> s16503.txt (w środku link do repozytorium)

            //do końca soboty
            //test2

            foreach (ActiveStudies a in list.As)
                Console.WriteLine(a.name);

        }
    }

    [Serializable]
    [XmlRoot("uczelnia")]
    public class Context
    {
        [XmlAttribute("createdAt")]
        public string date = DateTime.Now.ToShortDateString();
        [XmlAttribute("author")]
        public string aut = "Jan Klejn";

        public Context() 
        { this.Students = new Students();
          this.As = new List<ActiveStudies>();
        }

       

        [XmlArray("studenci")]
        [XmlArrayItem("student")]
        public Students Students { get; set; }

        ///////

        [XmlArray("activeStudies")]
        [XmlArrayItem("studies")]
        public List<ActiveStudies> As { get; set; }
    }

    public class Students : List<Student>
    {
    }

    public class ActiveStudies
    {
        [XmlAttribute(attributeName:"name")]
        public string name { get; set; }
        [XmlAttribute(attributeName: "numberOfStudents")]
        public int numberOfStudents { get; set; }
    }
}
