﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace polowijo.gosari.timbangan.Model
{
    public class ChangesHistoryModel
    {
        public long ID { get; set; }
        public int MODUL_ID { get; set; }
        public long FORM_ID { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime MODIFIED_DATE { get; set; }
        public string ACTION { get; set; }
    }
}
