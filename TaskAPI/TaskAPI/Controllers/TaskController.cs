using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TaskAPI.Models;
using TaskAPI.Services;

namespace TaskAPI.Controllers
{
    /// <summary>
    /// task controller
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class TaskController : ControllerBase
    {
        ITaskCollectionService _taskCollectionService;

        public TaskController(ITaskCollectionService taskCollectionService)
        {
            _taskCollectionService = taskCollectionService ?? throw new ArgumentNullException(nameof(TaskCollectionService));
        }

        /// <summary>
        /// returns all tasks
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetTasks()
        {
            List<TaskModel> tasks = await _taskCollectionService.GetAll();
            return Ok(tasks);
        }


        /// <summary>
        /// adds a task
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] TaskModel task)
        {
            await _taskCollectionService.Create(task);
            return Ok(task);
        }

        /// <summary>
        /// replaces the task that has the matching id with the sent task
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> UpdateTask([FromBody] TaskModel task)
        {
            await _taskCollectionService.Update(task.Id, task);
            return Ok(task);
        }

        /// <summary>
        /// deletes task with matching id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            bool deleted = await _taskCollectionService.Delete(id);
            
            if (!deleted)
                return NotFound();

            return Ok(200);
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateTaskStatus(Guid id, [FromBody] StatusUpdateRequest statusUpdateRequest)
        {
            // Fetch the task by id
            var task = await _taskCollectionService.Get(id);
            if (task == null)
            {
                return NotFound("Task not found.");
            }

            // Update the task's status
            task.Status = statusUpdateRequest.Status;
            var updated = await _taskCollectionService.Update(id, task); // Update task in DB

            if (!updated)
            {
                return BadRequest("Failed to update task.");
            }

            return Ok(task); // Return the updated task
        }


    }
}
