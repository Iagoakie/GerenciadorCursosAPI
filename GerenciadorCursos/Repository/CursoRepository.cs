using GerenciadorCursos.Data;
using GerenciadorCursos.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace GerenciadorCursos.Repository
{
    public class cursorepository : IRepository
    {
        private readonly GerenciadorContext _context;

        public cursorepository(GerenciadorContext context)
        {
            _context = context;
        }

        public CursosModel ObterCursosPorId(int id)
        {

            var cursosModel =  _context.CursosModels.Find(id);

            if (cursosModel == null)
            {
                return null;
            }

            return cursosModel;


        }

        public IEnumerable<CursosModel> ObterTodosCursos()
        {
            return _context.CursosModels.ToList();

        }
            
        }
    }

