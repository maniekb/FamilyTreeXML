using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
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

        public XDocument AddChild(int familyId, Role role)
        {
            throw new NotImplementedException();
        }

        public List<XDocument> Browse()
        {
            var ids = new List<int>();
            var trees = new List<XDocument>();
            String query = $"SELECT id from FamilyTreeX.dbo.FamilyTrees;";
            XDocument xdoc;

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

            foreach(var id in ids)
            {
                trees.Add(Get(id));
            }

            return trees;
        }

        public XDocument Get(int familyId)
        {
            String query = $"SELECT tree from FamilyTreeX.dbo.FamilyTrees where id = {familyId};";
            XDocument xdoc;

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);
                XmlReader xr = cmd.ExecuteXmlReader();

                xdoc = XDocument.Load(xr);
                xr.Close();
            }

            return xdoc;
        }
    }
}
