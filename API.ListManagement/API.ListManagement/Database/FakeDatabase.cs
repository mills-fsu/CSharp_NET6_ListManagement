using ListManagement.models;

namespace API.ListManagement.Database
{
    static public class FakeDatabase
    {
        public static List<int> Ints = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        public static List<double> Doubles = new List<double> { 1.1, 2, 3.3, 4, 5, 6, 7, 8, 9, 10 };

        public static List<Item> Items = new List<Item>
        {
            new Appointment{Name = "App1", Description ="App1Desc"},
            new ToDo{Name="ToDo1", Description="ToDo1Desc", IsCompleted=false},
        };
    }
}
