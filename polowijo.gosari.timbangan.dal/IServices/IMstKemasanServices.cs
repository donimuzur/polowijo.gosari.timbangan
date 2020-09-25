using polowijo.gosari.timbangan.dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace polowijo.gosari.timbangan.dal.IServices
{
    public interface IMstKemasanServices
    {
        List<MstKemasanDto> GetAll();
        List<MstKemasanDto> GetActiveAll();
        MstKemasanDto GetById(object Id);
        MstKemasanDto GetByNama(string Nama);
        void Save(MstKemasanDto model);
        MstKemasanDto Save(MstKemasanDto model, LoginDto LoginDto);
    }
}
