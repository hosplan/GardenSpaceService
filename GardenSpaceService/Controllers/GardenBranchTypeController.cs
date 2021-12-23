using GardenSpaceService.Data;
using GardenSpaceService.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GardenSpaceService.Controllers
{
    [Route("/garden_branch")]
    [ApiController]
    public class GardenBranchTypeController : ControllerBase
    {
        private readonly GardenSpaceContext _context;
        public GardenBranchTypeController(GardenSpaceContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public IActionResult GetBaseBranchType(int id)
        {
            try
            {
                GardenBranchType branchType = _context.GardenBranchType.FirstOrDefault(b => b.Id == id);
                return Ok(new { token = true, data = branchType });
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                return Ok(new { token = false, data = "오류가 발생 했습니다." });
            }
        }

        [HttpGet("root_type={id}")]
        public IActionResult Load(int id)
        {
            try
            {
                return Ok(new { token = true, data = _context.GardenBranchType.Where(b => b.RootTypeId == id).ToList() });
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                return Ok(new { token = false, data = "에러가 발생했습니다." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] GardenBranchType branchType)
        {
            try
            {
                if (CheckDuplicationName(branchType) == false)
                {
                    return Ok(new { token = false, data = "이름이 중복 되었어요." });
                }

                _context.Add(branchType);
                await _context.SaveChangesAsync();

                return Ok(new { token = true });
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                return Ok(new { token = false, data = "오류가 발생 했어요.. 잠시 후에 다시 시도해주세요." });
            }
        }


        [HttpPatch]
        public async Task<IActionResult> Update([FromForm] GardenBranchType branchType)
        {
            try
            {
                if (CheckDuplicationName(branchType) == false)
                {
                    return Ok(new { token = false, data = "이름이 중복 되었어요." });
                }

                GardenBranchType updateValue = _context.GardenBranchType.FirstOrDefault(b => b.Id == branchType.Id);

                if (updateValue == null) { return Ok(new { token = false }); }

                updateValue.Name = branchType.Name;
                updateValue.Description = branchType.Description;
                updateValue.Color = branchType.Color;

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
        public async Task<IActionResult> Delete([FromBody] GardenBranchType removeValue)
        {
            try
            {
                GardenBranchType branchType = _context.GardenBranchType.FirstOrDefault(b => b.Id == removeValue.Id);

                _context.Remove(branchType);
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
        /// <param name="branchType"></param>
        /// <returns></returns>
        private bool CheckDuplicationName(GardenBranchType branchType)
        {
            try
            {
                if (_context.GardenBranchType.FirstOrDefault(b => b.RootTypeId == branchType.RootTypeId
                                                                                 && b.Name == branchType.Name) != null)
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
