using Domain.Entities;
using Domain.ViewModels;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BClouderChallenge.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BClouderUserController : Controller
    {
        private BClouderDbContext _context;

        public BClouderUserController(BClouderDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        [HttpGet]
        public ActionResult<List<BClouderUser>> Get()
        {
            
            List<BClouderUser> allUsers = new List<BClouderUser>(); //getting all tasks in database
            allUsers = _context.Users.Where(x => x.Active && !x.IsDeleted).Include(x => x.Tasks).ToList();
            return Ok(allUsers);
        }

        [HttpPost]
        public ActionResult<List<BClouderUser>> Add(UserViewModel user)
        {
            if (user != null && user.Name != null && user.Email != null && user.DateOfBirth != null)
            {
                BClouderUser userToAdd = new BClouderUser();
                userToAdd.Name = user.Name;

                //insert email validation
                if (!user.Email.Contains("@") || !user.Email.Contains(".com"))
                {
                    return BadRequest("The email is in an incorrect format!");
                }
                userToAdd.Email = user.Email;

                userToAdd.DateOfBirth = user.DateOfBirth.Value;

                userToAdd.Created = DateTime.Now;
                userToAdd.Active = true;
                userToAdd.IsDeleted = false;
                userToAdd.Deleted = null;
                userToAdd.Available = true;

                var users = _context.Users.Add(userToAdd);
                _context.SaveChanges();

                return Ok(users.Entity);
            }
            else
            {
                return BadRequest("The user can't be null!");
            }
            
        }


        [HttpPut]
        public IActionResult Update(int id, UserViewModel partialUpdateUser)
        {
            var user = _context.Users.FirstOrDefault(u => u.ID == id);
            if (user != null)
            {
                // refresh only the specified features
                if (partialUpdateUser.Name != null)
                    user.Name = partialUpdateUser.Name;

                if (!user.Email.Contains("@") && !user.Email.Contains(".com"))
                {
                    return BadRequest("The email is in an incorrect format!");
                }

                if (partialUpdateUser.Email != null)
                    user.Email = partialUpdateUser.Email;

                if (partialUpdateUser.DateOfBirth != null)
                    user.DateOfBirth = partialUpdateUser.DateOfBirth.Value;
                _context.SaveChanges();
                return Ok("The user has been changed successfully!");
            }
            else
            {
                return BadRequest("The user doesn't exist!");
            }
            
            
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            // the user must have no tasks in order to be deleted
            var user = _context.Users.Include(x => x.Tasks).FirstOrDefault(user => user.ID == id); //finding the entity with the same ID
            try
            {
                // validation here
                if (user != null && (user.Tasks.Count == 0 || user.Tasks == null) )
                {
                    if (user.IsDeleted || !user.Active)

                    {
                        return BadRequest("The user is already deleted!");
                    }
                    user.IsDeleted = true;
                    user.Deleted = DateTime.Now;
                    user.Active = false;
                    _context.SaveChanges();
                    return Ok($"The user {user.Name} has been deleted successfully.");
                }
                else
                {
                    return BadRequest($"The user can't be removed because has {user.Tasks.Count} ongoing tasks!");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[HttpPost]
        //public ActionResult Undelete(int id)
        //{
        //    // the user must be deleted in order to get undeleted
        //    var user = _context.Users.FirstOrDefault(user => user.ID == id); //finding the entity with the same ID
        //    try
        //    {
        //        // validation here
        //        if (user != null)
        //        {
        //            if (!user.IsDeleted || user.Active)
        //            {
        //                return BadRequest("The user is not deleted!");
        //            }
        //            user.IsDeleted = false;
        //            user.Deleted = null;
        //            user.Active = true;
        //            _context.SaveChanges();
        //            return Ok();
        //        }
        //        else
        //        {
        //            return BadRequest("The user can't be removed because has ongoing tasks!");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}


    }
}
