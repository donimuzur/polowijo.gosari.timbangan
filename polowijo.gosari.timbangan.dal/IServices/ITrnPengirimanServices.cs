using polowijo.gosari.timbangan.dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace polowijo.gosari.timbangan.dal.IServices
{
    public interface ITrnPengirimanServices
    {
        List<TrnPengirimanDto> GetAll();
        List<TrnPengirimanDto> GetActiveAll();
        List<TrnPengirimanDto> GetAllByDoAndSPB(int Do, string NoSPB);
        TrnPengirimanDto GetById(object Id);
        void Save(TrnPengirimanDto Dto);
        TrnPengirimanDto Save(TrnPengirimanDto Dto, LoginDto Login);
        void Delete(int id, string Remarks);
        decimal? GetAkumulasi(int Do, string NoSPB);
    }
}
