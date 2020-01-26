﻿using polowijo.gosari.timbangan.core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace polowijo.gosari.timbangan.Model
{
    public class BaseModel
    {
        public BaseModel()
        {
            ChangesHistory = new List<ChangesHistoryModel>();
        }
        public MenuList MainMenu { get; set; }
        public LoginModel CurrentUser { get; set; }
        public List<ChangesHistoryModel> ChangesHistory { get; set; }

        public string ErrorMessage { get; set; }
        public string SuccesMessage { get; set; }
        public string MessageTitle { get; set; }
        public string Menu { get; set; }
        public string Tittle { set; get; }
        public List<string> MessageBody { get; set; }

        public bool IsShowNewButton { get; set; }
        public bool IsNotViewer { get; set; }
        public bool IsAdmin { get; set; }
    }
}
