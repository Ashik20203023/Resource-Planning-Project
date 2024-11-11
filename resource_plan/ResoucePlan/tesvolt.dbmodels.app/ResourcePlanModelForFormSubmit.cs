using System.ComponentModel.DataAnnotations;

namespace tesvolt.dbmodels.app;

public class ResourcePlanModelForFormSubmit
{
    public int Id { get; set; }
        
    [Display(Name = "Resource Name ID")]
    public int? ResourceNameId { get; set; }

    [Display(Name = "Resource Name")]
    public ResourceModel ResourceName { get; set; }
        
    [Display(Name = "Project Name ID")]
    public int? ProjectNameId { get; set; }
        
    [Display(Name = "Module Name ID")]
    public int? ModuleNameId { get; set; }
}