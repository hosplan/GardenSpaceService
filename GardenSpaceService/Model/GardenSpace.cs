using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GardenSpaceService.Model
{
    public class GardenSpace
    {
        [Key]
        public int Id { get; set; }
        public int CreatorId { get; set; }
        public string SpaceName { get; set; }
        public string Description { get; set; }
        public int BranchId { get; set; }
        public string SpaceTypeName { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsPrivate { get; set; }
        public bool OnlyInvite { get; set; }
        public Nullable<DateTime> PlanStartDate { get; set; }
        public Nullable<DateTime> PlanEndDate { get; set; }
        public Nullable<DateTime> StartDate { get; set; }
        public Nullable<DateTime> EndDate { get; set; }

        public ICollection<GardenSpaceUserMap> GardenSpaceUserMaps { get; set; }
    }
}

