using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using cat_API.DB;
using cat_API.Models;
using Microsoft.AspNetCore.Authorization;
using NuGet.Common;
using Microsoft.AspNetCore.Http.Headers;
using System.Security.Claims;

namespace cat_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatController : ControllerBase
    {
        private readonly appDbContext _context;

        public CatController(appDbContext context)
        {
            _context = context;
        }

        // GET: api/Cat
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<CatModel>>> GetCats()
        {


            var id = User.Claims.FirstOrDefault(e => e.Type == "nameid")!.Value;

            var cats = _context.Cats.Where(c => c.User.Id.ToString() == id);
            if(cats == null)
            {
                return NotFound();
            }
           

            return Ok(cats);
        }

        // GET: api/Cat/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CatModel>> GetCatModel(int id)
        {
            var catModel = await _context.Cats.FindAsync(id);

            if (catModel == null)
            {
                return NotFound();
            }

            return catModel;
        }

        // PUT: api/Cat/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCatModel(int id, CatModel catModel)
        {
            if (id != catModel.Id)
            {
                return BadRequest();
            }

            _context.Entry(catModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CatModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Cat
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CatModel>> PostCatModel(CatModel catModel)
        {
            _context.Cats.Add(catModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCatModel", new { id = catModel.Id }, catModel);
        }

        // DELETE: api/Cat/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCatModel(int id)
        {
            var catModel = await _context.Cats.FindAsync(id);
            if (catModel == null)
            {
                return NotFound();
            }

            _context.Cats.Remove(catModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CatModelExists(int id)
        {
            return _context.Cats.Any(e => e.Id == id);
        }
    }
}
