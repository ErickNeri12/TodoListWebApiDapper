using System.Collections.Generic;
using System.Threading.Tasks;
using TodoWebApi.Models;

namespace TodoWebApi.Interfaces
{
    public interface ITodoItemRepository
    {

        public Task<IEnumerable<TodoItem>> GetAllItemsAsync();
        public Task<TodoItem> GetOneItemAsync(string keyWord);
        public Task CreateItemAsync(TodoItem item);
        public Task UpdateItemAsync(TodoItem item);
        public Task DeleteItemAsync(int itemId);

    }
}
