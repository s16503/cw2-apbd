using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace cw2_apbd.models
{
    [Serializable]
    public class Studies
    {

      

        [XmlElement(elementName: "name")]
        public string name { get; set; }

        [XmlElement(elementName: "mode")]
        public string mode { get; set; }

    }
}
