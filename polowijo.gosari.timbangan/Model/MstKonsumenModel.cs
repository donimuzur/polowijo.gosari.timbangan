﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace polowijo.gosari.timbangan.Model
{
    public class MstKonsumenModel
    {
        public int ID { get; set; }
        public string NAMA_KONSUMEN { get; set; }
        public string ALAMAT_KONSUMEN { get; set; }
        public string CONTACT_PERSON { get; set; }
        public string NO_TELP { get; set; }
        public Nullable<int> STATUS { get; set; }
        public string PERSONNO { get; set; }
    }
   
}
