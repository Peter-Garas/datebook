

using ApplicationAPI.Data;
using ApplicationAPI.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationAPI.Controllers
{
    public class BugController : BaseApiController
    {
        private readonly DataContext _db;
        public BugController(DataContext db)
        {
            _db = db;
        }
        [Authorize]
        [HttpGet("auth")]
        public ActionResult<string> GetSecrets() 
        {
            return "secrect text";
        } 
        
        [HttpGet("not-found")]
        public ActionResult<AppUser> GetNotFound() 
        {
            var item = _db.Users.Find(-1);
            if (item == null) 
                return NotFound();
            return item;
        } 
        [HttpGet("server-error")]
        public ActionResult<string> GetServerError() 
        {
            var item = _db.Users.Find(-1);
            var itemToReturn = item.ToString();
            return itemToReturn;
        } 
        [HttpGet("bad-request")]
        public ActionResult<string> GetBadRequest() 
        {
            return BadRequest("This is a bad request");
        } 
    }
}