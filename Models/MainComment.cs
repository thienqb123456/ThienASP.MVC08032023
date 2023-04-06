using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ThienASPMVC08032023.Models
{
    public class MainComment : Comment
    {
        public int ClipId { get; set; }

        [Required]
        [ForeignKey("ClipId")]
        public Clip? Clip { get; set; }
        public List<SubComment>? subComments { get; set; }
    }
}
