using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace FamilyTreeXML.Infrastructure
{
    public class FamilyTreeService : IFamilyTreeService
    {
        private readonly string ConnectionString;

        public FamilyTreeService(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public List<XDocument> Browse()
        {
            var ids = GetFamilyIds();
            var trees = new List<XDocument>();

            foreach(var id in ids)
            {
                trees.Add(Get(id));
            }

            return trees;
        }

        public void DeleteAll()
        {
            String query = $"DELETE FROM FamilyTreeX.dbo.FamilyTrees;";

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.ExecuteNonQuery();
            }
        }

        public List<int> GetFamilyIds()
        {
            var ids = new List<int>();
            String query = $"SELECT id FROM FamilyTreeX.dbo.FamilyTrees;";

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ids.Add(reader.GetInt32(0));
                    }
                }

                reader.Close();
            }

            return ids;
        }

        public XDocument Get(int familyId)
        {
            String query = $"SELECT tree FROM FamilyTreeX.dbo.FamilyTrees WHERE id = {familyId};";
            XDocument xdoc = new XDocument();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);
                XmlReader xr = cmd.ExecuteXmlReader();

                if(xr.MoveToContent() != XmlNodeType.None)
                {
                    xdoc = XDocument.Load(xr);
                }
                
                xr.Close();
            }

            return xdoc;
        }

        public int Delete(int familyId)
        {

            foreach(var family in Browse())
            {
                if(family.Root.Element("Family").Attribute("fatherFamilyId").Value == familyId.ToString()
                    || family.Root.Element("Family").Attribute("motherFamilyId").Value == familyId.ToString())
                {
                    return 0;
                }
            }


            String query = $"DELETE FROM FamilyTreeX.dbo.FamilyTrees WHERE id = {familyId};";
            int rowsAffected;

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);
                rowsAffected = cmd.ExecuteNonQuery();
            }

            return rowsAffected;
        }

        public int AddChild(int familyId, Person child)
        {
            var family = Get(familyId);

            family = HelperClass.AddChild(family, child);

            String query = $"UPDATE FamilyTreeX.dbo.FamilyTrees SET tree = '{family}' WHERE id = {familyId};";

            int rowsAffected;

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);
                rowsAffected = cmd.ExecuteNonQuery();
            }

            return rowsAffected;
        }

        public void AddFamily(Family newFamily)
        {
            var id = HelperClass.GetSmallestPossibleId(GetFamilyIds());
            var xdoc = HelperClass.CreateFamily(newFamily, id); 

            String query = $"INSERT INTO FamilyTreeX.dbo.FamilyTrees VALUES({id},'{xdoc.ToString()}');";

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.ExecuteNonQuery();
            }
        }

        public List<Tuple<int, int>> ChildInEveryFamilyCount()
        {
            var data = new List<Tuple<int,int>>();
            String query = $"SELECT * from TotalChildInEveryFamily();";

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        data.Add(new Tuple<int,int>(reader.GetInt32(0),reader.GetInt32(1)));
                    }
                }

                reader.Close();
            }

            return data;
        }


        public string GetPersonBirthDate(string firstname, string lastname)
        {
            string birthDate = null;
            String query = $"SELECT * FROM GetPersonBirthDate ('{firstname}', '{lastname}');";

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();
                    birthDate = reader.GetString(0);
                }

                reader.Close();
            }

            return birthDate;
        }

        public List<int> GetFamilyIdByParentName(string firstname, string lastname)
        {
            String query = $"SELECT Id from GetFamilyIdByParentName('{firstname}','{lastname}');";
            var ids = new List<int>();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ids.Add(reader.GetInt32(0));
                    }
                }

                reader.Close();
            }

            return ids;
        }

        public string GetFamilyDAboville(int id)
        {
            StringBuilder tree = new StringBuilder();
            var progenitors = Get(id).Root.Element("Family");

            tree.Append($"1. {progenitors.Element("Father").Element("Firstname").Value} " +
                $"{progenitors.Element("Father").Element("Lastname").Value} & " +
                $"{ progenitors.Element("Mother").Element("Firstname").Value} " +
                $"{progenitors.Element("Mother").Element("Lastname").Value}\n");


            var i = 1;

            var childs = progenitors.Elements("Son").ToList();
            childs.AddRange(progenitors.Elements("Daughter").ToList());

            foreach (var child in childs)
            {
                tree.Append($"   1.{i.ToString()}. {child.Element("Firstname").Value} {child.Element("Lastname").Value}\n");

                var familyIds = GetFamilyIdByParentName(child.Element("Firstname").Value, child.Element("Lastname").Value);
                if (!familyIds.Any())
                    continue;

                var childs1 = GetDAbovilleChilds(familyIds);
                var i1 = 1;

                foreach (var child1 in childs1)
                {
                    if(familyIds.Count > 1)
                    {
                        tree.Append($"      1.{i}{child1.Marriage}.{i1}. {child1.Firstname} {child1.Lastname}\n");
                    }
                    else
                    {
                        tree.Append($"      1.{i}.{i1}. {child1.Firstname} {child1.Lastname}\n");
                    }

                    familyIds = GetFamilyIdByParentName(child1.Firstname, child1.Lastname);
                    if (!familyIds.Any())
                        continue;

                    var childs2 = GetDAbovilleChilds(familyIds);

                    var i2 = 1;

                    foreach (var child2 in childs2)
                    {
                        if(familyIds.Count > 1)
                        {
                            tree.Append($"        1.{i}.{i1}{child2.Marriage}.{i2++}. {child2.Firstname} {child2.Lastname}\n");
                        }
                        else
                        {
                            tree.Append($"        1.{i}.{i1}.{i2++}. {child2.Firstname} {child2.Lastname}\n");
                        }              
                    }

                    i1++;
                }

                i++;
            }


            return tree.ToString();
        }

        public List<DAbovilleChild> GetDAbovilleChilds(List<int> familyIds)
        {
            var childs = new List<DAbovilleChild>();
            char m = 'a';
            foreach (var familyId in familyIds)
            {
                var progenitors = Get(familyId);
                var data = progenitors.Root.Element("Family").Elements("Son").ToList();
                data.AddRange(progenitors.Root.Element("Family").Elements("Daughter").ToList());
                childs.AddRange(data.Select(x => new DAbovilleChild { Marriage = m, Firstname = x.Element("Firstname").Value, Lastname = x.Element("Lastname").Value }));
                m++;
            }

            return childs;
        }
    }
}
