using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Data;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly HotelContext _db;

        public OrdersController(HotelContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> Get()
        {
            return await _db.Orders.Include(o => o.Client).Include(o => o.Employee).Include(o => o.Room).Take(20).ToListAsync();
        }

        [HttpGet("clients")]
        public async Task<ActionResult<IEnumerable<Client>>> GetClients()
        {
            return await _db.Clients.OrderBy(c => c.Id).ToListAsync();
        }

        [HttpGet("employees")]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            return await _db.Employees.OrderBy(e => e.Id).ToListAsync();
        }

        [HttpGet("rooms")]
        public async Task<ActionResult<IEnumerable<Room>>> GetRooms()
        {
            return await _db.Rooms.OrderBy(r => r.Id).ToListAsync();
        }


        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> Get(int id)
        {
            Order order = await _db.Orders.FirstOrDefaultAsync(o => o.Id == id);
            if (order == null)
                return NotFound();

            return new ObjectResult(order);
        }

        // POST api/<ValuesController>
        [HttpPost]
        public async Task<ActionResult<Order>> Post(Order order)
        {
            if (order == null)
                return BadRequest();

            var employee = await _db.Employees.FirstOrDefaultAsync(e => e.Id == order.EmployeeId);
            var client = await _db.Clients.FirstOrDefaultAsync(c => c.Id == order.ClientId);
            var room = await _db.Rooms.FirstOrDefaultAsync(r => r.Id == order.RoomId);
            if (employee == null || client == null || room == null)
                return NotFound();

            Order newOrder = new Order
            {
                CheckInDate = order.CheckInDate,
                CheckOut = order.CheckOut,
                EmployeeId = order.EmployeeId,
                ClientId = order.ClientId,
                RoomId = order.RoomId
            };

            _db.Orders.Add(newOrder);
            await _db.SaveChangesAsync();

            order.Id = _db.Orders.ToList().LastOrDefault().Id;
            order.Employee = employee;
            order.Client = client;
            order.Room = room;

            return Ok(order);
        }

        // PUT api/<ValuesController>/5
        [HttpPut]
        public async Task<ActionResult<Order>> Put(Order updOrder)
        {
            Order order = await _db.Orders.FirstOrDefaultAsync(o => o.Id == updOrder.Id);
            if (order == null)
                return NotFound();

            var employee = await _db.Employees.FirstOrDefaultAsync(e => e.Id == updOrder.EmployeeId);
            var client = await _db.Clients.FirstOrDefaultAsync(c => c.Id == updOrder.ClientId);
            var room = await _db.Rooms.FirstOrDefaultAsync(r => r.Id == updOrder.RoomId);
            if (employee == null || client == null || room == null)
                return NotFound();

            order.CheckInDate = updOrder.CheckInDate;
            order.CheckOut = updOrder.CheckOut;
            order.EmployeeId = updOrder.EmployeeId;
            order.ClientId = updOrder.ClientId;
            order.RoomId = updOrder.RoomId;

            _db.Update(order);
            await _db.SaveChangesAsync();

            order.Employee = employee;
            order.Client = client;
            order.Room = room;

            return Ok(order);
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Order>> Delete(int id)
        {
            Order order = await _db.Orders.FirstOrDefaultAsync(o => o.Id == id);
            if (order == null)
                return NotFound();

            _db.Orders.Remove(order);
            await _db.SaveChangesAsync();

            return Ok(order);
        }
    }
}
