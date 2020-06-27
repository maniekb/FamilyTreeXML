using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FamilyTreeXML.Infrastructure
{
    public interface IFamilyTreeService
    {
        XDocument Get(int familyId);
        List<XDocument> Browse();
        XDocument AddChild(int familyId, Role role);
    }
}
