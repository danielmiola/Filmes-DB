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
    
    public partial class Studios
    {
        public Studios()
        {
            this.Filmes = new HashSet<Filmes>();
        }
    
        public int StudioID { get; set; }
        public string Nome { get; set; }
        public string Cidade { get; set; }
    
        public virtual ICollection<Filmes> Filmes { get; set; }
    }
}