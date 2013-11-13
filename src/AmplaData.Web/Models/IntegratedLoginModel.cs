using System.ComponentModel.DataAnnotations;

namespace AmplaData.Web.Models
{
    public class IntegratedLoginModel
    {
        [Required]
        [Display(Name = "Use Integrated Security")]
        public bool UseIntegratedSecurity { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}