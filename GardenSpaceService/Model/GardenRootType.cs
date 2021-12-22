using System.ComponentModel.DataAnnotations;

namespace GardenSpaceService.Model
{
    public class GardenRootType
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; } 
    }
}
