using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoCoreWebApi.Models
{
    public class TodoItem
    {
        public long? Id { get; set; }
        public long? Order { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public bool Completed { get; set; }
       
    }
}
