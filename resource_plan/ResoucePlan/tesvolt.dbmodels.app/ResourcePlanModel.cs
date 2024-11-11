using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tesvolt.dbmodels.app
{
    public class ResourcePlanModel
    {
        public int Id { get; set; }
        [Display(Name = "Resource Name ID")]
        public int? ResourceNameId { get; set; }

        [Display(Name = "Resource Name")]
        public ResourceModel ResourceName { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        [Display(Name = "Project Name ID")]
        public int? ProjectNameId { get; set; }
        [Required]
        [Display(Name = "Project Name")]
        public ProjectModel ProjectName { get; set; }

        public double Percentage { get; set; }
        
        public int? ModuleNameId { get; set; }

    }
    
    public enum MonthOfTheYear
    {
        January = 1,
        February = 2,
        March = 3,
        April = 4,
        May = 5,
        June = 6,
        July = 7,
        August = 8,
        September = 9,
        October = 10,
        November = 11,
        December = 12


    }
}
