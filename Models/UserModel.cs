using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace cat_API.Models
{
    [Index(nameof(UserName), IsUnique = true)]
    [Index(nameof(Email), IsUnique = true)]
    public class UserModel
    {
        public int Id { get; set; }

      
        public string UserName { get; set; }
        public string Password { get; set; }
        public string? Email { get; set; }
        public ICollection<CatModel> Cats { get; set; } = new List<CatModel>();

    }
}
