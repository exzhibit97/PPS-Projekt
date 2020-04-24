using Movies.Shared;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Movies.Domain.Models
{
    public class Reply : BaseEntity
    {
        public int RootPostId { get; set; }
        [ForeignKey("RootPostId")]
        public BoardPost RootPost { get; set; }
        public ApplicationUser User { get; set; }
        [Required]
        [MaxLength(500)]
        public string Content { get; set; }
        public string UserID { get; set; }
        public List<Reply> Replies { get; set; }
    }
}