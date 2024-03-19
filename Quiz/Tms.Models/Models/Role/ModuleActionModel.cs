namespace Tms.Models
{
    public class ModuleActionModel
    {
        public int ModuleActionId { get; set; }
        public string Module { get; set; }
        public string Action { get; set; }
        public string Description { get; set; }
        public bool IsChecked { get; set; }
        public int? Group { get; set; }
    }
}
