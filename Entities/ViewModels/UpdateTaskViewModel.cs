using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoApi.Domain.ViewModels
{
    public class UpdateTaskViewModel
    {
        public int TaskId { get; set; }
        public string Description { get; set; }

    }
}
