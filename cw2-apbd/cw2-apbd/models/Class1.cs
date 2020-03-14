using System;
using System.Xml.Serialization;

namespace cw2_apbd.models
{
    [Serializable]
    public class Student
    {
        // prop + tabx2

        //    [XmlAttribute(attributeName:"email")]
        //public string Email { get; set; }
 

        [XmlAttribute(attributeName: "indexNumber")]
        public string index { get; set; }

        [XmlElement(elementName: "fname")]
        public string imie { get; set; }
        //public string nazwisko { get; set; }


        // prpfull + tabx  << właściwość pełna
        [XmlElement(elementName: "lname")]
        private string _nazwisko;

        public string Nazwisko
        {
            get { return _nazwisko; }
            set
            {
                if (value == null) throw new ArgumentException();
                _nazwisko = value;
            }
        }

        [XmlElement(elementName: "birthdate")]
        public string dataUr { get; set; }

        [XmlElement(elementName: "email")]
        public string email { get; set; }

        [XmlElement(elementName: "mothersName")]
        public string imieMatki { get; set; }

        [XmlElement(elementName: "fathersName")]
        public string imieOjca { get; set; }


        [XmlElement(elementName: "studies")]
        public Studies studies { get; set; }




    }
}
