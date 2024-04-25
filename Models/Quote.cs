using System.ComponentModel.DataAnnotations;

namespace veneer.Models
{
    public class Quote
    {
        [Required]
        [StringLength(500, ErrorMessage = "Max length is 500!")]
        public string Text { get; set; }
    }
}
