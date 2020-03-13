using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace cw2_apbd.models
{
    class Studies
    {

       // [XmlElement(Namespace ="studies")]

        [XmlAttribute(AttributeName = "name")]
        public string name { get; set; }

        [XmlElement(elementName: "mode")]
        public string mode { get; set; }

    }
}
