using ForkliftMqtt.Application.DTOs;
using ForkliftMqtt.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace ForkliftMqtt.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SensorsController : ControllerBase
    {
        private readonly ISensorApplicationService _sensorService;
        private readonly ILogger<SensorsController> _logger;

        public SensorsController(
            ISensorApplicationService sensorService,
            ILogger<SensorsController> logger)
        {
            _sensorService = sensorService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ForkliftSensorDto>>> GetAllSensors()
        {
            var sensors = await _sensorService.GetAllSensorsAsync();
            return Ok(sensors);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ForkliftSensorDto>> GetSensor(string id)
        {
            var sensor = await _sensorService.GetSensorByIdAsync(id);
            if (sensor == null)
            {
                return NotFound();
            }
            return Ok(sensor);
        }

        [HttpPost("publish")]
        public async Task<IActionResult> PublishReading(SensorReadingDto reading)
        {
            var result = await _sensorService.PublishSensorReadingAsync(reading);
            if (!result)
            {
                return BadRequest("Failed to publish sensor reading");
            }
            return Ok();
        }

        [HttpPost("{id}/monitor/start")]
        public async Task<IActionResult> StartMonitoring(string id)
        {
            var result = await _sensorService.StartMonitoringSensorAsync(id);
            if (!result)
            {
                return BadRequest("Failed to start monitoring sensor");
            }
            return Ok();
        }

        [HttpPost("{id}/monitor/stop")]
        public async Task<IActionResult> StopMonitoring(string id)
        {
            var result = await _sensorService.StopMonitoringSensorAsync(id);
            if (!result)
            {
                return BadRequest("Failed to stop monitoring sensor");
            }
            return Ok();
        }
    }
}
