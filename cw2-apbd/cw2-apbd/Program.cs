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

          

            Dictionary<string, Student> dict = new Dictionary<string, Student>();
            //Dictionary<String, int> kierunki = new Dictionary<string, int>();
            System.IO.File.WriteAllText(@"log.txt",String.Empty);


            Context2 kierunki = new Context2();


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

                        foreach(string stud in student)
                        {
                            if (stud.Equals(""))
                                pustaWart = true;

                        }

                        if(student.Length != 9)
                        {
                            System.IO.File.AppendAllText(@"log.txt", "Błąd danych(niepoprawna liczba kolumn) dla:  " + line + "\n");
                        }
                        else if(pustaWart)
                        {
                            System.IO.File.AppendAllText(@"log.txt", "Błąd danych(pusta kolumna) dla:  " + line + "\n");
                        }
                        else if(!dict.ContainsKey(student[4]))
                        {
                            string[] tb = student[2].Split(" ");
                            string kier = "";
                            for(int j = 0; j<tb.Length-1; j++)
                            {
                                kier += tb[j];
                                if (j < tb.Length - 2)
                                    kier += " ";
                            }

                            dict.Add(student[4], new Student()
                            {


                                index = student[4],
                                imie = student[0],
                                Nazwisko = student[1],
                                studies = new Studies()
                                {

                                    name = kier,
                                    mode = student[3]
                                },
                                dataUr = student[5],
                                email = student[6],
                                imieMatki = student[7],
                                imieOjca = student[8]


                            });


                            bool istn = false;
                            foreach(ActiveStudies aS in kierunki.As)
                            {
                                if (aS.name.Equals(kier))
                                {
                                    aS.numberOfStudents++;
                                    istn = true;
                                }
                                   
                               
                            }

                            if(!istn)
                            {
                                kierunki.As.Add(new ActiveStudies()
                                {
                                    name = kier,
                                    numberOfStudents = 1
                                });

                            }




                                    
                        }else
                        {
                            System.IO.File.AppendAllText(@"log.txt", "Dupilkat dla:  " + line + "\n");
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
            Context list = new Context();

            foreach (Student st in s)
                list.Students.Add(st);



           
           
           

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
            XmlSerializer serializer = new XmlSerializer(typeof(Context),typeof(Context2),
                                       new XmlRootAttribute("uczelnia"));
            XmlSerializer serializer2 = new XmlSerializer(typeof(Context2));


            serializer.Serialize(writer, list); // zapisuje jako plik xml w folderze /debug w projekciesera
            serializer2.Serialize(writer,kierunki);

         
            writer.Dispose();

            // pgago\studenci -> s16503.txt (w środku link do repozytorium)

            //do końca soboty
            //test2


            foreach (ActiveStudies a in kierunki.As)
                Console.WriteLine(a.name);
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



    [XmlRoot("Context2")]
    public class Context2
    {

        public Context2() { this.As = new List<ActiveStudies>();  }



        [XmlArray("activeStudies")]
        [XmlArrayItem("studies")]
        public List<ActiveStudies> As { get; set; }
    }

    public class ActiveStudies
    {

        //public ActiveStudies(string n, int i)
        //{
        //    this.name = n;
        //    this.numberOfStudents = i;
        //}
        
        [XmlAttribute(attributeName:"name")]
        public string name { get; set; }
        [XmlAttribute(attributeName: "numberOfStudents")]
        public int numberOfStudents { get; set; }
    }
}
