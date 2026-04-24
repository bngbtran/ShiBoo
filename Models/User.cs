// D:\ShiBoo\Models\User.cs
namespace ShiBoo.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = "123@abc"; 
        public string Role { get; set; } = "Member"; 
        public bool IsFirstLogin { get; set; } = true;
    }
}