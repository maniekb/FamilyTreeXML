using System;
using System.Collections.Generic;
using System.Text;

namespace FamilyTreeXML.Infrastructure
{
    public class Person
    {
        public int FamilyId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime BirthDate { get; set; }
        public Role Role { get; set; }
 
    }
}
