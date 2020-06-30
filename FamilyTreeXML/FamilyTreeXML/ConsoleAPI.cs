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
                Console.WriteLine("FamilyTreeXML");
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
                        Person father;
                        Person mother;
                        XDocument fatherFamily;
                        XDocument motherFamily;

                        Family newFamily = new Family();

                        Console.WriteLine("Is father family arleady in DB? (Y/N)");
                        choice = Console.ReadLine()[0];
                        if(choice == 'n')
                        {
                            father = InputUtilities.GetPersonData(Role.Father);
                            newFamily.FatherFamilyId = -1;
                            newFamily.Father = InputUtilities.PersonToXElement(father);
                        }
                        else if(choice == 'y')
                        {
                            var ids = FamilyTreeService.GetFamilyIds();
                            Console.WriteLine("Insert father family id.");
                            var fatherFamilyId = Convert.ToInt32(Console.ReadLine());
                            if (!ids.Contains(fatherFamilyId))
                            {
                                Console.WriteLine("No family with given id.");
                                break;
                            }
                            fatherFamily = FamilyTreeService.Get(fatherFamilyId);
                            newFamily.Father = InputUtilities.GetParentDataFromXDoc(fatherFamily, Role.Father);
                        }

                        Console.WriteLine("Is mother family arleady in DB? (Y/N)");
                        choice = Console.ReadLine()[0];
                        if (choice == 'n')
                        {
                            mother = InputUtilities.GetPersonData(Role.Mother);
                            newFamily.MotherFamilyId = -1;
                            newFamily.Mother = InputUtilities.PersonToXElement(mother);
                        }
                        else if (choice == 'y')
                        {
                            var ids = FamilyTreeService.GetFamilyIds();
                            Console.WriteLine("Insert mother family id.");
                            var motherFamilyId = Convert.ToInt32(Console.ReadLine());
                            if (!ids.Contains(motherFamilyId))
                            {
                                Console.WriteLine("No family with given id.");
                                break;
                            }
                            motherFamily = FamilyTreeService.Get(motherFamilyId);
                            newFamily.Mother = InputUtilities.GetParentDataFromXDoc(motherFamily, Role.Mother);
                        }

                        
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
                        while (true)
                        {
                            Console.WriteLine("Son or daughter? (S/D): ");
                            choice = Char.ToLower(Console.ReadLine()[0]);
                            if (choice == 's' || choice == 'd')
                                break;
                            Console.WriteLine("Please, type S/D.");
                        }
                        var role = choice == 's' ? Role.Son : Role.Daughter;
                        var child = InputUtilities.GetPersonData(role);
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
                        FamilyTreeService.DeleteAll();
                        break;

                }
            }
            
        }
    }
}
