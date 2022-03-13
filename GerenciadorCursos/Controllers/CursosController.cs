using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GerenciadorCursos.Data;
using GerenciadorCursos.Models;
using GerenciadorCursos.Repository;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace GerenciadorCursos.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CursosController : ControllerBase
    {
        private readonly GerenciadorContext _context;
        private readonly IRepository repository;

        public CursosController(GerenciadorContext context, IRepository repository)
        {
            _context = context;
            this.repository = repository;
        }

        // GET: api/Cursos
        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetCursosModels()
        {
            var retorno = repository.ObterTodosCursos();
            return Ok(retorno);
        }

        // GET: api/Cursos/5
        [AllowAnonymous]
        [HttpGet("Status")]
        public IActionResult GetCursosModel(StatusEnum Status)
        {
            var curso = repository.ObterCursosPorStatus(Status);

            return Ok(curso);
        }

        // PUT: api/Cursos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

        [Authorize(Roles = "Secretaria,Gerente")]
        [HttpPut("Secretaria,Gerente")]
        public async Task<IActionResult> PutCursosModel(int id, CursosModel cursosModel)
        {
            if (id != cursosModel.Id)
            {
                return BadRequest();
            }

            _context.Entry(cursosModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CursosModelExists(id))
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

        // POST: api/Cursos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CursosModel>> PostCursosModel(CursosModel cursosModel)
        {
            _context.CursosModels.Add(cursosModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCursosModel", new { id = cursosModel.Id }, cursosModel);
        }

        // DELETE: api/Cursos/5
        [Authorize(Roles = "Gerente")]
        [HttpDelete("Gerente")]
        public async Task<IActionResult> DeleteCursosModel(int id)
        {
            var cursosModel = await _context.CursosModels.FindAsync(id);
            if (cursosModel == null)
            {
                return NotFound();
            }

            _context.CursosModels.Remove(cursosModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CursosModelExists(int id)
        {
            return _context.CursosModels.Any(e => e.Id == id);
        }



    }
}
