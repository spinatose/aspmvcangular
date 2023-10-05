using AutoMapper;
using DutchTreat.Data;
using DutchTreat.Data.Entities;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DutchTreat.Controllers
{
    [Route("api/orders/{orderid}/items")]
    public class OrderItemsController : Controller
    {
        private readonly ILogger<OrderItemsController> logger;
        private readonly IDutchRepository repository;
        private readonly IMapper mapper;

        public OrderItemsController(ILogger<OrderItemsController> logger, IDutchRepository repository, IMapper mapper)
        {
            this.logger = logger;
            this.repository = repository;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get(int orderId)
        {
            try
            {
                var order = this.repository.GetOrderById(orderId);

                if (order != null)
                    return Ok(this.mapper.Map<IEnumerable<OrderItemViewModel>>(order.Items));
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Failed to get order by orderId [{orderId}]: {ex}");
            }

            return BadRequest($"Failed to get order by orderId [{orderId}]");
        }

        [HttpGet("{id}")]
        public IActionResult Get(int orderId, int id)
        {
            var order = this.repository.GetOrderById(orderId);

            if (order != null)
            {
                var item = order.Items.Where(i => i.Id == id).FirstOrDefault();
                if (item != null)
                    return Ok(this.mapper.Map<OrderItemViewModel>(item));
            }

            return NotFound();
        }
    }
}
