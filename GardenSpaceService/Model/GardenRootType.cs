using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GardenSpaceService.Model
{
    public class GardenRootType
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; } 
        public int GardenSpaceId { get; set; }
        [ForeignKey("GardenSpaceId")]
        public GardenSpace GardenSpace { get; set; }
        public string Color { get; set; }
        public ICollection<GardenBranchType> GardenBranchTypes { get; set; }
    }
}
