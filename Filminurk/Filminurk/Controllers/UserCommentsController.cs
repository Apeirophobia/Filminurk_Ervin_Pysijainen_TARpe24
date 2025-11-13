using Filminurk.ApplicationServices.Services;
using Filminurk.Core.Dto;
using Filminurk.Core.ServiceInterface;
using Filminurk.Data;
using Filminurk.Models.Comments;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace Filminurk.Controllers
{
    public class UserCommentsController : Controller
    {
        private readonly FilminurkTARpe24Context _context;
        private readonly IUserCommentsServices _userCommentServices;

        public UserCommentsController(FilminurkTARpe24Context context, IUserCommentsServices userCommentServices)
        {
            _context = context;
            _userCommentServices = userCommentServices;
        }

        public IActionResult Index()
        {
            var result = _context.UserComments
                .Select(
                c => new UserCommentsIndexViewModel 
                { 
                    CommentID = c.CommentID,
                    CommentBody = c.CommentBody,
                    IsHarmful = c.IsHarmful,
                    CommentCreatedAt = c.CommentCreatedAt,
                    // CommentModifiedAt = c.CommentModifiedAt

                }
                );

            return View(result);
        }

        [HttpGet]
        public IActionResult NewComment()
        {
            UserCommentCreateViewModel newcomment = new();
            return View(newcomment);
        }

        [HttpPost, ActionName("NewComment")]
        public async Task<IActionResult> UserCommentPost(UserCommentCreateViewModel newcommentVM)
        {
            newcommentVM.CommentUserID = "00000000-0000-0000-000000000000";

            if (ModelState.IsValid)
            {
                var dto = new UserCommentDTO()
                {
                    CommentID = (Guid)newcommentVM.CommentID,
                    CommentBody = newcommentVM.CommentBody,
                    CommentUserID = newcommentVM.CommentUserID,
                    CommentedScore = newcommentVM.CommentedScore,
                    CommentCreatedAt = newcommentVM.CommentCreatedAt,
                    CommentModifiedAt = newcommentVM.CommentModifiedAt,
                    IsHelpful = newcommentVM.IsHelpful,
                    IsHarmful = newcommentVM.IsHarmful,
                };
                var result = await _userCommentServices.NewComment(dto);

                if (result == null)
                {
                    return NotFound();
                }

                // TODO: erista ära, kas tegu on admini või kasutajaga, admin tagastab admin-comments-index
                // kasutaja aga vastava filmi juurde

                return RedirectToAction("Index");
                // return RedirectToAction("Details", "Movies", id)
            }
            return NotFound();

        }
    }
}
