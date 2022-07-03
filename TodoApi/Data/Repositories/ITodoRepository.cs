using TodoApi.Models;

namespace TodoApi.Data.Repositories
{
    public interface ITodoRepository
    {
        Task<List<Todo>> GetAsync();

        Task<Todo> GetAsync(string id);

        Task CreateAsync(Todo newTodo);

        Task UpdateAsync(string id, Todo updateTodo);

        Task RemoveAsync(string id);
    }
}