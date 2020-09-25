using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using polowijo.gosari.timbangan.dto;
using polowijo.gosari.timbangan.dal;
using polowijo.gosari.timbangan.Model;

namespace polowijo.gosari.timbangan
{
    public class CustomMapper
    {
        public CustomMapper()
        {
            AutoMapper.Mapper.Initialize(cfg => {
                cfg.CreateMap<TrnDoModel, TrnDoDto>().ReverseMap();
                cfg.CreateMap<TrnDoDto, TRN_DO>().ReverseMap();

                cfg.CreateMap<SlipTimbanganModel, TrnSlipTimbanganDto>().ReverseMap();
                cfg.CreateMap<TrnSlipTimbanganDto, SLIP_TIMBANGAN>().ReverseMap();

                cfg.CreateMap<LoginModel, LoginDto>().ReverseMap();
                cfg.CreateMap<LoginDto, Login>().ReverseMap();
                
                cfg.CreateMap<MstKonsumenModel, MstKonsumenDto>().ReverseMap();
                cfg.CreateMap<MstKonsumenDto, MST_KONSUMEN>().ReverseMap();

                cfg.CreateMap<TrnPengirimanModel, TrnPengirimanDto>().ReverseMap();
                cfg.CreateMap<TrnPengirimanDto, TRN_PENGIRIMAN>().ReverseMap();

                cfg.CreateMap<RptEkspedisiHarianModel, RptEkspedisiHarianDto>().ReverseMap();
                cfg.CreateMap<RptEkspedisiHarianDto, SP_RealisasiHarian_Result>().ReverseMap();

            });
        }
    }
}
