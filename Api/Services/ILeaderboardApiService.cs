using Api.DTOs;

namespace Api.Services
{
    public interface ILeaderboardApiService
    {
        Task<List<PointDto>> GetPointsAsync();
    }
}
