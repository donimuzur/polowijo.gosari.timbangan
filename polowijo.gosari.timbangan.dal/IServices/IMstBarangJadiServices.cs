using polowijo.gosari.timbangan.dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace polowijo.gosari.timbangan.dal.IServices
{
    public interface IMstBarangJadiServices
    {
        List<MstBarangJadiDto> GetAll();
        MstBarangJadiDto GetById(object Id);
        MstBarangJadiDto GetByNama(string NamaBarang);
        void Save(MstBarangJadiDto model);
        void Save(MstBarangJadiDto model, LoginDto LoginDto);
        bool TambahSaldo(int IdBarang, decimal Jumlah);
        bool KurangSaldo(int IdBarang, decimal Jumlah);
    }
}
