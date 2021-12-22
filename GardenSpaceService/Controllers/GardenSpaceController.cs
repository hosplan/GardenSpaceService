using GardenSpaceService.Data;
using GardenSpaceService.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace GardenSpaceService.Controllers
{
    [Route("/garden_space")]
    [ApiController]
    public class GardenSpaceController : ControllerBase
    {
        private readonly GardenSpaceContext _context;

        public GardenSpaceController(GardenSpaceContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] GardenSpace gardenSpace)
        {
            try
            {
               
                _context.Add(gardenSpace);
                await _context.SaveChangesAsync();
                return Ok(new { token = true });
            }
            catch(Exception ex)
            {
                string error = ex.Message;
                return Ok(new { token = true, data = "오류가 발생 했습니다."});
            }
        }
    }
}
