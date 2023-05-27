using Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApi.Entities.Interfaces;
using ToDoApi.Entities.Models;

namespace ToDoApi.Data.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly ToDoContext _db;
        public TaskRepository(ToDoContext db)
        {
            this._db = db;
        }
        public void DeleteTask(UserTask task)
        {
            _db.Tasks.Remove(task);
        }

        public async Task<IEnumerable<UserTask>> GetAllTasks()
        {
            return await _db.Tasks.ToListAsync();
        }

        public async Task<IEnumerable<UserTask>> GetTasksByUser(string userId)
        {
            IQueryable<UserTask> query = _db.Tasks;
            query = query.Where(x => x.UserId == userId);
            return await query.ToListAsync();
        }

        public async Task InsertTask(UserTask task)
        {
            await _db.Tasks.AddAsync(task);
        }

        public async Task Save()
        {
            await _db.SaveChangesAsync();
        }

        public void UpdateTask(UserTask task)
        {
            _db.Tasks.Update(task);
        }
    }
}
