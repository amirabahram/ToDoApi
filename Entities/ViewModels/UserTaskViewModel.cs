﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoApi.Entities.ViewModels
{
    public class UserTaskViewModel
    {
        public int TaskId { get; set; }
        public string Description { get; set; }
        public string UserName { get; set; }
    }
}
