using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace tesvolt.dbmodels.app
{
    public class ResourceModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public ResourceCategory Category { get; set; }

        [Required]
        public ResourceRole Role { get; set; }

        public string Skills { get; set; }

        // Change this line to use ICollection<VacationPlanModel>
        public virtual ICollection<VacationPlanModel> VacationPlans { get; set; } = new List<VacationPlanModel>();
    }

    public enum ResourceCategory
    {
        Internal,
        External
    }

    public enum ResourceRole
    {
        Developer,
        Designer,
        ProductOwner,
        Analyst,
        Tester,
        HR
    }
}
