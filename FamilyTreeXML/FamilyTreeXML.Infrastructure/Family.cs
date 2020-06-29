using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace FamilyTreeXML.Infrastructure
{
    public class Family
    {
        public int FatherFamilyId { get; set; }
        public int MotherFamilyId { get; set; }
        public XElement Father { get; set; }
        public XElement Mother { get; set; }
    }
}
