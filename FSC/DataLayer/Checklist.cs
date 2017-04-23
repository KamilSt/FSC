using FSC.Models;
using System.ComponentModel.DataAnnotations;

namespace FSC.DataLayer
{
    public class Checklist
    {
        [Required]
        public int Id { get; set; }

        public int ParentId { get; set; }

        [Required]
        public string Description { get; set; }

        public bool IsCompleted { get; set; }

        public ApplicationUser User { get; set; }

        [Required]
        public string UserId { get; set; }
    }
}