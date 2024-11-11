using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tesvolt.dbmodels.app
{
    public class VacationPlanModel
    {
        [Key]
        public int Id { get; set; }

        // Foreign key
        [Display(Name ="Resource Name")]
        public int ResourceModelId { get; set; }

        // Navigation property
        //[ForeignKey(nameof(ResourceModelId))]
        public virtual ResourceModel? ResourceModel { get; set; }

        // Other properties for VacationPlanModel
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }
        public int NoOfDay { get; set; }

        public string StartDateFormatted => StartDate.ToShortDateString();
        public string EndDateFormatted => EndDate.ToShortDateString();
    }
}
