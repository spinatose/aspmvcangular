using AutoMapper;
using DutchTreat.Data;
using DutchTreat.Data.Entities;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DutchTreat.Controllers
{
    [Route("api/[Controller]")]
    [Authorize(AuthenticationSchemes=JwtBearerDefaults.AuthenticationScheme)]
    public class OrdersController : Controller
    {
        private readonly ILogger<OrdersController> logger;
        private readonly IDutchRepository repository;
        private readonly IMapper mapper;
        private readonly UserManager<StoreUser> userManager;

        public OrdersController(ILogger<OrdersController> logger, IDutchRepository repository, IMapper mapper, UserManager<StoreUser> userManager)
        {
            this.logger = logger;
            this.repository = repository;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        [HttpGet]
        public IActionResult Get(bool includeItems = true)
        {
            try
            {
                var username = User.Identity.Name;
                var results = this.repository.GetAllOrdersByUser(username, includeItems);

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
                var order = this.repository.GetOrderById(User.Identity.Name, id);

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
        public async Task<IActionResult> Post([FromBody] OrderViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var newOrder = this.mapper.Map<Order>(model);

                    if (newOrder.OrderDate == DateTime.MinValue)
                        newOrder.OrderDate = DateTime.Now;

                    // get user
                    var user = await this.userManager.FindByNameAsync(User.Identity.Name);
                    if (user != null)
                        newOrder.User = user;

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
