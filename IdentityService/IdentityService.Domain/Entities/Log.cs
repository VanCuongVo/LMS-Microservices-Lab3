

namespace IdentityService.Domain.Entities
{
    public class Log
    {
        public int Id { get; set; }
        public string Message { get; set; } = null!;
        public string? MessageTemplate { get; set; }
        public string Level { get; set; } = null!;
        public DateTime TimeStamp { get; set; }
        public string? Exception { get; set; }
        public string? Properties { get; set; } // JSON string

        public string? UserId { get; set; }
        public string? Email { get; set; }
    }
}