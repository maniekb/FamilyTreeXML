using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace FamilyTreeXML.Infrastructure
{
    public static class HelperClass
    {
        public static XDocument CreateFamily(int fatherFamilyId, int motherFamilyId, Person mother, Person fater)
        {
            var xdoc = new XDocument();



            return xdoc;
        }

        public static XDocument AddChild(XDocument family, Person child)
        {
            XElement childNode = new XElement(child.Role.ToString(),
                new XElement("Firstname", child.Firstname),
                new XElement("Lastname", child.Lastname),
                new XElement("BirthDate", child.BirthDate.ToString(@"yyyy-MM-dd"))
            );

            var familyNode = family.Descendants("Family").FirstOrDefault();
            if(familyNode != null)
            {
                familyNode.Add(childNode);
            }

            return family;
        }
    }
}
