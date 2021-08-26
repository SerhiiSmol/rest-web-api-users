using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestWebApi.Data;
using RestWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public UsersController(ApplicationDbContext db)
        {
            _db = db;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_db.Users.ToList());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var user = _db.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpGet]
        [Route("/api/Users/Status/{status}")]
        public IActionResult GetByStatus(int status)
        {
            var users = _db.Users.Where(u => u.status == status).ToList();
            if (users == null || users.Count <= 0)
            {
                return NotFound();
            }
            return Ok(users);
        }


        [HttpPost]
        public IActionResult Create([FromBody] User user)
        {
            
            _db.Users.Add(user);
            _db.SaveChanges();
            return CreatedAtAction(nameof(GetById),
                new {user.id},
                user
                );
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id,User user)
        {
            if (id != user.id)
            {
                return BadRequest();
            }
            _db.Entry(user).State = EntityState.Modified;

            try
            {
                _db.SaveChanges();
            }
            catch (Exception)
            {
                if (_db.Users.Find(id) == null)
                {
                    return NotFound();
                }
            }

            return NoContent();
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var user = _db.Users.Find(id);
            if(user == null)
            {
                return NotFound();
            }
            _db.Remove(user);
            _db.SaveChanges();
            return Ok(user);
        }
    }
}
