using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace FamilyTreeXML.Infrastructure
{
    public static class HelperClass
    {
        public static XDocument CreateFamily(Family familyData, int id)
        { 

            familyData.Father.Name = "Father";
            familyData.Mother.Name = "Mother";

            var newFamily = new XElement("Family", familyData.Father, familyData.Mother);

            newFamily.SetAttributeValue("Id", id);
            newFamily.SetAttributeValue("fatherFamilyId", familyData.FatherFamilyId);
            newFamily.SetAttributeValue("motherFamilyId", familyData.MotherFamilyId);

            var xdoc = new XDocument(newFamily);
            return new XDocument(new XElement("Tree", xdoc.Root));
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

        public static int GetSmallestPossibleId(List<int> ids)
            => (
                from n in ids
                where !ids.Select(nu => nu).Contains(n + 1)
                orderby n
                select n + 1
            ).FirstOrDefault();
    }
}
