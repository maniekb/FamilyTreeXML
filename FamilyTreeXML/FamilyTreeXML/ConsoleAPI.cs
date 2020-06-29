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
            int id;

            while(true)
            {
                Console.WriteLine("--- OPTIONS ---");
                Console.WriteLine("1 - Browse all families");
                Console.WriteLine("2 - Get family with id");
                Console.WriteLine("3 - Create new family");
                Console.WriteLine("4 - Add child to family");
                Console.WriteLine("8 - Delete family with id");
                Console.WriteLine("9 - Delete all families");

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
                        id = Convert.ToInt32(Console.ReadLine());
                        xdoc = FamilyTreeService.Get(id);
                        if (xdoc.Root == null)
                        {
                            Console.WriteLine("No family with given id.");
                            break;
                        }
                        Console.WriteLine(xdoc.ToString());                     
                        break;
                    case '3':
                        var ids = FamilyTreeService.GetFamilyIds();
                        Console.WriteLine("Insert father family id: ");
                        var fatherFamilyId = Convert.ToInt32(Console.ReadLine());
                        if (!ids.Contains(fatherFamilyId))
                        {
                            Console.WriteLine("No family with given id.");
                            break;
                        }
                        Console.WriteLine("Insert mother family id: ");
                        var motherFamilyId = Convert.ToInt32(Console.ReadLine());
                        if(!ids.Contains(motherFamilyId))
                        {
                            Console.WriteLine("No family with given id.");
                            break;
                        }

                        XDocument fatherFamily = FamilyTreeService.Get(fatherFamilyId);
                        XDocument motherFamily = FamilyTreeService.Get(motherFamilyId);
                        var newFamily = InputUtilities.GetNewFamilyData(fatherFamily, motherFamily);

                        FamilyTreeService.AddFamily(newFamily);

                        break;
                    case '4':
                        Console.WriteLine("Insert family id:");                
                        id = Convert.ToInt32(Console.ReadLine());
                        if(!FamilyTreeService.GetFamilyIds().Contains(id))
                        {
                            Console.WriteLine("No family with given id.");
                            break;
                        }
                        var child = InputUtilities.GetChildData();
                        FamilyTreeService.AddChild(id, child);
                        break;
                    case '8':
                        Console.WriteLine("Insert id:");
                        int id3 = Convert.ToInt32(Console.ReadLine());
                        int rowsAffected = FamilyTreeService.Delete(id3);
                        if (rowsAffected == 0)
                        {
                            Console.WriteLine("No tree with given id.");
                        }
                        break;
                    case '9':
                        Console.WriteLine("Insert id:");
                        int id3 = Convert.ToInt32(Console.ReadLine());
                        int rowsAffected = FamilyTreeService.Delete(id3);
                        if (rowsAffected == 0)
                        {
                            Console.WriteLine("No tree with given id.");
                        }
                        break;

                }
            }
            
        }
    }
}
