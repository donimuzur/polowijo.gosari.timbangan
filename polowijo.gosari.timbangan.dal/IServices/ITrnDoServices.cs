using polowijo.gosari.timbangan.dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace polowijo.gosari.timbangan.dal.IServices
{
    public interface ITrnDoServices
    {
        List<TrnDoDto> GetAll();
        TrnDoDto GetById(object Id);
        List<TrnDoDto> GetBySPB(string SPB);
        TrnDoDto GetByDo(string DO);
        TrnDoDto GetBySpbAndDo(string SPB, string DO);
        void Save(TrnDoDto model);
        TrnDoDto Save(TrnDoDto Dto, LoginDto Login);
    }
}
