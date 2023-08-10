using Dapper;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using TodoWebApi.Context;
using TodoWebApi.Interfaces;
using TodoWebApi.Models;

namespace TodoWebApi.Repository
{
    public class TodoItemRepository : ITodoItemRepository
    {
        
        private readonly DapperContext _context;

        //injeta o DapperContext no repository 
        public TodoItemRepository(DapperContext context)
        {
            _context = context;
        }

        //pegar todos os items
        public async Task<IEnumerable<TodoItem>> GetAllItemsAsync()
        {
            var query = "SELECT * FROM items";
            using(var connection = _context.CreateConnection())
            {
                var items = await connection.QueryAsync<TodoItem>(query);
                return items;
            };
        }

        //buscar por palavra-chave 
        public async Task<TodoItem> GetOneItemAsync(string keyWord)
        {
            var query = "SELECT * FROM items WHERE name_ LIKE @Name_";
            using(var connection = _context.CreateConnection())
            {
                var item = await connection.QueryFirstOrDefaultAsync<TodoItem>(query, new { Name_ = "%" + keyWord + "%"});
                return item;
            }
        }

        //Cria um item novo
        public async Task CreateItemAsync(TodoItem item)
        {
            //não precisa passar o id porquê é uma PRIMARY KEY
            var query = "INSERT INTO items(name_,isComplete) VALUES(@Name_,IsComplete)";  

            var parameters = new DynamicParameters();
            parameters.Add("name_", item.Name_, DbType.String); 
            parameters.Add("isComplete", item.IsComplete, DbType.Boolean);

            using(var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
            }
        }

        // atualizar um item
        public async Task UpdateItemAsync(TodoItem item)
        {
            // os nomes ex: @Name_, isComplete tem q estar iguais ao do objeto 
            var query = "UPDATE items SET name_ = @Name_, isComplete = @IsComplete WHERE id = @Id";

            var parameters = new DynamicParameters();
            parameters.Add("name_", item.Name_, DbType.String);
            parameters.Add("isComplete", item.IsComplete, DbType.Boolean);
            parameters.Add("id", item.Id, DbType.Int32);

            using( var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
            }
        }

        //Deletar algum item baseado no id
        public async Task DeleteItemAsync(int itemId)
        {
            var query = "DELETE FROM items WHERE id = @Id";

            using(var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, new { Id = itemId });  
            }
        }

    }
}
