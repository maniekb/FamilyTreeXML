﻿using FamilyTreeXML.Infrastructure;
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
                Console.WriteLine("3 - Add child to family");
                Console.WriteLine("9 - Delete family with id");
                
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
                        Console.WriteLine("Insert family id:");
                        id = Convert.ToInt32(Console.ReadLine());
                        var child = InputUtilities.GetChildData();
                        FamilyTreeService.AddChild(id, child);
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
