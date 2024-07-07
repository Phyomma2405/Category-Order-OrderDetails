using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiProjectPRN231;
using ApiProjectPRN231.Models;

namespace ApiProjectPRN231.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OrderDetailsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/OrderDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDetails>>> GetOrderDetails()
        {
            return await _context.OrderDetails.ToListAsync();
        }

        // GET: api/OrderDetails/5
        [HttpGet("{orderId}/{productId}")]
        public async Task<ActionResult<OrderDetails>> GetOrderDetails(int orderId, int productId)
        {
            var orderDetails = await _context.OrderDetails
                .Include(od => od.Order)
                .Include(od => od.Product)
                .FirstOrDefaultAsync(od => od.OrderId == orderId && od.ProductId == productId);

            if (orderDetails == null)
            {
                return NotFound();
            }

            return orderDetails;
        }

        // PUT: api/OrderDetails/5
        [HttpPut("{orderId}/{productId}")]
        public async Task<IActionResult> PutOrderDetails(int orderId, int productId, OrderDetails orderDetails)
        {
            if (orderId != orderDetails.OrderId || productId != orderDetails.ProductId)
            {
                return BadRequest();
            }

            _context.Entry(orderDetails).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderDetailsExists(orderId, productId))
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

        // POST: api/OrderDetails
        [HttpPost]
        public async Task<ActionResult<OrderDetails>> PostOrderDetails(OrderDetails orderDetails)
        {
            _context.OrderDetails.Add(orderDetails);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrderDetails", new { orderId = orderDetails.OrderId, productId = orderDetails.ProductId }, orderDetails);
        }

        // DELETE: api/OrderDetails/5
        [HttpDelete("{orderId}/{productId}")]
        public async Task<IActionResult> DeleteOrderDetails(int orderId, int productId)
        {
            var orderDetails = await _context.OrderDetails.FindAsync(orderId, productId);
            if (orderDetails == null)
            {
                return NotFound();
            }

            _context.OrderDetails.Remove(orderDetails);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderDetailsExists(int orderId, int productId)
        {
            return _context.OrderDetails.Any(od => od.OrderId == orderId && od.ProductId == productId);
        }
    }
}
