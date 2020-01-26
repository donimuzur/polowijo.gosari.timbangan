using polowijo.gosari.timbangan.dal;
using polowijo.gosari.timbangan.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace polowijo.gosari.timbangan
{
    public class BaseController
    {
        public static Login CurrentUser { set; get; }
    }
}
