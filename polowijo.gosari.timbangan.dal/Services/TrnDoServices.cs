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
    public class TrnDoServices : ITrnDoServices
    {
        private IGenericRepository<TRN_DO> _trnDoRepo;
        private IGenericRepository<DOCUMENT_NUMBER> _docNumberRepo;
        private IUnitOfWork _uow;
        public TrnDoServices()
        {
            _uow = new SqlUnitOfWork();
            _docNumberRepo = _uow.GetGenericRepository<DOCUMENT_NUMBER>();
            _trnDoRepo = _uow.GetGenericRepository<TRN_DO>();
        }
        public List<TrnDoDto> GetAll()
        {
            try
            {
                var Data = _trnDoRepo.Get();
                var ReData = Mapper.Map<List<TrnDoDto>>(Data);

                return ReData;
            }
            catch (Exception)
            {

                throw;
            }
           
        }
        public TrnDoDto GetById(object Id)
        {
            try
            {
                var Data = _trnDoRepo.GetByID(Id);
                var ReData = Mapper.Map<TrnDoDto>(Data);

                return ReData;
            }
            catch (Exception)
            {

                throw;
            }
          
        }
        public List<TrnDoDto> GetBySPB(string SPB)
        {
            try
            {
                var Data = _trnDoRepo.Get().Where(x => x.NO_SPB.ToUpper() == SPB.ToUpper()).ToList();
                var ReData = Mapper.Map<List<TrnDoDto>>(Data);

                return ReData;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public TrnDoDto GetByDo(string DO)
        {
            try
            {
                var Data = _trnDoRepo.Get().Where(x => x.NO_DO.ToUpper() == DO.ToUpper()).FirstOrDefault();
                var ReData = Mapper.Map<TrnDoDto>(Data);

                return ReData;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public TrnDoDto GetBySpbAndDo(string SPB, string DO)
        {
            try
            {
                var Data = _trnDoRepo.Get().Where(x => x.NO_SPB.ToUpper() == SPB.ToUpper() && x.NO_DO.ToUpper() == DO.ToUpper()).FirstOrDefault();
                var ReData = Mapper.Map<TrnDoDto>(Data);

                return ReData;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public void Save(TrnDoDto model)
        {
            try
            {
                var db = Mapper.Map<TRN_DO>(model);
                _trnDoRepo.InsertOrUpdate(db);
                _uow.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public TrnDoDto Save(TrnDoDto Dto, LoginDto Login)
        {
            try
            {
                var Db = Mapper.Map<TRN_DO>(Dto);
                if (Db.ID == 0)
                {
                    int Id = 0;
                    var GetLatestNumber = _docNumberRepo.Get().Where(x => x.TANGGAL.Month == DateTime.Now.Month && x.TANGGAL.Year == DateTime.Now.Year && x.FORM_ID == (int)MenuList.TrnDo).ToList();
                    if (GetLatestNumber.Count() > 0)
                    {
                        Id = GetLatestNumber.Max(x => x.NO.Value);
                    }
                    Db.NO_DO = (Id + 1).ToString();

                    DOCUMENT_NUMBER DbDocNumber = new DOCUMENT_NUMBER();
                    DbDocNumber.NO = Id + 1;
                    DbDocNumber.FORM_ID = (int)MenuList.TrnDo;
                    DbDocNumber.TANGGAL = DateTime.Now;

                    _trnDoRepo.InsertOrUpdate(Db, Mapper.Map<Login>(Login), MenuList.TrnDo);
                    _docNumberRepo.InsertOrUpdate(DbDocNumber, Mapper.Map<Login>(Login), MenuList.TrnDo);

                }
                else
                {
                    _trnDoRepo.InsertOrUpdate(Db, Mapper.Map<Login>(Login), MenuList.TrnDo);
                }
                _uow.SaveChanges();
                return Mapper.Map<TrnDoDto>(Db);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
