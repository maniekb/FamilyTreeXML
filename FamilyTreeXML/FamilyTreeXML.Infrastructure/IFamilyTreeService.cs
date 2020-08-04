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
        List<int> GetFamilyIdByParentName(string firstname, string lastname);
        int Delete(int familyId);
        List<XDocument> Browse();
        int AddChild(int familyId, Person child);
        List<int> GetFamilyIds();
        void AddFamily(Family newFamily);
        List<Tuple<int, int>> ChildInEveryFamilyCount();
        string GetPersonBirthDate(string firstname, string lastname);
        string GetFamilyDAboville(int familyId);
        void DeleteAll();
    }
}
