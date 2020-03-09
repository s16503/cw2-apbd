using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace cw2_apbd.models
{
    public class Student
    {
        // prop + tabx2

            [XmlAttribute(attributeName:"email")]
        public string Email { get; set; }

        [XmlElement(elementName: "fname")]
        public string Imie { get; set; }
        //public string Nazwisko { get; set; }


            // prpfull + tabx  << właściwość pełna
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



    }
}
