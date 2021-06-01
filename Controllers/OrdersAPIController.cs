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
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text.Json;
using System.Dynamic;
using System.Reflection;
using System.ComponentModel;

namespace hatshop.Controllers
{
    [Authorize]
    [Route("api/orders")]
    [ApiController]
    public class OrdersAPIController : ControllerBase
    {
        private readonly HatShopDbContext _context;

        public OrdersAPIController(HatShopDbContext context)
        {
            _context = context;
        }

        public string GetUserId()
        {
            string userIdValue = User.Identity.Name;
            if (User.Identity is ClaimsIdentity claimsIdentity)
            {
                var userIdClaim = claimsIdentity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

                if (userIdClaim != null)
                    return userIdClaim.Value;
            }
            return string.Empty;
        }

        // GET: api/orders
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<JsonResult> GetOrders()
        {
            var orders = await _context.Orders.Include(o => o.Hats).ThenInclude(h => h.Hat).ToListAsync();
            var result = new List<dynamic>();

            foreach (var order in orders)
            {
                IDictionary<string, object> newOrder = new ExpandoObject();

                PropertyInfo[] properties = order.GetType().GetProperties();
                foreach (PropertyInfo property in properties)
                    newOrder.Add(property.Name, property.GetValue(order, null));

                newOrder["Hats"] = order.Hats.Select(h => new { h.Hat, quantity = h.Quantity });

                result.Add(newOrder);
            }

            return new JsonResult(result, new JsonSerializerOptions
            {
                WriteIndented = true,
            });
        }

        // GET: api/OrdersAPI/5
        [Authorize(Roles = "Admin")]
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
        [NonAction]
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
        public async Task<HttpResponseMessage> PostOrder(IEnumerable<OrderHatViewModel> orderItems)
        {
            HttpResponseMessage response;
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
                        Hat hat = await _context.Hats.FindAsync(item.HatId);
                        if (hat.Stock < item.Quantity)
                        {
                            response = new HttpResponseMessage(HttpStatusCode.BadRequest);
                            response.Content = new StringContent("Out of stock");
                            return response;
                        }
                        hat.Stock = hat.Stock - item.Quantity >= 0 ? hat.Stock - item.Quantity : 0;
                        order.Total += hat.Price;
                        orderHats.Add(new OrderHat { OrderId = order.Id, HatId = item.HatId, Quantity = item.Quantity });
                    }

                    order.Hats = orderHats;

                    _context.Orders.Add(order);
                    await _context.SaveChangesAsync();

                    response = new HttpResponseMessage(HttpStatusCode.OK);
                    return response;
                }
                return new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }
            return new HttpResponseMessage(HttpStatusCode.Unauthorized);
        }

        // DELETE: api/OrdersAPI/5
        [NonAction]
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
