using Dapper;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoWebApi.Models;

namespace TodoWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemController : ControllerBase
    {

        // interface generica usada para acessar as configurações da aplicação(conexões com banco de dados, configurações personalizadas).
        private readonly IConfiguration _configuration;

        //injeta o IConfiguration no construtor para permitir que o controlador acesse informações de configuração, que podem ser usadas em diferentes partes do código.
        public TodoItemController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        //pegar todos os items
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetAllItemsAsync() 
        {
            using var connection = new MySqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            var items = await connection.QueryAsync<TodoItem>("select * from items");
            return Ok(items);
        }

        //buscar por palavra-chave 
        [HttpGet("{keyWord}")]
        public async Task<ActionResult<TodoItem>> GetOneItemAsync(string keyWord) 
        {
            using var connection = new MySqlConnection(_configuration.GetConnectionString("DefaultConnection"));                 
            var items = await connection.QueryFirstAsync<TodoItem>("select * from items where name_ like @Name_", new { Name_ = "%" + keyWord + "%" });
            return Ok(items);
        }

        //Cria um item novo
        [HttpPost]
        public async Task<ActionResult<IEnumerable<TodoItem>>> CreateItemAsync(TodoItem item) 
        {
            using var connection = new MySqlConnection(_configuration.GetConnectionString("DefaultConnection"));  //não precisa passar o id porquê é uma PRIMARY KEY
            await connection.ExecuteAsync("insert into items (name_,isComplete) VALUES (@Name_ , @isComplete)", item);
            // não esquecer de usar o await para aguardar a execução do método SelectAllItems 
            // O método SelectAllItems só é usado para mostrar que o item foi inserido após a requisição post mas não é uma boa prática fazer isso na mesma requisição
            return Ok(await SelectAllItemsAsync(connection));
        }

        [HttpPut]
        public async Task<ActionResult<IEnumerable<TodoItem>>> UpdateItemAsync(TodoItem item)
        {
            using var connection = new MySqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            // os nomes ex: @Name_, isComplete tem q estar iguais ao do objeto 
            await connection.ExecuteAsync("update items set name_ = @Name_, isComplete = @IsComplete where id = @Id", item);
            return Ok(await SelectAllItemsAsync(connection));
        }

        //Deletar algum item baseado no id
        [HttpDelete("{itemId}")]
        public async Task<ActionResult<IEnumerable<TodoItem>>> DeleteItemAsync(int itemId)
        {
            using var connection = new MySqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("delete from items where id = @Id", new {Id = itemId});
            return Ok(await SelectAllItemsAsync(connection));
        }

        //Método que recebe uma conexão com o mysql para buscar todos os items
        private static async Task<IEnumerable<TodoItem>> SelectAllItemsAsync(MySqlConnection connection)
        {
            return await connection.QueryAsync<TodoItem>("select * from items");    
        }
    }
}
