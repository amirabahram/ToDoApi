using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoApi.Entities.ViewModels
{
    public class ResponseViewModel
    {
        public string Email { get; set; }
        public string UserId { get; set; }
        public string TokenString { get; set; }
    }
}
