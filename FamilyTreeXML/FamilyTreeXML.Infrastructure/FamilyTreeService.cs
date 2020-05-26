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
        public XDocument GetAsync()
        {
            String query = "SELECT tree from FamilyTreeX.dbo.FamilyTrees;";
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
