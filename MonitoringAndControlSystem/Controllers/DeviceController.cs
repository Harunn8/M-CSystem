using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Services;
using Application.Services;
namespace MonitoringAndControlSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommunicationController : ControllerBase
    {
        private readonly CommunicationService _communicationService;

        public CommunicationController(CommunicationService communicationService)
        {
            _communicationService = communicationService;
        }

        [HttpGet("start")]
        public IActionResult StartCommunication()
        {
            try
            {
                _communicationService.StartCommunication();
                return Ok("Communication started.");
            }
            catch(Exception ex) 
            {
                return BadRequest(ex.Message);
            }
           
        }

        [HttpPost("Set-Value")]
        public IActionResult SetValue([FromBody] SetValueRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.IpAddress) || string.IsNullOrEmpty(request.Oid) || string.IsNullOrEmpty(request.Value))
            {
                return BadRequest("Invalid request data");
            }

            try
            {
                _communicationService.SetValue(request.IpAddress, request.Oid, request.Value);
                return Ok("Value set successfully.");
            }
            catch (Exception ex)
            {
                
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("device-data")]
        public IActionResult GetDeviceData()
        {
            var data = _communicationService.GetDeviceData();
            if (data == null)
            {
                return NotFound("No data found from devices.");
            }
            return Ok(data);
        }

        #region SetValueRequest
        public class SetValueRequest
        {
            public string IpAddress { get; set; }
            public string Oid { get; set; }
            public string Value { get; set; }
        }
        #endregion
    }
}
