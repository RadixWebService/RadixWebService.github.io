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
    
    public partial class contratos
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public contratos()
        {
            this.asistencias = new HashSet<asistencias>();
        }
    
        public int Con_Id { get; set; }
        public int Sub_Id { get; set; }
        public Nullable<int> Doc_Id { get; set; }
        public Nullable<int> TCon_Id { get; set; }
        public string Per_Rut { get; set; }
        public string Con_FechaInicio { get; set; }
        public string Con_FechaFin { get; set; }
        public string Con_Estado { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<asistencias> asistencias { get; set; }
        public virtual documentos documentos { get; set; }
        public virtual personas personas { get; set; }
        public virtual subempresas subempresas { get; set; }
        public virtual tiposcontratos tiposcontratos { get; set; }
    }
}