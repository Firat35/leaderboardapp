using Api.Models;

using Microsoft.Extensions.Configuration;

using MongoDB.Driver;

namespace Api.Repositories
{
    public class LeaderboardRepository : ILeaderboardRepository
    {
        private readonly IMongoCollection<LeaderboardUser> _leaderboardUserCollection;
        public LeaderboardRepository(IMongoDatabase mongoDatabase)
        {
            _leaderboardUserCollection = mongoDatabase.GetCollection<LeaderboardUser>("leaderboard");
        }

        public async Task<List<LeaderboardUser>> GetAllAsync()
        {
            return await _leaderboardUserCollection.Find(_ => true).ToListAsync(); ;
        }
        public async Task AddRangeAsync(List<LeaderboardUser> leaderboard)
        {
            await _leaderboardUserCollection.InsertManyAsync(leaderboard);
        }

    }
}
