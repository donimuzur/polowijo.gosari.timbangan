using polowijo.gosari.timbangan.dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace polowijo.gosari.timbangan.dal.IServices
{
    public interface IRptRealisasiHarianServices
    {
        List<RptEkspedisiHarianDto> GetRpt(string DateTime);
    }
}
