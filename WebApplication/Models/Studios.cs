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
    
    public partial class Studios
    {
        public Studios()
        {
            this.Filmes = new HashSet<Filmes>();
        }
    
        public int StudioID { get; set; }

        [Required(ErrorMessage = "O {0} � obrigat�rio")]
        [Display(Name = "Studio")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O {0} deve ter no m�nimo 3 e no m�ximo 100 caracteres.")]
        public string Nome { get; set; }

        [StringLength(100, MinimumLength = 3, ErrorMessage = "O {0} deve ter no m�nimo 3 e no m�ximo 100 caracteres.")]
        public string Cidade { get; set; }
    
        public virtual ICollection<Filmes> Filmes { get; set; }
    }
}
