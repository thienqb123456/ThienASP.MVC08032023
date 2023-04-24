using System.ComponentModel.DataAnnotations;
using ThienASPMVC08032023.Models;

namespace ThienASPMVC08032023.Areas.Admin.Models.User
{
    public class SetRoleUserModel
    {
        public AppUser? User { get; set; }

        public IList<string>? RolesUser { get; set; }


    }
}
