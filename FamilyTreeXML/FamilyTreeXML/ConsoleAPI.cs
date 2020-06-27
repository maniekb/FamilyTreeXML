using FamilyTreeXML.Infrastructure;
using System.Configuration;
using System;
using System.Xml.Linq;

namespace FamilyTreeXML.App
{
    class ConsoleAPI
    {
        private readonly IFamilyTreeService FamilyTreeService;
        private XDocument xdoc;

        public ConsoleAPI(IFamilyTreeService familyTreeService)
        {
            FamilyTreeService = familyTreeService;
        }

        public void Run()
        {
            char choice;

            while(true)
            {
                Console.WriteLine("--- OPTIONS ---");
                Console.WriteLine("1 - Browse all trees");
                Console.WriteLine("2 - Get tree by id");
                choice = Console.ReadLine()[0];
                Console.Clear();

                switch (choice)
                {
                    case '1':
                        var trees = FamilyTreeService.Browse();
                        foreach(var tree in trees)
                        {
                            Console.WriteLine(tree.ToString());
                        }
                        break;
                    case '2':
                        Console.WriteLine("Insert id:");
                        int id = Convert.ToInt32(Console.ReadLine());
                        xdoc = FamilyTreeService.Get(id);
                        Console.WriteLine(xdoc.ToString());                     
                        break;

                }
            }
            
        }
    }
}
