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
    
    public partial class TRN_SURAT_PERINTAH_PRODUKSI
    {
        public int ID { get; set; }
        public string NO_SURAT { get; set; }
        public Nullable<System.DateTime> TANGGAL { get; set; }
        public Nullable<int> TUJUAN_PRODUKSI { get; set; }
        public string NO_SPB { get; set; }
        public string WILAYAH { get; set; }
        public string NAMA_KONSUMEN { get; set; }
        public string NAMA_PRODUK { get; set; }
        public string KOMPOSISI { get; set; }
        public string BENTUK { get; set; }
        public string BENTUK_DESC { get; set; }
        public string KEMASAN { get; set; }
        public Nullable<decimal> KUANTUM { get; set; }
        public string NO_DOKUMEN { get; set; }
        public Nullable<System.DateTime> RENCANA_KIRIM { get; set; }
        public string CATATAN_PPIC { get; set; }
        public Nullable<System.DateTime> TGL_SELESAI { get; set; }
        public string CATATAN_PRODUKSI { get; set; }
        public string CREATED_BY { get; set; }
        public Nullable<System.DateTime> CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public Nullable<System.DateTime> MODIFIED_DATE { get; set; }
        public byte STATUS { get; set; }
        public string REMARKS { get; set; }
    }
}
