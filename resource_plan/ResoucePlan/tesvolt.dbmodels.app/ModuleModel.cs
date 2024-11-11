using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tesvolt.dbmodels.app
{
    public class ModuleModel
    {
        public int Id { get; set; }

        public string Modules { get; set; }

        public int NameId { get; set; } // Foreign key

        [Display(Name = "Project Name")]
        public ProjectModel? Name { get; set; } // Navigation property

        public DateTime WorkStartDate { get; set; }

        public DateTime WorkEndDate { get; set; }

        public int NoOfDaysNeeded { get; set; }
    }
}
