using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GardenSpaceService.Model
{
    public class GardenBranchType
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int RootTypeId { get; set; }        
        public string Color { get; set; }

        [ForeignKey("RootTypeId")]
        public GardenRootType RootType { get; set; }

    }
}
