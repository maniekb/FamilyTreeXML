using FamilyTreeXML.Infrastructure;
using System.Configuration;
using System;
using System.Xml.Linq;
using System.Linq;

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
                Console.WriteLine("2 - Get tree with id");
                Console.WriteLine("3 - Delete tree with id");
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
                        int id2 = Convert.ToInt32(Console.ReadLine());
                        xdoc = FamilyTreeService.Get(id2);
                        if (xdoc.Root == null)
                        {
                            Console.WriteLine("No tree with given id.");
                            break;
                        }
                        Console.WriteLine(xdoc.ToString());                     
                        break;
                    case '3':
                        Console.WriteLine("Insert id:");
                        int id3 = Convert.ToInt32(Console.ReadLine());
                        int rowsAffected = FamilyTreeService.Delete(id3);
                        if(rowsAffected == 0)
                        {
                            Console.WriteLine("No tree with given id.");
                        }
                        break;

                }
            }
            
        }
    }
}
