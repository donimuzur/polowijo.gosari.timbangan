﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace polowijo.gosari.timbangan.dto
{
    public class MstBahanBakuDto
    {
        public int ID { get; set; }
        public string NAMA_BARANG { get; set; }
        public decimal STOCK { get; set; }
        public Nullable<bool> STATUS { get; set; }
        public Nullable<System.DateTime> CREATED_DATE { get; set; }
        public string CREATED_BY { get; set; }
        public Nullable<System.DateTime> MODIFIED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public Nullable<decimal> STOCK_AWAL { get; set; }
        public Nullable<decimal> STOCK_AKHIR { get; set; }
        public string SATUAN { get; set; }
        public Nullable<byte> ISBAHAN_PEMBANTU { get; set; }
    }
}
