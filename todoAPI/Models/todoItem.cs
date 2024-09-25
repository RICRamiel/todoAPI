using Microsoft.Build.Framework;

namespace todoAPI.Models
{
    public class todoItem
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool IsComplete { get; set; } = false;
    }
}
