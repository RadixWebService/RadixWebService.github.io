//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Proyecto_RadixWeb.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tiposhorasextras
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tiposhorasextras()
        {
            this.personas = new HashSet<personas>();
        }
    
        public int THor_Id { get; set; }
        public string THor_Nom { get; set; }
        public float THor_ValorDia { get; set; }
        public float THor_ValorHoraDia { get; set; }
        public float THor_ValorHoraExtraEsp { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<personas> personas { get; set; }
    }
}