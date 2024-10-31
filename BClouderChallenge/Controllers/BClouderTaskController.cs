using Domain.Entities;
using Domain.ViewModels;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BClouderChallenge.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BClouderTaskController : Controller
    {
        private BClouderDbContext _context;

        public BClouderTaskController(BClouderDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        [HttpGet]
        public ActionResult<List<BClouderTask>> Get()
        {
            List<BClouderTask> allTasks = new List<BClouderTask>(); //getting all tasks in database
            allTasks = _context.Tasks.Where(x=> !x.IsDeleted && x.Active).Include(x => x.User).ToList();
            return Ok(allTasks);
        }

        [HttpPost]
        public ActionResult<List<BClouderTask>> Add(TaskViewModel task)
        {
            if (task == null)
            {
                return BadRequest("The task is null!");
            }
            else
            {
                if (task.UserID == 0)
                {
                    return BadRequest("The user must be informed!");
                }
                else if(task.Description.IsNullOrEmpty() || task.Title.IsNullOrEmpty())
                {
                    return BadRequest("The task must have a title and a description.");
                }

                BClouderTask taskToAdd = new BClouderTask();
                taskToAdd.UserID = task.UserID;
                var user = _context.Users.Include(x => x.Tasks).FirstOrDefault(x => x.ID == task.UserID);
                if (user != null)
                {
                    user.Available = false;
                    taskToAdd.User = user;
                    taskToAdd.Description = task.Description;
                    taskToAdd.Title = task.Title;

                    taskToAdd.Active = true;
                    taskToAdd.Created = DateTime.Now;
                    taskToAdd.IsDeleted = false;

                    taskToAdd.TaskStatus = (BClouderTask.Status)task._TaskStatus;

                    var tasks = _context.Tasks.Add(taskToAdd); // creating the new entity 

                    _context.SaveChanges(); // saving

                    return Ok(tasks.Entity);
                }
                else
                {
                    return BadRequest("The user can't be null!");
                }
                
            }

        }

        [HttpPut]
        public ActionResult<List<BClouderTask>> Update(int taskId, TaskViewModel task)
        {
            var taskToAdd = _context.Tasks.FirstOrDefault(u => u.ID == taskId);

            if (taskToAdd != null)
            {
                // refresh only the specified features
                if (task.Title != null)
                    taskToAdd.Title = task.Title;

                if (task.Description != null)
                    taskToAdd.Description = task.Description;

                if (task._TaskStatus != 0)
                    taskToAdd.TaskStatus = (BClouderTask.Status)task._TaskStatus;
                // we cannot change fk user here in order to avoid problems in the future (database normalization)

                _context.SaveChanges();
                return Ok("The task has been changed successfully!");
            }
            else
            {
                return BadRequest("The task can't be null!");
            }
           
        }

        [HttpDelete]
        public ActionResult<List<BClouderTask>> Delete(int id)
        {
            var task = _context.Tasks.FirstOrDefault(task => task.ID == id); //finding the entity with the same ID and removing it

            if (task != null && (task.Active && !task.IsDeleted))
            {
                task.Active = false;
                task.IsDeleted = true;
                task.Deleted = DateTime.Now;
                //_context.Remove(id);
                
                _context.SaveChanges();

                return Ok(task);
            }
            else
            {
                return BadRequest("The task couldn't be deleted.");
            }
            
        }
    }
}
