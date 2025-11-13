namespace Filminurk.Models.Comments
{
    public class UserCommentCreateViewModel
    {
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
