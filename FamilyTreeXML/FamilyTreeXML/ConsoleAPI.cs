using FamilyTreeXML.Infrastructure;
using System.Configuration;
using System;

namespace FamilyTreeXML.App
{
    class ConsoleAPI
    {
        private readonly IFamilyTreeService FamilyTreeService;

        public ConsoleAPI(IFamilyTreeService familyTreeService)
        {
            FamilyTreeService = familyTreeService;
        }

        public void Run()
        {
            char choice;

            while(true)
            {
                Console.WriteLine("1 - View");
                choice = Console.ReadLine()[0];

                switch(choice)
                {
                    case '1':
                        Console.WriteLine(FamilyTreeService.GetAsync().ToString());
                        break;

                }
            }
            
        }
    }
}
