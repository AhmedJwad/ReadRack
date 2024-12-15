using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReadRack.Backend.Data;
using ReadRack.Backend.Helpers;
using ReadRack.Backend.UnitsOfWork.Interfaces;
using ReadRack.Shared.DTOs;
using ReadRack.Shared.Entites;
using ReadRack.Shared.Responses;

namespace ReadRack.Backend.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class CollegesController : GenericController<College>
    {
        private readonly IcollegesUnitOfWork _icollegesUnitOfWork;
       

        public CollegesController(IGenericUnitOfWork<College> genericUnitOfWork ,
            IcollegesUnitOfWork icollegesUnitOfWork ,
            IFileStorage fileStorage, DataContext context) : base(genericUnitOfWork)
        {
            _icollegesUnitOfWork = icollegesUnitOfWork;
           
        }
        [HttpPost("full")]
        public async Task<IActionResult> PostFullAsync(College college)
        {
            var action = await _icollegesUnitOfWork.AddFullAsync(college);
            if (action.WasSuccess)
            {
                return Ok(action.Result);
            }
            return NotFound(action.Message);
        }

        [HttpPut("full")]
        public async Task<IActionResult> PutFullAsync(College college)
        {
            var action = await _icollegesUnitOfWork.PutAsync(college);
            if (action.WasSuccess)
            {
                return Ok(action.Result);
            }
            return NotFound(action.Message);
        }

        [HttpGet("recordsNumber")]
        public override async Task<IActionResult> GetRecordsNumberAsync([FromQuery] PaginationDTO pagination)
        {
            var response = await _icollegesUnitOfWork.GetRecordsNumberAsync(pagination);
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return BadRequest();
        }

        [HttpGet("combo")]
        public async Task<IActionResult> GetComboAsync()
        {
            var response = await _icollegesUnitOfWork.GetComboAsync();
            return Ok(response);
        }

        [HttpGet("full")]
        public override async Task<IActionResult> GetAsync()
        {
            var response = await _icollegesUnitOfWork.GetAsync();
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return BadRequest();
        }


        [HttpGet]
        public override async Task<IActionResult> GetAsync(PaginationDTO pagination)
        {
            var response = await _icollegesUnitOfWork.GetAsync(pagination);
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return BadRequest();
        }

        [HttpGet("{id}")]
        public override async Task<IActionResult> GetAsync(int id)
        {
            var response = await _icollegesUnitOfWork.GetAsync(id);
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return NotFound(response.Message);
        }

        [HttpGet("totalPages")]
        public override async Task<IActionResult> GetPagesAsync([FromQuery] PaginationDTO pagination)
        {
            var action = await _icollegesUnitOfWork.GetTotalPagesAsync(pagination);
            if (action.WasSuccess)
            {
                return Ok(action.Result);
            }
            return BadRequest();
        }
    }
}
