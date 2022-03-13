using GerenciadorCursos.Data;
using GerenciadorCursos.Models;
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

        public IEnumerable<CursosModel> ObterCursosPorStatus(StatusEnum Status)
        {

            return _context.CursosModels.Where((CursosModel course) => course.Status == Status);

        }

        public IEnumerable<CursosModel> ObterTodosCursos()
        {
            return _context.CursosModels.ToList();

        }
            
        }
    }

