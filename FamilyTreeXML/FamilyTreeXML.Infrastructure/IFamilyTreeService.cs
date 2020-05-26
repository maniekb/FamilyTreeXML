using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FamilyTreeXML.Infrastructure
{
    public interface IFamilyTreeService
    {
        XDocument GetAsync();
    }
}
