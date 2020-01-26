using AutoMapper;
using polowijo.gosari.timbangan.core;
using polowijo.gosari.timbangan.dal.IServices;
using polowijo.gosari.timbangan.dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace polowijo.gosari.timbangan.dal.Services
{
    public class MstKonsumenServices : IMstKonsumenServices
    {
        private IGenericRepository<MST_KONSUMEN> _mstKonsumenRepo;
        private IUnitOfWork _uow;
        public MstKonsumenServices()
        {
            _uow = new SqlUnitOfWork();
            _mstKonsumenRepo = _uow.GetGenericRepository<MST_KONSUMEN>();
        }
        public List<MstKonsumenDto> GetAll()
        {
            var Data = _mstKonsumenRepo.Get();
            var ReData = Mapper.Map<List<MstKonsumenDto>>(Data);

            return ReData;
        }
        public MstKonsumenDto GetById(object Id)
        {
            var Data = _mstKonsumenRepo.GetByID(Id);
            var ReData = Mapper.Map<MstKonsumenDto>(Data);

            return ReData;
        }
        public MstKonsumenDto GetByNama(string Nama)
        {
            var Data = _mstKonsumenRepo.Get().Where(x => x.NAMA_KONSUMEN.ToUpper() == Nama.ToUpper()).FirstOrDefault();
            var ReData = Mapper.Map<MstKonsumenDto>(Data);

            return ReData;
        }
        public void Save(MstKonsumenDto model)
        {
            try
            {
                var db = Mapper.Map<MST_KONSUMEN>(model);
                _mstKonsumenRepo.InsertOrUpdate(db);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public MstKonsumenDto Save(MstKonsumenDto model, LoginDto LoginDto)
        {
            try
            {
                var db = Mapper.Map<MST_KONSUMEN>(model);
                var Login = Mapper.Map<Login>(LoginDto);
                _mstKonsumenRepo.InsertOrUpdate(db, Login, MenuList.MasterKonsumen);
                return Mapper.Map<MstKonsumenDto>(db);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void InsertUpdateMstKonsumen(MstKonsumenDto model, LoginDto LoginDto)
        {
            try
            {
                var GetAll = _mstKonsumenRepo.Get();
                var CheckExist = GetAll.Where(x => x.NAMA_KONSUMEN.ToUpper() == model.NAMA_KONSUMEN.ToUpper()).FirstOrDefault();

                if (CheckExist != null)
                {
                    model.ID = CheckExist.ID;
                    model.NO_TELP = CheckExist.NO_TELP;
                    model.CONTACT_PERSON = CheckExist.CONTACT_PERSON;
                }

                var db = Mapper.Map<MST_KONSUMEN>(model);
                var Login = Mapper.Map<Login>(LoginDto);
               _mstKonsumenRepo.InsertOrUpdate(db, Login, MenuList.MasterKonsumen);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
