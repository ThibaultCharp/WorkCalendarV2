using LogicLayer.Entitys;
using LogicLayer.Classes;
using LogicLayer.Services;
using Microsoft.AspNetCore.Mvc;
using DAL.Repos;

namespace WorkCalendarV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class PositionController : ControllerBase
    {
        private readonly PositionService positionService;

        private readonly ILogger<PositionController> _logger;


        public PositionController(ILogger<PositionController> logger)
        {
            _logger = logger;
            positionService = new PositionService(new PositionRepo());
        }

        [HttpGet("GetAllPositions")]
        public IActionResult GetAllPositions() 
        {
            List<Position> positions = positionService.GetAllActivitiesPerEmployee();
            return new JsonResult(positions);
        }

    }
}
