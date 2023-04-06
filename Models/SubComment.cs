using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ThienASPMVC08032023.Models
{
    public class SubComment : Comment
    {
        public int MainCommentId { get; set; }

        [Required]
        [ForeignKey("MainCommentId")]
        public MainComment? MainComment { get; set; }
        
    }
}
