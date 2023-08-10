using Dapper;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoWebApi.Context;
using TodoWebApi.Interfaces;
using TodoWebApi.Models;

namespace TodoWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemController : ControllerBase
    {

        private readonly ITodoItemRepository _todoItemRepo;

        public TodoItemController(ITodoItemRepository todoItemRepo)
        {
            _todoItemRepo = todoItemRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllItemsAsync()
        {
            var items = await _todoItemRepo.GetAllItemsAsync();
            return Ok(items);
        }

        [HttpGet("{keyWord}")]
        public async Task<IActionResult> GetOneItemAsync(string keyWord)
        {
            var item = await _todoItemRepo.GetOneItemAsync(keyWord);
            if(item is null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> CreateItemAsync(TodoItem item)
        {
            await _todoItemRepo.CreateItemAsync(item);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateItemAsync(TodoItem item)
        {
            await _todoItemRepo.UpdateItemAsync(item);
            return Ok();
        }

        [HttpDelete("{itemId}")]
        public async Task<IActionResult> DeleteItemAsync(int itemId)
        {
            await _todoItemRepo.DeleteItemAsync(itemId);
            return Ok();
        }
    }
}
