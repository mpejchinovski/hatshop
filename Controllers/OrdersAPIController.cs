using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using hatshop.Data;
using hatshop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using hatshop.ViewModels;

namespace hatshop.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrdersAPIController : ControllerBase
    {
        private readonly HatShopDbContext _context;

        public OrdersAPIController(HatShopDbContext context)
        {
            _context = context;
        }

        // GET: api/OrdersAPI
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            string userIdValue = User.Identity.Name;
            if (User.Identity is ClaimsIdentity claimsIdentity)
            {
                var userIdClaim = claimsIdentity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

                if (userIdClaim != null)
                {
                    userIdValue = userIdClaim.Value;
                }
            }
            return await _context.Orders.ToListAsync();
        }

        // GET: api/OrdersAPI/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        // PUT: api/OrdersAPI/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, Order order)
        {
            if (id != order.Id)
            {
                return BadRequest();
            }

            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/OrdersAPI
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(IEnumerable<OrderHatViewModel> orderItems)
        {
            string userIdValue = User.Identity.Name;
            if (User.Identity is ClaimsIdentity claimsIdentity)
            {
                var userIdClaim = claimsIdentity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

                if (userIdClaim != null)
                {
                    userIdValue = userIdClaim.Value;

                    Order order = new Order
                    {
                        UserId = userIdValue,
                        DateTime = DateTime.Now
                    };

                    List<OrderHat> orderHats = new List<OrderHat>();
                    foreach (var item in orderItems)
                    {
                        orderHats.Add(new OrderHat { OrderId = order.Id, HatId = item.HatId, Quantity = item.Quantity });
                        order.Total += await _context.Hats.Where(h => h.Id == item.HatId).Select(h => h.Price).FirstOrDefaultAsync() * item.Quantity;
                    }

                    order.Hats = orderHats;

                    _context.Orders.Add(order);
                    await _context.SaveChangesAsync();

                    return CreatedAtAction("GetOrder", new { id = order.Id }, order);
                }
                return Unauthorized();
            }
            return Unauthorized();
        }

        // DELETE: api/OrdersAPI/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Order>> DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return order;
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}
