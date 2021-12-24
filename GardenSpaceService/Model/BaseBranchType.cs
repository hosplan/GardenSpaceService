using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GardenWeb.Models
{
    public class BaseBranchType
    {
        [Key]
        public int Id { get; set; }
        public int BranchId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int BaseRootTypeId { get; set; }
        public string Color { get; set; }

    }
}
