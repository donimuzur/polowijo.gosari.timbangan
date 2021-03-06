﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace polowijo.gosari.timbangan.dto
{
    public class TrnSuratPermintaanBahanBakuDto
    {
        public int ID { get; set; }
        public System.DateTime TANGGAL { get; set; }
        public string NO_SPP { get; set; }
        public string NO_SURAT { get; set; }
        public string NAMA_PRODUK { get; set; }
        public string KOMPOSISI { get; set; }
        public string BENTUK { get; set; }
        public string KEMASAN { get; set; }
        public Nullable<decimal> KUANTUM_PRODUKSI { get; set; }
        public Nullable<int> SHIFT { get; set; }
        public System.DateTime CREATED_DATE { get; set; }
        public string CREATED_BY { get; set; }
        public Nullable<System.DateTime> MODIFIED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public Nullable<byte> STATUS { get; set; }
        public string CATATAN { get; set; }
        public string REMARKS { get; set; }


        public virtual List<TrnSuratPermintaanBahanBakuDetailsDto> TRN_SURAT_PERMINTAAN_BAHAN_BAKU_DETAILS { get; set; }
    }
    public class TrnSuratPermintaanBahanBakuDetailsDto
    {
        public int ID { get; set; }
        public int ID_TRN_SURAT_PERMINTAAN_BAHAN_BAKU { get; set; }
        public int ID_MST_BAHAN_BAKU { get; set; }
        public Nullable<decimal> KUANTUM { get; set; }

        public virtual MstBahanBakuDto MST_BAHAN_BAKU { get; set; }
        public virtual TrnSuratPermintaanBahanBakuDto TRN_SURAT_PERMINTAAN_BAHAN_BAKU { get; set; }
    }
}
