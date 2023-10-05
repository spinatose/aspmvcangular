using AutoMapper;
using DutchTreat.Data;
using DutchTreat.Data.Entities;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DutchTreat.Controllers
{
    [Route("api/[Controller]")]
    public class OrdersController : Controller
    {
        private readonly ILogger<OrdersController> logger;
        private readonly IDutchRepository repository;
        private readonly IMapper mapper;

        public OrdersController(ILogger<OrdersController> logger, IDutchRepository repository, IMapper mapper)
        {
            this.logger = logger;
            this.repository = repository;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get(bool includeItems = true)
        {
            try
            {
                var results = this.repository.GetAllOrders(includeItems);

                return Ok(this.mapper.Map<IEnumerable<OrderViewModel>>(results));
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Failed to get orders: {ex}");
            }

            return BadRequest("Failed to get orders");
        }

        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            try
            {
                var order = this.repository.GetOrderById(id);

                if (order != null)
                    return Ok(this.mapper.Map<Order, OrderViewModel>(order));
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
        public IActionResult Post([FromBody] OrderViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var newOrder = this.mapper.Map<Order>(model);

                    if (newOrder.OrderDate == DateTime.MinValue)
                        newOrder.OrderDate = DateTime.Now;

                    this.repository.AddEntity(newOrder);

                    if (this.repository.SaveAll())
                        return Created($"/api/orders/{newOrder.Id}", this.mapper.Map<OrderViewModel>(newOrder));
                }
                else
                    return BadRequest(ModelState);
            }
            catch (Exception ex)
            {

                this.logger.LogError($"Failed to save new order: {ex}");
            }

            return BadRequest("Failed to save new order");
        }
    }
}
