using GerenciadorCursos.Models;
using System.Collections.Generic;

namespace GerenciadorCursos.Repository
{
    public interface IRepository
    {

        IEnumerable<CursosModel> ObterTodosCursos();
        IEnumerable<CursosModel> ObterCursosPorStatus(StatusEnum Status);
   
    }
}
