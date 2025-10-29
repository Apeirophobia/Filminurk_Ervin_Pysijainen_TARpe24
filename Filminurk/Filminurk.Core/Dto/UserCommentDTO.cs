using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filminurk.Core.Dto
{
    public class UserCommentDTO
    {
        [Key]
        public Guid CommentID { get; set; }
        public string? CommentUserID { get; set; }
        public string CommentBody { get; set; }
        public int CommentedScore { get; set; }
        public int IsHelpful { get; set; } // Like
        public int IsHarmful { get; set; } // Dislike

        // Andmebaasi jaoks vajalikud andmed
        public DateTime CommentCreatedAt { get; set; }
        public DateTime CommentModifiedAt { get; set; }
        public DateTime? CommentDeletedAt { get; set; }
    }
}
