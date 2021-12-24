using GardenSpaceService.Data;
using GardenSpaceService.Model;
using GardenSpaceService.Services;
using GardenWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GardenSpaceService.Controllers
{
    
    [ApiController]
    public class GardenBaseTypeController : ControllerBase
    {
        private readonly GardenSpaceContext _context;

        public GardenBaseTypeController(GardenSpaceContext context)
        {
            _context = context;
        }

        [Route("/garden_route_type")]
        [HttpPost]
        public async Task<IActionResult> GenerateRootType([FromForm] BaseRootType value)
        {
            try
            {    
                _context.Add(value);
                await _context.SaveChangesAsync();

                return Ok(new { token = true });
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                return Ok(new { token = true , data= "타입 생성도중 오류가 발생 했네요."});
            }
        }

        [Route("/garden_route_type")]
        [HttpPatch]
        public async Task<IActionResult> UpdateRootType([FromForm] BaseRootType value)
        {
            try
            {
                BaseRootType updateValue = _context.BaseRootType.FirstOrDefault(b => b.RootId == value.RootId);

                updateValue.Name = value.Name;
                updateValue.Description = value.Description;
                
                _context.Update(updateValue);
                await _context.SaveChangesAsync();

                return Ok(new { token = true });
            }
            catch(Exception ex)
            {
                string error = ex.Message;
                return Ok(new { token = true, data = "타입 수정도중 오류가 발생 했네요." });
            }
        }

        [Route("/garden_route_type")]
        [HttpDelete]
        public async Task<IActionResult> RemoveRootType([FromBody] BaseRootType value)
        {
            try
            {
                List<BaseBranchType> removeBranchValue = _context.BaseBranchType.Where(b => b.BaseRootTypeId == value.RootId).ToList();               
                BaseRootType removeValue = _context.BaseRootType.FirstOrDefault(b => b.RootId == value.RootId);

                if(removeBranchValue.Count() != 0)
                {
                    _context.RemoveRange(removeBranchValue);
                    await _context.SaveChangesAsync();
                }


                _context.Remove(removeValue);
                await _context.SaveChangesAsync();

                return Ok(new { token = true }); 
            }
            catch(Exception ex)
            {
                string error = ex.Message;
                return Ok(new { token = true, data = "타입 삭제도중 오류가 발생 했네요." });
            }
        }

        [Route("/garden_branch_type")]
        [HttpPost]
        public async Task<IActionResult> GenerateBranchType([FromForm] BaseBranchType value)
        {
            try
            {
                _context.Add(value);
                await _context.SaveChangesAsync();

                return Ok(new { token = true });
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                return Ok(new { token = true, data = "타입 생성도중 오류가 발생 했네요." });
            }
        }


        [Route("/garden_branch_type")]
        [HttpPatch]
        public async Task<IActionResult> UpdateRootType([FromForm] BaseBranchType value)
        {
            try
            {
                BaseBranchType updateValue = _context.BaseBranchType.FirstOrDefault(b => b.BranchId == value.BranchId);

                updateValue.Name = value.Name;
                updateValue.Description = value.Description;
                updateValue.Color = value.Color;

                _context.Update(updateValue);
                await _context.SaveChangesAsync();

                return Ok(new { token = true });
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                return Ok(new { token = true, data = "타입 수정도중 오류가 발생 했네요." });
            }
        }


        [Route("/garden_branch_type")]
        [HttpDelete]
        public async Task<IActionResult> RemoveBranchType([FromBody] BaseBranchType value)
        {
            try
            {

                BaseBranchType removeValue = _context.BaseBranchType.FirstOrDefault(b => b.BranchId == value.BranchId);

                _context.Remove(removeValue);
                await _context.SaveChangesAsync();

                return Ok(new { token = true });
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                return Ok(new { token = true, data = "타입 삭제도중 오류가 발생 했네요." });
            }
        }

    }
}
