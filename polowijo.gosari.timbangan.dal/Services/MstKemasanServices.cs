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
    public class MstKemasanServices : IMstKemasanServices
    {
        private IUnitOfWork _uow;
        private IGenericRepository<MST_KEMASAN> _mstKemasanRepo;

        public MstKemasanServices()
        {
            _uow = new SqlUnitOfWork();
            _mstKemasanRepo = _uow.GetGenericRepository<MST_KEMASAN>();
        }
        public List<MstKemasanDto> GetAll()
        {
            var Data = _mstKemasanRepo.Get();
            var ReData = Mapper.Map<List<MstKemasanDto>>(Data);

            return ReData;
        }
        public List<MstKemasanDto> GetActiveAll()
        {
            var Data = _mstKemasanRepo.Get().Where(x => x.STATUS != (int)StatusDocument.Cancel).ToList();
            var ReData = Mapper.Map<List<MstKemasanDto>>(Data);

            return ReData;
        }
        public MstKemasanDto GetById(object Id)
        {
            var Data = _mstKemasanRepo.GetByID(Id);
            var ReData = Mapper.Map<MstKemasanDto>(Data);

            return ReData;
        }
        public MstKemasanDto GetByNama(string Nama)
        {
            var Data = _mstKemasanRepo.Get().Where(x => x.KEMASAN.ToUpper() == Nama.ToUpper()).FirstOrDefault();
            var ReData = Mapper.Map<MstKemasanDto>(Data);

            return ReData;
        }
        public void Save(MstKemasanDto model)
        {
            try
            {
                var db = Mapper.Map<MST_KEMASAN>(model);
                _mstKemasanRepo.InsertOrUpdate(db);
                _uow.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public MstKemasanDto Save(MstKemasanDto model, LoginDto LoginDto)
        {
            try
            {
                var db = Mapper.Map<MST_KEMASAN>(model);
                _mstKemasanRepo.InsertOrUpdate(db, Mapper.Map<Login>(LoginDto), MenuList.MasterKemasan);
                _uow.SaveChanges();
                return Mapper.Map<MstKemasanDto>(db);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
