using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tesvolt.dbmodels.app
{


    public class AttendanceEntryModel
    {
        public int Id { get; set; }
       
        [Display(Name = "Resource Name Id")]
        public int? ResourceNameId { get; set; }
      
        [Display(Name = "Resource Name")]
        public ResourceModel? ResourceName { get; set; }
        [Required]
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]  // Ensure the Date only is used
        public DateTime StartDate { get; set; }

        [Required]
        [Display(Name = "End Date")]
        [DataType(DataType.Date)]

        public DateTime EndDate { get; set; }
        [Display(Name = "Project Name")]
        public ProjectModel? ProjectName { get; set; }

        [Display(Name = "Peoject Name Id")]
        public int? ProjectNameId { get; set; }
        [Required]
        [Display(Name = "Task Type")]
        public TaskType TaskType { get; set; }
        public string StartDateFormatted => StartDate.ToShortDateString();
        public string EndDateFormatted => EndDate.ToShortDateString();
    }



    public enum TaskType
    {
        NewFeature,
        Fixation,
        Bug
    }

}