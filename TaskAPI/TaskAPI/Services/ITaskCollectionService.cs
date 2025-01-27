using TaskAPI.Models;

namespace TaskAPI.Services
{
    public interface ITaskCollectionService: ICollectionService<TaskModel>
    {
        Task<List<TaskModel>> GetTasksByStatus(string status);
    }
}
