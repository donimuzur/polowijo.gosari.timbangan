//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace polowijo.gosari.timbangan.dal
{
    using System;
    using System.Collections.Generic;
    
    public partial class SO_BAHAN_BAKU_DETAILS
    {
        public int ID { get; set; }
        public int ID_SO_BAHAN_BAKU { get; set; }
        public int ID_BARANG { get; set; }
        public decimal STOCK_SISTEM { get; set; }
        public Nullable<decimal> STOCK_REAL { get; set; }
        public Nullable<decimal> SELISIH { get; set; }
        public string KETERANGAN { get; set; }
    
        public virtual MST_BAHAN_BAKU MST_BAHAN_BAKU { get; set; }
        public virtual SO_BAHAN_BAKU SO_BAHAN_BAKU { get; set; }
    }
}
