using API.ListManagement.Database;
using ListManagement.models;
using ListManagement.services;
using Microsoft.AspNetCore.Mvc;

namespace API.ListManagement.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AppointmentController : ControllerBase
    {
        private readonly ILogger<AppointmentController> _logger;
        public AppointmentController(ILogger<AppointmentController> logger)
        {
            _logger = logger;
        }

        [HttpGet()]
        public IEnumerable<Item> Get()
        {
            return FakeDatabase.Items;
        }

        [HttpPost("AddOrUpdate")]
        public Appointment AddOrUpdate([FromBody] Appointment appointment)
        {
            if (appointment.Id <= 0)
            {
                //CREATE
                appointment.Id = ItemService.Current.NextId;
                FakeDatabase.Items.Add(appointment);
            }
            else
            {
                //UPDATE
                var itemToUpdate = FakeDatabase.Items.FirstOrDefault(i => i.Id == appointment.Id);
                if (itemToUpdate != null)
                {
                    var index = FakeDatabase.Items.IndexOf(itemToUpdate);
                    FakeDatabase.Items.Remove(itemToUpdate);
                    FakeDatabase.Items.Insert(index, appointment);
                }
                else
                {
                    //CREATE FALLBACK
                    FakeDatabase.Items.Add(appointment);
                }
            }
            return appointment;
        }

        [HttpPost("Remove")]
        public void Remove([FromBody] Appointment appointment)
        {

            //REMOVE
            var itemToUpdate = FakeDatabase.Items.FirstOrDefault(i => i.Id == appointment.Id);
            if (itemToUpdate != null)
            {
                var index = FakeDatabase.Items.IndexOf(itemToUpdate);
                FakeDatabase.Items.Remove(itemToUpdate);
            }



        }


    }
}
