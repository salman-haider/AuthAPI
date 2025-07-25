using API.DTO;
using API.Model;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("API/Comment")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public CommentController(ApplicationDBContext context)
        {
            _context = context;

        }

        [HttpGet]
        public IActionResult GetAll()
        {

            var Comments = _context.Comments.ToList();
            return Ok(Comments);
        }

        [HttpGet ("{id}")]
        public IActionResult GetById([FromRoute] int id)
        {
            var comment = _context.Comments.Find(id);

            if (comment == null)
            {
                return NotFound();
            }
            return Ok(comment);
        }

        [HttpPost]
        public IActionResult Create([FromBody] CommentsDTO comments)
        {
            var comment = new Comment { 
            Title = comments.Title,
            Content = comments.Content,
            CreatedOn = comments.CreatedOn,
            StockId = comments.StockId
            };
            
            _context.Comments.Add(comment);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetById), new { id = comment.Id }, comment);
        }

        [HttpPut ("{id}")]
        public IActionResult Update(int id, [FromBody] CommentsDTO comments)
        {
            var comment = _context.Comments.Find(id);
            if (comment == null)
            {
                return NotFound();
            }

            comment.Title = comments.Title;
            comment.Content = comments.Content;
            _context.SaveChanges();
            return Ok("Comments updated sucessfully");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id) 
        {
            var comment = _context.Comments.Find(id);

            if (comment == null)
            {
                return NotFound();
            }

            _context.Comments.Remove(comment);
            _context.SaveChanges();
            return Ok("Comment deleted sucessfully!");
        }

    }
}
