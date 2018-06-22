using EFCoreConsole.Data;
using EFCoreConsole.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EFCoreConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            int result = -1;
            while(result != 9)
            {
                result = MainMenu();
            }
        }

        static int MainMenu()
        {
            int result = -1;
            ConsoleKeyInfo cki;
            bool cont = false;
            do
            {
                Console.Clear();
                WriteHeader("Welcome to Newbie Data Systems");
                WriteHeader("Main Menu");
                Console.WriteLine("\r\nPlease select from the list below for what you would like to do today");
                Console.WriteLine("1. List All Machines in Inventory");
                Console.WriteLine("2. List All Operating System");
                Console.WriteLine("3. Data Entry Menu");
                Console.WriteLine("9. Exit Menu");
                cki = Console.ReadKey();
                try
                {
                    result = Convert.ToInt16(cki.KeyChar.ToString());
                    if (result == 1)
                    {
                        // DisplayAllMachines();
                    }
                    else if (result == 2)
                    {
                         DisplayOperatingSystems();
                    }
                    else if (result == 3)
                    {
                        DataEntryMenu();
                    }
                    else if (result == 9)
                    {
                        cont = true;
                    }
                }
                catch (FormatException)
                {
                    // a key that wasn't a number
                }
            }
            while (!cont);

            return result;
        }

        static void DataEntryMenu()
        {
            ConsoleKeyInfo cki;
            int result = -1;
            bool cont = false;
            do
            {
                Console.Clear();
                WriteHeader("Data Entry Menu");
                Console.WriteLine("\r\nPlease select from the list below for what you would like to do today");
                Console.WriteLine("1. Add a New Machine");
                Console.WriteLine("2. Add a New Operating System");
                Console.WriteLine("3. Add a New Warranty Provider");
                Console.WriteLine("9. Exit Menu");
                cki = Console.ReadKey();
                try
                {
                    result = Convert.ToInt16(cki.KeyChar.ToString());
                    if (result == 1)
                    {
                        //AddMachine();
                    }
                    else if (result == 2)
                    {
                        AddOperatingSystem();
                    }
                    else if (result == 3)
                    {
                        //AddNewWarrantyProvider();
                    }
                    else if (result == 9)
                    {
                        // We are exiting so nothing to do
                        cont = true;
                    }
                }
                catch (System.FormatException)
                {
                    // a key that wasn't a number
                }
            } while (!cont);
        }

        static void DisplayOperatingSystems()
        {
            Console.Clear();
            Console.WriteLine("Operating Systems");
            using (var context = new MachineContext())
            {
                foreach (var os in context.OperatingSys.ToList())
                {
                    Console.Write($"Name: {os.Name,-39}\tStill Supported = ");
                    if (os.StillSupported == true)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    Console.WriteLine(os.StillSupported);
                    Console.ForegroundColor = ConsoleColor.Green;
                }
            }
            Console.WriteLine("\r\nAny key to continue...");
            Console.ReadKey();
        }

        static void AddOperatingSystem()
        {
            Console.Clear();
            ConsoleKeyInfo cki;
            string result;
            bool cont = false;
            OperatingSys os = new OperatingSys();
            string osName = "";
            do
            {
                WriteHeader("Add New Operating System");
                Console.WriteLine("Enter the Name of the Operating System and hit Enter");
                osName = Console.ReadLine();
                if (osName.Length >= 4)
                {
                    cont = true;
                }
                else
                {
                    Console.WriteLine("Please enter a valid OS name of at least 4 characters.\r\nPress and key to continue...");
                    Console.ReadKey();
                }
            } while (!cont);
            cont = false;
            os.Name = osName;
            Console.WriteLine("Is the Operating System still supported? [y or n]");
            do
            {
                cki = Console.ReadKey();
                result = cki.KeyChar.ToString();
                cont = ValidateYorN(result);
            } while (!cont);
            if (result.ToLower() == "y")
            {
                os.StillSupported = true;
            }
            else
            {
                os.StillSupported = false;
            }
            cont = false;
            do
            {
                Console.Clear();
                Console.WriteLine($"You entered {os.Name} as the Operating System Name\r\nIs the OS still supported, you entered {os.StillSupported}.\r\nDo you wish to continue? [y or n]");
                cki = Console.ReadKey();
                result = cki.KeyChar.ToString();
                cont = ValidateYorN(result);
            } while (!cont);
            if (result.ToLower() == "y")
            {
                bool exists = CheckForExistingOS(os.Name);
                if (exists)
                {
                    Console.WriteLine("\r\nOperating System already exists in the database\r\nPress any key to continue...");
                    Console.ReadKey();
                }
                else
                {
                    using (var context = new MachineContext())
                    {
                        Console.WriteLine("\r\nAttempting to save changes...");
                        context.OperatingSys.Add(os);
                        int i = context.SaveChanges();
                        if (i == 1)
                        {
                            Console.WriteLine("Contents Saved\r\nPress any key to continue...");
                            Console.ReadKey();
                        }
                    }
                }
            }
        }

        static void SelectOperatingSystem(string operation)
        {
            ConsoleKeyInfo cki;
            Console.Clear();
            WriteHeader($"{operation} an Existing Operating System Entry");
            Console.WriteLine($"{"ID",-7}|{"Name",-50}|Still Supported");
            Console.WriteLine("-------------------------------------- -----------");
            using (var context = new MachineContext())
            {
                List<OperatingSys> lOperatingSystems = context.OperatingSys.ToList();
                foreach (OperatingSys os in lOperatingSystems)
                {
                    Console.WriteLine($"{os.OperatingSysId,-7}|{os.Name,-50}|{os.StillSupported}");
                }
            }
            Console.WriteLine($"\r\nEnter the ID of the record you wish to {operation} and hit Enter\r\nYou can hit Esc to exit this menu");
            bool cont = false;
            string id = "";
            do
            {
                cki = Console.ReadKey(true);
                if (cki.Key == ConsoleKey.Escape)
                {
                    cont = true;
                    id = "";
                }
                else if (cki.Key == ConsoleKey.Enter)
                {
                    if (id.Length > 0)
                    {
                        cont = true;
                    }
                    else
                    {
                        Console.WriteLine("Please enter an ID that is at least 1 digit.");
                    }
                }
                else if (cki.Key == ConsoleKey.Backspace)
                {
                    Console.Write("\b \b");
                    try
                    {
                        id = id.Substring(0, id.Length - 1);
                    }
                    catch (System.ArgumentOutOfRangeException)
                    {
                        // at the 0 position, can't go any further back
                    }
                }
                else
                {
                    if (char.IsNumber(cki.KeyChar))
                    {
                        id += cki.KeyChar.ToString();
                        Console.Write(cki.KeyChar.ToString());
                    }
                }
            } while (!cont);
            int osId = Convert.ToInt32(id);
            if ("Delete" == operation)
            {
                DeleteOperatingSystem(osId);
            }
            else if ("Modify" == operation)
            {
                //ModifyOperatingSystem(osId);
            }
        }

        static OperatingSys GetOperatingSystemById(int id)
        {
            var context = new MachineContext();
            OperatingSys os = context.OperatingSys.FirstOrDefault(i => i.OperatingSysId == id);
            return os;
        }

        static void DeleteOperatingSystem(int id)
        {
            OperatingSys os = GetOperatingSystemById(id);
            if (os != null)
            {
                Console.WriteLine($"\r\nAre you sure you want to delete {os.Name}? [y or n]");
                ConsoleKeyInfo cki;
                string result;
                bool cont;
                do
                {
                    cki = Console.ReadKey(true);
                    result = cki.KeyChar.ToString();
                    cont = ValidateYorN(result);
                } while (!cont);
                if ("y" == result.ToLower())
                {
                    Console.WriteLine("\r\nDeleting record");
                    using (var context = new MachineContext())
                    {
                        context.Remove(os);
                        context.SaveChanges();
                    }
                    Console.WriteLine("Record Deleted");
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine("Delete Aborted\r\nHit any key to continue...");
                    Console.ReadKey();
                }
            }
            else
            {
                Console.WriteLine("\r\nOperating System Not Found!");
                Console.ReadKey();
                SelectOperatingSystem("Delete");
            }
        }

        static void WriteHeader(string headerText)
        {
            Console.WriteLine(string.Format("{0," + ((Console.WindowWidth / 2) + headerText.Length / 2) + "}", headerText));
        }

        static bool ValidateYorN(string entry)
        {
            if (entry.ToLower() == "y" || entry.ToLower() == "n")
            {
                return true;
            }

            return false;
        }

        static bool CheckForExistingOS(string osName)
        {
            bool exists = false;
            using (var context = new MachineContext())
            {
                var os = context.OperatingSys.Where(prop => prop.Name == osName);
                if (os.Count() > 0)
                {
                    exists = true;
                }
            }

            return exists;
        }
    }
}
