using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ThienASPMVC08032023.Models
{
    public class AppUser : IdentityUser
    {
        [Display(Name = "Name")]
        [MaxLength(20)]
        public string? Name { get; set; }

        [Column(TypeName = "nvarchar")]
        [StringLength(100)]
        public string? HomeAddress { get; set; }
    }
}
