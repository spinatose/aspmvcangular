using DutchTreat.Data;
using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DutchTreat.Controllers
{
    [Route("api/[Controller]")]
    public class OrdersController : Controller
    {
        private readonly ILogger<OrdersController> logger;
        private readonly IDutchRepository repository;

        public OrdersController(ILogger<OrdersController> logger, IDutchRepository repository)
        {
            this.logger = logger;
            this.repository = repository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Order>> Get()
        {
            try
            {
                return Ok(this.repository.GetAllOrders());
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Failed to get orders: {ex}");
            }

            return BadRequest("Failed to get orders");
        }

        [HttpGet("{id:int}")]
        public ActionResult<Order> Get(int id)
        {
            try
            {
                var order = this.repository.GetOrderById(id);

                if (order != null)
                    return Ok(order);
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Failed to get order by id [{id}]: {ex}");
            }

            return BadRequest($"Failed to get order by id [{id}]");
        }

        [HttpPost]
        public IActionResult Post([FromBody]Order model)
        {
            try
            {
                this.repository.AddEntity(model);
                if (this.repository.SaveAll())
                    return Created($"/api/orders/{model.Id}", model);
            }
            catch (Exception ex)
            {

                 this.logger.LogError($"Failed to save new order: {ex}");               
            }

            return BadRequest("Failed to save new order");
        }
    }
}
