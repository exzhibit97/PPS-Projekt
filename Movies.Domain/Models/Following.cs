using Movies.Shared;
using Movies.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Movies.Domain.Models
{
    public class Following : BaseEntity, IFeed
    {
        public string FollowedUserId { get; set; }
        [ForeignKey("FollowedUserId")]
        public ApplicationUser FollowedUser { get; set; }
        public string FollowingUserId { get; set; }
        [ForeignKey("FollowingUserId")]
        public ApplicationUser FollowingUser { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime EndedAt { get; set; }
        public bool IsActive { get; set; }
    }
}
