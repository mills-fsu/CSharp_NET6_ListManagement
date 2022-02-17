using Library.ListManagement.helpers;
using ListManagement.models;
using ListManagement.services;
using Newtonsoft.Json;
using System2 = System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ListManagement // Note: actual namespace depends on the project name.
{
    public class Program
    {
        
        static void Main(string[] args)
        {
            
            JsonSerializerSettings serializerSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            var persistencePath = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\SaveData.json";
            var itemService = ItemService.Current;
           
            Console.WriteLine("Welcome to the List Management App 2.0!");
            var List = new List<Item>();



            PrintMenu();

            int input;
            if (int.TryParse(Console.ReadLine(), out input))
            {
                while (input != 9) //not quitting
                {

                    if (input == 1) //create 
                    {
                        Console.WriteLine("Would you like to add a Task (t) or an Appointment (a)");
                        bool keeploop = true;
                        while (keeploop)
                        {
                            string type = Console.ReadLine();
                            if (type == "t")
                            {
                                //Add a ToDo
                                Console.WriteLine("Please enter a name: ");
                                string name = Console.ReadLine();
                                Console.WriteLine("Please enter a description: ");
                                string desc = Console.ReadLine();
                                Console.WriteLine("Please enter a deadline: ");
                                var dead = DateTime.Parse(Console.ReadLine());
                                ToDo nextTodo = new ToDo(name, desc, dead);
                                List.Add(nextTodo);
                                keeploop = false;
                            }
                            else if (type == "a")
                            {
                                //Add an Appointment
                                Console.WriteLine("Please enter a name: ");
                                string name = Console.ReadLine();
                                Console.WriteLine("Please enter a description: ");
                                string desc = Console.ReadLine();
                                Console.WriteLine("Please enter a starting date: ");
                                DateTime start = DateTime.Parse(Console.ReadLine());
                                Console.WriteLine("Please enter an ending date: ");
                                DateTime stop = DateTime.Parse(Console.ReadLine());
                                Console.WriteLine("Please enter the # of attendants: ");
                                int noAttend = Convert.ToInt32(Console.ReadLine());
                                Console.WriteLine("Please enter the names of the attendants:");
                                var Attendies = new List<String>();
                                for (int i = 0; i < noAttend; i++)
                                {
                                    Attendies.Add(Console.ReadLine());
                                }
                                var newTask = new Appointment(name, desc, start, stop, Attendies);
                                List.Add(newTask);
                                keeploop = false;
                            }
                        }


                    }
                    else if (input == 2)
                    {
                        //D - Delete/Remove
                        Console.WriteLine("Please select which task to delete by entering the index number...");
                        if (int.TryParse(Console.ReadLine(), out int selection))
                        {
                            
                            List.RemoveAt(selection-1);
                            Console.WriteLine("\nTask deleted.");
                        }
                        else
                        {
                            Console.WriteLine("Sorry, I can't find that item!");
                        }
                    }
                    else if (input == 3)
                    {
                        Console.WriteLine("Please give the index of the entry you would like to edit: ");
                        int index = int.Parse(Console.ReadLine());
                        int counter = index -1;
                        if (List[counter] is ToDo)
                        {                                                                     
                            Console.WriteLine("Give the task a new name:");
                            List[counter].Name = Console.ReadLine();
                            Console.WriteLine("Give the task a new description:");
                            List[counter].Description = Console.ReadLine();
                            Console.WriteLine("Give the task a deadline:");
                            (List[counter] as ToDo).Deadline = DateTime.Parse(Console.ReadLine());
                        }
                        else
                        {
                            Console.WriteLine("Give the appointment a new name:");
                            List[counter].Name = Console.ReadLine();
                            Console.WriteLine("Give the appointment a new description:");
                            List[counter].Description = Console.ReadLine();
                            Console.WriteLine("Give the appointment a new start time: \n\tFormat: MM/DD/YYYY HR:MN:SC AM/PM");
                            (List[counter] as Appointment).Start = DateTime.Parse(Console.ReadLine());
                            Console.WriteLine("Give the appointment a new end time: \n\tFormat: MM/DD/YYYY HR:MN:SC AM/PM");
                            (List[counter] as Appointment).End = DateTime.Parse(Console.ReadLine());
                            Console.WriteLine("Please provide the new number of attendees: (int)");
                            int noAttendies = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("Please enter the names of the attendees:");
                            var Attends = new List<String>();
                            for (int o = 0; o < noAttendies; o++)
                            {
                                Console.Write("\t");
                                Attends.Add(Console.ReadLine());
                            }
                            (List[counter] as Appointment).Attendees = Attends;                                                                      
                        }
                    }
                       

                    else if (input == 4)
                    {
                        //Complete Task
                        Console.WriteLine("Which item should I complete?");
                        if (int.TryParse(Console.ReadLine(), out int selection))
                        {
                            var selectedItem = itemService.Items[selection - 1] as ToDo;
                            if (selectedItem != null)
                            {
                                selectedItem.IsCompleted = true;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Sorry, I can't find that item!");
                        }
                    }
                    else if (input == 5) //listOustanding
                    {
                        var notComplete = new List<Item>();
                        for (int i = 0; i < List.Count; i++)
                        {
                            if (List[i] is ToDo)
                            {
                                if ((List[i] as ToDo).IsCompleted == false)
                                {
                                    notComplete.Add(List[i]);
                                }
                            }
                        }
                        ListNavigator(notComplete);

                    }
                    else if (input == 6) //listAll
                    {
                        ListNavigator(List);

                    }
                    else if (input == 7) //save
                    {
                        Console.WriteLine("Save (s) or Load (l): ");
                        string search = Console.ReadLine();
                        if (search == "s")
                        {
                            WriteToJsonFile<List<Item>>("C:\\Users\\reece\\Documents\\GitHub\\gabbett-proj2\\ListManagement\\save.txt", List);
                        }
                        else if (search == "l")
                        {
                            var Found = new List<ToDo>();
                            Found = ReadFromJsonFile<List<ToDo>>("C:\\Users\\reece\\Documents\\GitHub\\gabbett-proj2\\ListManagement\\save.txt");
                            List.AddRange(Found); 
                           
                        }
                    }
                    else if (input == 8) //search
                    {
                        Console.WriteLine("Enter a string to search for: ");
                        string stringToSearch = Console.ReadLine();
                        stringToSearch = stringToSearch.ToUpper();
                        var Found = new List<Item>();
                        var results = from item in List
                                      where item.Name.ToUpper().Contains(stringToSearch) || item.Description.ToUpper().Contains(stringToSearch)
                                      || ((item as Appointment)?.Attendees?.Any(a => a.ToUpper().Contains(stringToSearch)) ?? false)
                                      select item;
                        foreach (var res in results)
                        {
                            Found.Add(res);
                        }
                        ListNavigator(Found);
                    }
                    else
                    {
                        Console.WriteLine("No Items found for that string");
                    }

                    PrintMenu();
                    if (!int.TryParse(Console.ReadLine(), out input))
                    {
                        Console.WriteLine("Incorrect Input.");
                    }
                }
            }
            else
            {
                Console.WriteLine("User did not specify a valid int!");
            }

            
        }

        public static void PrintMenu()
        {
            Console.WriteLine("1. Add Item");
            Console.WriteLine("2. Delete Item");
            Console.WriteLine("3. Edit Item");
            Console.WriteLine("4. Complete Item");
            Console.WriteLine("5. List Outstanding");
            Console.WriteLine("6. List All");
            Console.WriteLine("7. Save");
            Console.WriteLine("8. Search");
            Console.WriteLine("9. Exit");
        }

        public static void ListNavigator(List<Item> list)
        {
            if (list.Count == 0)
                Console.WriteLine("This list is empty!");
            if (list.Count <= 5)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    Output(list, i);
                }
            }
            else
            {
                bool view = true;
                int page = 0;
                while (view)
                {
                    for (int i = (page * 5); i < ((page * 5) + 5); i++)
                    {
                        if (i >= list.Count)
                        {
                            if (page > 0)
                                Console.WriteLine("Input 'p' to see the previous page.");
                            if (page == 0 & list.Count > i)
                                Console.WriteLine("Input 'n' to see the next page.");
                            Console.WriteLine("Input 'q' to return to menu.");
                            string temp = Console.ReadLine();
                            temp = temp.ToLower();
                            if (temp == "n")
                                page = page + 1;
                            else if (temp == "p")
                                page = page - 1;
                            else if (temp == "q")
                                view = false;
                            else
                                Console.WriteLine("Entry not valid.");
                            Console.WriteLine("\n");
                            page = (page / 5) * 5;
                            break;
                        }
                        Output(list, i);
                        if (i == ((page * 5) + 4))
                        {
                            Console.WriteLine("Page navigation:");
                            if (page > 0)
                                Console.WriteLine("Input 'p' to see the previous page.");
                            if (page == 0 & list.Count > i)
                                Console.WriteLine("Input 'n' to see the next page.");
                            Console.WriteLine("Input 'q' to return to menu.");
                            string temp = Console.ReadLine();
                            if (temp == "n")
                                page = page + 1;
                            else if (temp == "p")
                                page = page - 1;
                            else if (temp == "q")
                                view = false;
                            else
                                Console.WriteLine("Entry not valid.");
                            Console.WriteLine("\n");
                        }
                    }
                }
            }
        }
        public static void Output(List<Item> list, int i)
        {
            if (list[i] is ToDo)
            {
                string print = ($"Entry #{i + 1}: {list[i].Name} {list[i].Description} at {(list[i] as ToDo).Deadline}");
                if ((list[i] as ToDo).IsCompleted) print = print + (" Status:\t\tCompleted");
                else print = print + (" Status:\t\tIncomplete"); 
                Console.WriteLine(print);
                
            }
            else if (list[i] is Appointment)
            {
                
                string print = ($"Entry #{i+1}: {list[i].Name} {list[i].Description} from {(list[i] as Appointment).Start} to {(list[i] as Appointment).End}");
                Console.WriteLine(print);
                Console.WriteLine("Attendees:");
                for (int j = 0; j < ((list[i] as Appointment).Attendees.Count); j++)
                {
                    Console.WriteLine("\t{0}", (list[i] as Appointment).Attendees[j]);
                }
            }
        }
        public static void WriteToJsonFile<T>(string filePath, List<Item> list, bool append = false) where T : new()
        {
            TextWriter writer = null;
            try
            {
                var contentsToWriteToFile = Newtonsoft.Json.JsonConvert.SerializeObject(list);
                writer = new StreamWriter(filePath, append);
                writer.Write(contentsToWriteToFile);
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }
        public static T ReadFromJsonFile<T>(string filePath) where T : new()
        {
            TextReader reader = null;
            try
            {
                reader = new StreamReader(filePath);
                var fileContents = reader.ReadToEnd();
                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(fileContents);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }

    }
}
