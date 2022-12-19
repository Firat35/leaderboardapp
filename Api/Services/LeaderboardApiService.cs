using Api.DTOs;

using Microsoft.AspNetCore.Http;
using MongoDB.Bson;

using System.Net.Http;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Api.Services
{
    public class LeaderboardApiService
    {
        private readonly HttpClient _httpClient;

        public LeaderboardApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<PointDto>> GetPointsAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<PointDto>>("testcase/points.json");

            return response;
        }
    }
}
