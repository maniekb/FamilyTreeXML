using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
                else
                {
                    Console.WriteLine("No trees in the database.");
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
            String query = $"DELETE FROM FamilyTreeX.dbo.FamilyTrees WHERE id = {familyId};";
            int rowsAffected;
            XDocument xdoc;

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
    }
}
