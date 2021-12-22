using System;
using System.ComponentModel.DataAnnotations;

namespace GardenSpaceService.Model
{
    public class GardenSpace
    {
        [Key]
        public int Id { get; set; }
        public int CreatorId { get; set; }
        public string SpaceName { get; set; }
        public string Description { get; set; }
        public string BranchId { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsPrivate { get; set; }
        public bool OnlyInvite { get; set; }
        public Nullable<DateTime> PlanStartDate { get; set; }
        public Nullable<DateTime> PlanEndDate { get; set; }
        public Nullable<DateTime> StartDate { get; set; }
        public Nullable<DateTime> EndDate { get; set; }
    }
}

