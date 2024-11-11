using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tesvolt.dbmodels.app
{
    public class AttendanceDetailEntryModel
    {
       public int Id {  get; set; }
        public int ? ResourceNameId { get; set; }
        public ResourceModel? ResourceName { get; set; }
       
        public DateTime AttendanceDate { get; set; }
        public int AttendanceEntryId { get; set; }

        public AttendanceEntryModel AttendanceEntry { get; set; }
        public ProjectModel? ProjectName { get; set; }
        public int? ProjectNameId { get; set; }
        public TaskType TaskType { get; set; }

    }
}
