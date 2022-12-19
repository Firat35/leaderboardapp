namespace Api.DTOs
{
    public class LeaderboardFilterDto
    {
        /// <example>2022-12</example>
        public DateTime? LeaderCreatedMonth { get; set; }
        public string UserId { get; set; }
    }
}
