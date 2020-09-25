using AutoMapper;
using polowijo.gosari.timbangan.dal.IServices;
using polowijo.gosari.timbangan.dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace polowijo.gosari.timbangan.dal.Services
{
    public class RptRealisasiHarianServices : IRptRealisasiHarianServices
    {
        private IUnitOfWork _uow;

        public RptRealisasiHarianServices()
        {
            _uow = new SqlUnitOfWork();
        }
        public List<RptEkspedisiHarianDto> GetRpt(string DateTime)
        {
            var Data = new List<SP_RealisasiHarian_Result>();
            
            using (var EMSDD_CONTEXT = new TIMBANGANEntities())
            {
                var result = EMSDD_CONTEXT.SP_RealisasiHarian(DateTime);
                return Mapper.Map<List<RptEkspedisiHarianDto>>(result);
            }
        }
    }
}
