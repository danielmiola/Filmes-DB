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
    
    public partial class Atores
    {
        public Atores()
        {
            this.Papeis = new HashSet<Papeis>();
        }
    
        public int AtoresID { get; set; }
        public string Nome { get; set; }
        public System.DateTime DataNascimento { get; set; }
    
        public virtual ICollection<Papeis> Papeis { get; set; }
    }
}