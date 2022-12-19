using Api.DTOs;
using Api.Models;
using Api.Repositories;
using Api.Services;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class LeaderboardController : ControllerBase
    {
        private readonly LeaderboardApiService _leaderboardApiService;
        private readonly ILeaderboardRepository _leaderboardRepository;
        private readonly IUserRepository _userRepository;
        public LeaderboardController(LeaderboardApiService leaderboardApiService,
            ILeaderboardRepository leaderboardRepository,
            IUserRepository userRepository)
        {
            _leaderboardApiService = leaderboardApiService;
            _leaderboardRepository = leaderboardRepository;
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var leaderboardUsers = await _leaderboardRepository.GetAllAsync();
            #region month validation
            var leaderboardUser = leaderboardUsers
               .FirstOrDefault(x => x.CreatedTime.Year == DateTime.Now.Year && x.CreatedTime.Month == DateTime.Now.Month);
            if (leaderboardUser != null)
                return BadRequest("You can only create a leaderboard once a month.");
            #endregion

            var points =  await _leaderboardApiService.GetPointsAsync();
            var leaderboard = points
                .Where(x => x.Approved)
                .GroupBy(x => x.User_Id.oid)
                .Select(x => new LeaderboardUser
                {
                    Point = x.Select(y => y.Point).Sum(),
                    UserId = x.Key,
                })
                .OrderByDescending(x => x.Point)
                .ToList();
            leaderboard[0].Prize = "First Prize";
            leaderboard[1].Prize = "Second Prize";
            leaderboard[2].Prize = "Third Prize";
            var size = leaderboard.Count();
            var willUpdateUsers = new List<User>();
            var willAddUsers = new List<User>();
            var users = await _userRepository.GetAllAsync();
            for (int i= 0; i< size; i++)
            {
                leaderboard[i].Rank= i + 1;
                leaderboard[i].CreatedTime = DateTime.Now;
                if (i >= 3 && i <= 100)
                    leaderboard[i].Prize = "25$";
                if (i > 100 && i < 1000)
                    leaderboard[i].Prize = "Consolation prize";
                #region user add or update
                var existUser = users.FirstOrDefault(x => x.Id == leaderboard[i].UserId);
                var newUser = new User();
                newUser.TotalPoint = leaderboard[i].Point;
                newUser.Id = leaderboard[i].UserId;
                if (existUser != null) {
                    if (leaderboard[i].Prize != null) 
                    {
                        existUser.Prizes.Add(leaderboard[i].Prize);
                        newUser.Prizes = existUser.Prizes.ToList();
                    }
                    willUpdateUsers.Add(newUser);
                }
                else 
                {
                    if (leaderboard[i].Prize != null)
                    {
                        newUser.Prizes.Add(leaderboard[i].Prize);
                    }
                    willAddUsers.Add(newUser);
                }
                #endregion
            }
            if(willUpdateUsers.Any())
                await _userRepository.UpdateManyAsync(willUpdateUsers);
            if (willAddUsers.Any())
                await _userRepository.AddRangeAsync(willAddUsers);
            await _leaderboardRepository.AddRangeAsync(leaderboard);

            return Ok("Leaderboard created.");
        }

        [HttpGet]
        public async Task<IActionResult> List([FromQuery]LeaderboardFilterDto filter)
        {
            var leaderboardUsers = await _leaderboardRepository.GetAllAsync();
            if (filter.LeaderCreatedMonth != null)
                leaderboardUsers = leaderboardUsers
                    .Where(x => filter.LeaderCreatedMonth?.Year == DateTime.Now.Year && filter.LeaderCreatedMonth?.Month == DateTime.Now.Month)
                    .ToList();
            if (filter.UserId != null)
                leaderboardUsers = leaderboardUsers
                    .Where(x => x.UserId == filter.UserId)
                    .ToList();
            var newLeaderboardDtoList = new List<LeaderboardDto>();
            var users = await _userRepository.GetAllAsync();
            foreach (var item in leaderboardUsers)
            {
                var newLeaderboardDto = new LeaderboardDto();
                newLeaderboardDto.UserId = item.UserId;
                newLeaderboardDto.Rank = item.Rank;
                var user = users.FirstOrDefault(x => x.Id == item.UserId);
                newLeaderboardDto.TotalPoint = user.TotalPoint;
                newLeaderboardDtoList.Add(newLeaderboardDto);
            }
            return Ok(newLeaderboardDtoList);
           
        }

        [HttpGet]
        public async Task<IActionResult> Users([FromQuery] string userId)
        {
            if(userId == null)
                 return Ok(await _userRepository.GetAllAsync());
            var user =  await _userRepository.GetByIdAsync(userId);
            if(user == null)
                return NotFound();
            return Ok(user);
        }
    }
}
