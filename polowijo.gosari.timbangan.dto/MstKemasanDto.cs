﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace polowijo.gosari.timbangan.dto
{
    public class MstKemasanDto
    {
        public int ID { get; set; }
        public string KEMASAN { get; set; }
        public Nullable<byte> STATUS { get; set; }
        public string CREATED_BY { get; set; }
        public Nullable<System.DateTime> CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public Nullable<System.DateTime> MODIFIED_DATE { get; set; }
        public Nullable<decimal> CONVERTION { get; set; }
    }
}
