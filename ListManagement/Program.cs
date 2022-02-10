
using ListManagement.models;

namespace ListManagement // Note: actual namespace depends on the project name.
{
    public class Program
    {
        static void Main(string[] args)
        {
            //setup
            var items = new List<Item>();
            Console.WriteLine("Welcome to the List App");

            //non-null ToDo
            ToDo nextTodo = new ToDo();

            //Menu
            PrintMenu(0);
            int input = -1;

            //check input
            if (int.TryParse(Console.ReadLine(), out input))
            {
                //if still running
                while (input != 7)
                {
                    //reset ToDo
                    nextTodo = new ToDo();

                    //Create
                    if (input == 1)
                    {
                        //ask for property values
                        Console.WriteLine("Input Name...");
                        nextTodo.Name = Console.ReadLine();

                        Console.WriteLine("Input Description...");
                        nextTodo.Description = Console.ReadLine();

                        Console.WriteLine("Input Deadline (ex. 01/13/2000)...");
                        nextTodo.Deadline = DateTime.Parse(Console.ReadLine());

                        //Add task
                        items.Add(nextTodo);
                        Console.WriteLine("\nTask added.");

                    }
                    //Delete
                    else if (input == 2)
                    {
                        //Print Items
                        PrintList(items);

                        //Delete indexed item
                        Console.WriteLine("Please select which task to delete by entering the index number...");
                        var listIndex = int.Parse(Console.ReadLine());
                        items.RemoveAt(listIndex - 1);
                        Console.WriteLine("\nTask deleted.");
                    }
                    //Update
                    else if (input == 3)
                    {
                        //Print Items
                        PrintList(items);

                        //Select Item
                        Console.WriteLine("\nPlease select which task to edit by entering the index number...");
                        var listIndex = int.Parse(Console.ReadLine());
                        listIndex--;

                        //Select Property
                        PrintMenu(1);
                        var prop = int.Parse(Console.ReadLine());

                        //If Name Change
                        if (prop == 1)
                        {
                            Console.WriteLine("\nPlease enter new name for task...");
                            items[listIndex].Name = Console.ReadLine();
                        }
                        //If Desc Change
                        else if (prop == 2)
                        {
                            Console.WriteLine("\nPlease enter new description for task...");
                            items[listIndex].Description = Console.ReadLine();
                        }
                        //If Date Change
                        else if (prop == 3)
                        {
                            //get datetime from ToDo instead of Item
                            Console.WriteLine("\nPlease enter new deadline for task...");
                            ToDo newToDo = (ToDo)items[listIndex];
                            newToDo.Deadline = DateTime.Parse(Console.ReadLine());
                            items[listIndex] = newToDo;
                        }
                        else
                        {
                            //Invalid
                            Console.WriteLine("\nInvalid entry.");
                        }
                        Console.WriteLine("\nTask updated.");
                    }
                    //Complete
                    else if (input == 4)
                    {
                        //List items to select Index
                        PrintList(items);
                        Console.WriteLine("\nPlease select which task to complete by entering the index number...");
                        var listIndex = int.Parse(Console.ReadLine());
                        listIndex--;

                        //get IsComplete from ToDo not Item
                        var newToDo = (ToDo)items[listIndex];
                        if (!newToDo.IsCompleted) { newToDo.IsCompleted = true; };
                        Console.WriteLine("\nTask completed.");

                    }
                    //List Outstandings
                    else if (input == 5)
                    {
                        //Print items
                        Console.WriteLine("\n\nOustanding Tasks:");
                        int i = 1;
                        foreach (var todo in items)
                        {
                            //if not completed
                            var newToDo = (ToDo)todo;
                            if (!newToDo.IsCompleted)
                            {
                                Console.WriteLine($"{i}) {todo.ToString()}");
                                i++;
                            }
                        }
                    }
                    //List All
                    else if (input == 6)
                    {
                        //Print Function
                        Console.WriteLine("\n\nAll Tasks:");
                        PrintList(items);
                    }
                    //Invalid
                    else
                    {
                        Console.WriteLine("\nInvalid input.");
                    }
                    //Print Menu and read new Input
                    PrintMenu(0);
                    input = int.Parse(Console.ReadLine());
                }
            }
            //first input is invalid
            else
            {
                Console.WriteLine("\nUser did not specify a valid int!");
            }
        }

        public static void PrintMenu(int menu)
        {
            if (menu == 0)
            {
                Console.WriteLine("\n\nList Management Menu\n----------------\nPlease input a number\n");
                Console.WriteLine("1: Create a New Task");
                Console.WriteLine("2: Delete an Existing Task");
                Console.WriteLine("3: Edit an Existing Task");
                Console.WriteLine("4: Complete a Task");
                Console.WriteLine("5: List all Oustanding Tasks");
                Console.WriteLine("6: List all Tasks");
                Console.WriteLine("7: Close Program");
            }

            if (menu == 1)
            {

                Console.WriteLine("\n\nPlease select which property to edit...");

                Console.WriteLine("1: Name");
                Console.WriteLine("2: Description");
                Console.WriteLine("3: Deadline");

            }
        }



        public static void PrintList(List<Item> items)
        {
            Console.WriteLine("\n\n");
            int i = 1;
            foreach (var todo in items)
            {
                Console.WriteLine($"{i}) {todo.ToString()}");
                i++;
            }
        }
    }
}