using GardenSpaceService.Data;
using GardenSpaceService.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GardenSpaceService.Controllers
{
    [Route("/garden_root")]
    [ApiController]
    public class GardenRootTypeController : ControllerBase
    {
        private readonly GardenSpaceContext _context;
        public GardenRootTypeController(GardenSpaceContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public IActionResult GetBaseRootType(int id)
        {
            try
            {
                GardenRootType gardenRootType = _context.GardenRootType.FirstOrDefault(b => b.Id == id);
                return Ok(new { token = true, data = gardenRootType });
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                return Ok(new { token = false, data = "오류가 발생 했습니다." });
            }
        }

        [HttpGet("space_id={id}")]
        public IActionResult Load(int id)
        {
            try
            {
                return Ok(new { token = true, data = _context.GardenRootType.ToList() });
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                return Ok(new { token = false, data = "에러가 발생했습니다." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] GardenRootType gardenRootType)
        {
            try
            {
                if (CheckDuplicationName(gardenRootType) == false)
                {
                    return Ok(new { token = false, data = "이름이 중복 되었어요." });
                }

                _context.Add(gardenRootType);
                await _context.SaveChangesAsync();

                return Ok(new { token = true });
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                return Ok(new { token = false });
            }
        }
        [HttpPatch]
        public async Task<IActionResult> Update([FromForm] GardenRootType gardenRootType)
        {
            try
            {
                var updateValue = _context.GardenRootType.FirstOrDefault(b => b.Id == gardenRootType.Id);

                if (updateValue == null) { return Ok(new { token = false }); }

                updateValue.Name = gardenRootType.Name;
                updateValue.Description = gardenRootType.Description;
                updateValue.Color = gardenRootType.Color;

                _context.Update(updateValue);
                await _context.SaveChangesAsync();

                return Ok(new { token = true });
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                return Ok(new { token = false });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] GardenRootType removeValue)
        {
            try
            {
                GardenRootType gardenRootType = _context.GardenRootType
                                                    .Include(b => b.GardenBranchTypes)
                                                    .FirstOrDefault(b => b.Id == removeValue.Id);
                //BaseBrachType 삭제
                _context.RemoveRange(gardenRootType.GardenBranchTypes);
                await _context.SaveChangesAsync();

                //BaseRootType 삭제
                _context.Remove(gardenRootType);
                await _context.SaveChangesAsync();

                return Ok(new { token = true });
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                return Ok(new { token = false });
            }
        }

        /// <summary>
        /// 타입 이름 중복검사
        /// </summary>
        /// <param name="gardenRootType"></param>
        /// <returns></returns>
        private bool CheckDuplicationName(GardenRootType gardenRootType)
        {
            try
            {
                if (_context.GardenRootType.FirstOrDefault(b => b.Name == gardenRootType.Name) != null)
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                return false;
            }
        }

    }
}
