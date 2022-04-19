using API.ListManagement.Database;
using ListManagement.models;
using ListManagement.services;
using Microsoft.AspNetCore.Mvc;

namespace API.ListManagement.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ToDoController : ControllerBase
    {
        
        private  List<int> numbers= new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        private readonly ILogger<ToDoController> _logger;

        public ToDoController(ILogger<ToDoController> logger)
        {
            _logger = logger;
        }

        [HttpGet()]
        public IEnumerable<Item> Get()
        {
            return FakeDatabase.Items;
        }

        [HttpPost("AddOrUpdate")]
        public ToDo AddOrUpdate([FromBody] ToDo todo)
        {
            if(todo.Id <= 0)
            {
                //CREATE
                todo.Id = ItemService.Current.NextId;
                FakeDatabase.Items.Add(todo);
            }
            else
            {
                //UPDATE
                var itemToUpdate = FakeDatabase.Items.FirstOrDefault(i => i.Id == todo.Id);
                if (itemToUpdate != null)
                {
                    var index = FakeDatabase.Items.IndexOf(itemToUpdate);
                    FakeDatabase.Items.Remove(itemToUpdate);
                    FakeDatabase.Items.Insert(index, todo);
                } else
                {
                    //CREATE FALLBACK
                    FakeDatabase.Items.Add(todo);
                }
            }
            return todo;
        }




    }
}