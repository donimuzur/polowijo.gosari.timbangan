using polowijo.gosari.timbangan.dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace polowijo.gosari.timbangan.dal.IServices
{
    public interface ITrnSlipTimbanganServices
    {
        List<TrnSlipTimbanganDto> GetAll();
        List<TrnSlipTimbanganDto> GetAllActive();
        TrnSlipTimbanganDto GetById(object Id);
        TrnSlipTimbanganDto GetBySuratJalan(string NoSuratJalan);
        TrnSlipTimbanganDto Save(TrnSlipTimbanganDto Dto, LoginDto Login);
        TrnSlipTimbanganDto Save(TrnSlipTimbanganDto model);
        void DeleteById(object Id, LoginDto Login);
    }
}
