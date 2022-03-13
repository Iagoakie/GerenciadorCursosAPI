﻿using System;
using System.ComponentModel.DataAnnotations;

namespace GerenciadorCursos.Models
{
    public class CursosModel
    {

        [Key]
        public int Id { get; set; }

        public string Titulo { get; set; }
       
        public  string duracao { get; set; }

        public StatusEnum Status { get; set; }


     

    }
}
