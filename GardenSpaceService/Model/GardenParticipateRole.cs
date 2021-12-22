using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GardenSpaceService.Model
{
    public class GardenParticipateRole
    {
        [Key]
        public int Id { get; set; }
        public int Name { get; set; }
        public int GardenSpaceId { get; set; }
        public int GardenBranchId { get; set; }
        [ForeignKey("GardenSpaceId")]
        public GardenSpace GardenSpace { get; set; }
        [ForeignKey("GardenBranchId")]
        public GardenBranchType GardenBranchType { get; set; }
    }
}
