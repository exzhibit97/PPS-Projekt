using Movies.Shared;
using Movies.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Movies.Domain.Models
{
    public class BoardPost : BaseEntity, IFeed
    {
        [Required]
        [MaxLength(60)]
        public string Title { get; set; }
        [Required]
        [MaxLength(500)]
        public string Content { get; set; }
        public ApplicationUser User { get; set; }
        public string UserID { get; set; }
        public List<Reply> Replies{ get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
