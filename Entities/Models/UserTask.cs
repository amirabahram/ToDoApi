using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoApi.Entities.Models
{
    public class UserTask
    {
        
        public int Id { get; set; }
        [MaxLength(1000)]
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        public IdentityUser User { get; set; }
    }
}
