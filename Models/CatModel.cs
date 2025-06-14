using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace cat_API.Models
{
    public class CatModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; } = string.Empty;
        public string Age { get; set; } 
        public string Description { get; set; } = string.Empty;
        public UserModel User { get; set; } 


    }
}
