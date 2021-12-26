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
    [Route("/garden_space")]
    [ApiController]
    public class GardenSpaceController : ControllerBase
    {
        private readonly GardenSpaceContext _context;
        private readonly IJWTService _jwtService;

        public GardenSpaceController(GardenSpaceContext context, IJWTService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] GardenSpace gardenSpace)
        {
            try
            {
                gardenSpace.CreatorId = GetLoginUserId();
                gardenSpace.IsPrivate = true;
                gardenSpace.OnlyInvite = true;
                gardenSpace.CreateDate = DateTime.Now;
                gardenSpace.Color = "#FFF";
                _context.Add(gardenSpace);
                await _context.SaveChangesAsync();

                //프로젝트 생성시 기본 템플릿 타입 생성 - 추후 코드 분기
                List<BaseRootType> baseRootTypes = _context.BaseRootType.ToList();

                List<GardenBranchType> branchTypes = new List<GardenBranchType>();
    
                foreach(BaseRootType baseRootType in baseRootTypes)
                {
                    GardenRootType rootType = new GardenRootType();

                    rootType.Name = baseRootType.Name;
                    rootType.Description = baseRootType.Description;
                    rootType.GardenSpaceId = gardenSpace.Id;

                    _context.Add(rootType);
                    _context.SaveChanges();

                    var baseBranchTypes = _context.BaseBranchType.Where(b => b.BaseRootTypeId == baseRootType.RootId);

                    foreach(var basebranchType in baseBranchTypes)
                    {
                        GardenBranchType branchType = new GardenBranchType();

                        branchType.Name = basebranchType.Name;
                        branchType.Description = basebranchType.Description;
                        branchType.Color = basebranchType.Color;
                        branchType.RootTypeId = rootType.Id;
                        branchTypes.Add(branchType);
                    }
                }
                _context.AddRange(branchTypes);
                await _context.SaveChangesAsync();

                


                return Ok(new { token = true, data = gardenSpace.Id });
            }
            catch(Exception ex)
            {
                string error = ex.Message;
                return Ok(new { token = true, data = "오류가 발생 했습니다.."});
            }
        }


        


        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                GardenSpace gardenSpace = _context.GardenSpace.FirstOrDefault(g => g.Id == id);
                if(gardenSpace == null)
                {
                    return Ok(new { token = false });
                }
                object obj = new
                {
                    id = gardenSpace.Id,
                    name = gardenSpace.SpaceName,
                    creator = gardenSpace.CreatorId,
                    description = gardenSpace.Description,
                    createDate = gardenSpace.CreateDate.ToString("yyyy-MM-dd"),
                    spaceTypeName = gardenSpace.SpaceTypeName,
                    isPrivate = gardenSpace.IsPrivate,
                    onlyInvite = gardenSpace.OnlyInvite,
                    color = gardenSpace.Color,
                    planStartDate = gardenSpace.PlanStartDate.HasValue ? gardenSpace.PlanStartDate.Value.ToString("yyyy-MM-dd") : "",
                    planEndDate = gardenSpace.PlanEndDate.HasValue ? gardenSpace.PlanEndDate.Value.ToString("yyyy-MM-dd") : "",
                    startDate = gardenSpace.StartDate.HasValue ? gardenSpace.StartDate.Value.ToString("yyyy-MM-dd") : "",
                    endDate = gardenSpace.EndDate.HasValue ? gardenSpace.EndDate.Value.ToString("yyyy-MM-dd") : ""
                };
                
                return Ok(new { token = true, data = obj });
            }
            catch(Exception ex)
            {
                string error = ex.Message;
                return Ok(new { token = false, data  = "오류가 발생 했습니다.." });
            }
        }

        [HttpPatch]
        public async Task<IActionResult> Patch([FromForm] GardenSpace gardenSpace)
        {
            try
            {
                _context.Update(gardenSpace);
                await _context.SaveChangesAsync();

                return Ok(new { token = true });
            }
            catch(Exception ex)
            {
                string error = ex.Message;
                return Ok(new { token = false });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] GardenSpace gardenSpace)
        {
            try
            {
                GardenSpace removeValue = _context.GardenSpace
                                                    .Include(g => g.GardenSpaceUserMaps)
                                                    .FirstOrDefault(g => g.Id == gardenSpace.Id);
                
                if(removeValue != null)
                {
                    _context.RemoveRange(removeValue.GardenSpaceUserMaps);
                    await _context.SaveChangesAsync();

                    _context.Remove(removeValue);
                    await _context.SaveChangesAsync();
                }

                return Ok(new { token = true });
            }
            catch(Exception ex)
            {
                string error = ex.Message;
                return Ok(new { token = false });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Load()
        {
            List<object> values = new List<object>();
            try
            {
                int loginUserId = GetLoginUserId();

                //자신이 master 인경우
                List<GardenSpace> gardenSpaces = await _context.GardenSpace.Where(g => g.CreatorId == loginUserId)
                                                                           .Include(z => z.GardenSpaceUserMaps)
                                                                           .AsNoTracking()
                                                                           .ToListAsync();

                //자신이 속해있는 gardenSpace
                List<GardenSpaceUserMap> maps = await _context.GardenSpaceUserMap
                                                              .Include(g => g.GardenSpace)
                                                              .Where(g => g.UserId == loginUserId)
                                                              .AsNoTracking()
                                                              .ToListAsync();

                foreach(GardenSpaceUserMap map in maps)
                {
                    gardenSpaces.Add(map.GardenSpace);
                }

                return Ok(new { token = true, data = gardenSpaces });
            }
            catch(Exception ex)
            {
                string error = ex.Message;
                return Ok(new { token = false, data = "오류가 발생 했습니다.. 잠시후에 다시 시도해주세요." });
            }
        }
        /// <summary>
        /// 로그인 유저의 id 가져오기
        /// </summary>
        /// <returns></returns>
        private int GetLoginUserId()
        {
            try
            {
                string jwt = GetJWT();
                if(string.IsNullOrEmpty(jwt)) { return 0; }
                return _jwtService.GetId(_jwtService.GetClaimList(jwt));
            }
            catch(Exception ex)
            {
                string error = ex.Message;
                throw;
            }
        }

        /// <summary>
        /// jwt 토큰값 가져오기
        /// </summary>
        /// <returns></returns>
        private string GetJWT()
        {
            return HttpContext.Request.Headers["Authorization"].ToString();
        }
    }
}
