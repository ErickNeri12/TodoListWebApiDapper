using System.Runtime.InteropServices;

namespace TodoWebApi.Models
{
    public class TodoItem
    {
        public int Id { get; set; }
        public string Name_ { get; set; } = string.Empty;
        public bool IsComplete { get; set; }

    }
}
