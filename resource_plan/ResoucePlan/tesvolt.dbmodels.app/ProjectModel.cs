using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace tesvolt.dbmodels.app
{
    public class ProjectModel
    {

        public int Id { get; set; }



        [Required]
        [Display(Name = "Project Code")]
        public string ProjectCode { get; set; }

        [Required]
        [Display(Name = "Project Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Asset Name")]
        public string AssetName { get; set; }

        [Required]
        [Display(Name = "Product Owner")]
        public int PoNameId { get; set; }  // Store selected project owner ID

        public ResourceModel? PoName { get; set; }  // Reference to the selected project owner

        //[Required]
        //[Display(Name = "Accumulated HR")]
        //public int AccumulatedHrId { get; set; }  // Foreign key for Accumulated HR
        //public  ResourceModel? AccumulatedHr { get; set; }  // Navigation property


        [Required]
        [Display(Name = "Project Status")]
        public ProjectStatus ProjectStatus { get; set; }
    }
   
    public enum ProjectStatus
    {
        Planning_Phase,
        Development_phase
    }
}
