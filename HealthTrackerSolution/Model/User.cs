using System.Text.Json.Serialization;

namespace HealthTrackerSolution.Model
{
    public class User
    {
        public int Id { get; set; } // Primary key, auto-incremented

        public string Name { get; set; } // Assuming this is an integer ID for a name, consider changing to string if it's a name
        public long PhoneNumber { get; set; }
        public string Password { get; set; }
        public bool IsSuperUser { get; set; }
    }
}
