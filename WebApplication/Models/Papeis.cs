//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApplication.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    
    public partial class Papeis
    {
        [Required]
        [Key, Column(Order = 0)]
        public int FilmeID { get; set; }

        [Required]
        [Key, Column(Order = 1)]
        public int AtorID { get; set; }

        [Required(ErrorMessage = "O {0} � obrigat�rio")]
        [Display(Name = "Nome do personagem")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O {0} deve ter no m�nimo 3 e no m�ximo 100 caracteres.")]
        public string NomePersonagem { get; set; }
    
        public virtual Atores Atores { get; set; }
        public virtual Filmes Filmes { get; set; }
    }
}
