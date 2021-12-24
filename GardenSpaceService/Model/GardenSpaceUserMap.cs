using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GardenSpaceService.Model
{
    public class GardenSpaceUserMap
    {
        [Key]
        public int Id { get; set; }
        public int GardenSpaceId { get; set; }
        public int UserId { get; set; }

        public int BranchId { get; set; }
        public DateTime ParticiDate { get; set; }

        [ForeignKey("GardenSpaceId")]
        public GardenSpace GardenSpace { get; set; }
    }
}
