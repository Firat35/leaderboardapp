using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Api.Models;

namespace Api.Repositories
{
    public interface ILeaderboardRepository
    {

        Task<List<LeaderboardUser>> GetAllAsync();

        Task AddRangeAsync(List<LeaderboardUser> leaderboard);
    }
}
