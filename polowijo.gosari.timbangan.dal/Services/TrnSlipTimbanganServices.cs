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
    public class TrnSlipTimbanganServices : ITrnSlipTimbanganServices
    {
        private IGenericRepository<SLIP_TIMBANGAN> _trnSlipTimbanganRepo;
        private IGenericRepository<DOCUMENT_NUMBER> _docNumberRepo;
        private IUnitOfWork _uow;
        public TrnSlipTimbanganServices()
        {
            _uow = new SqlUnitOfWork();
            _docNumberRepo = _uow.GetGenericRepository<DOCUMENT_NUMBER>();
            _trnSlipTimbanganRepo = _uow.GetGenericRepository<SLIP_TIMBANGAN>();
        }
        public List<TrnSlipTimbanganDto> GetAll()
        {
            var Data = _trnSlipTimbanganRepo.Get();
            var ReData = Mapper.Map<List<TrnSlipTimbanganDto>>(Data);

            return ReData;
        }
        public List<TrnSlipTimbanganDto> GetAllActive()
        {
            var Data = _trnSlipTimbanganRepo.Get().Where(x=>x.STATUS != (int)StatusDocument.Cancel);
            var ReData = Mapper.Map<List<TrnSlipTimbanganDto>>(Data);

            return ReData;
        }
        public TrnSlipTimbanganDto GetById(object Id)
        {
            var Data = _trnSlipTimbanganRepo.GetByID(Id);
            var ReData = Mapper.Map<TrnSlipTimbanganDto>(Data);

            return ReData;
        }
        public TrnSlipTimbanganDto GetBySuratJalan(string NoSuratJalan)
        {
            var Data = _trnSlipTimbanganRepo.Get().Where(x => x.NO_SURAT_JALAN != null && x.NO_SURAT_JALAN.ToUpper() == NoSuratJalan.ToUpper()).FirstOrDefault();
            var ReData = Mapper.Map<TrnSlipTimbanganDto>(Data);

            return ReData;
        }
        public TrnSlipTimbanganDto Save(TrnSlipTimbanganDto Dto, LoginDto Login)
        {
            try
            {
                var Db = Mapper.Map<SLIP_TIMBANGAN>(Dto);
                if (Db.ID == 0)
                {
                    int Id = 0;
                    var GetLatestNumber = _docNumberRepo.Get().Where(x => x.TANGGAL.Month == DateTime.Now.Month && x.TANGGAL.Year == DateTime.Now.Year && x.FORM_ID == (int)MenuList.TrnSlipTimbangan).ToList();
                    if (GetLatestNumber.Count() > 0)
                    {
                        Id = GetLatestNumber.Max(x => x.NO.Value);
                    }

                    Db.NO_RECORD = Id + 1;

                    DOCUMENT_NUMBER DbDocNumber = new DOCUMENT_NUMBER();
                    DbDocNumber.NO = Id + 1;
                    DbDocNumber.FORM_ID = (int)MenuList.TrnSlipTimbangan;
                    DbDocNumber.TANGGAL = Db.TANGGAL;

                    _docNumberRepo.InsertOrUpdate(DbDocNumber, Mapper.Map<Login>(Login), MenuList.TrnSlipTimbangan);
                    _trnSlipTimbanganRepo.InsertOrUpdate(Db, Mapper.Map<Login>(Login), MenuList.TrnSlipTimbangan);
                   
                }
                else
                {
                    _trnSlipTimbanganRepo.InsertOrUpdate(Db, Mapper.Map<Login>(Login), MenuList.TrnSlipTimbangan);
                }
                _uow.SaveChanges();
                return Mapper.Map<TrnSlipTimbanganDto>(Db);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public TrnSlipTimbanganDto Save(TrnSlipTimbanganDto model)
        {
            try
            {
                var db = Mapper.Map<SLIP_TIMBANGAN>(model);
                _trnSlipTimbanganRepo.InsertOrUpdate(db);
                _uow.SaveChanges();
                return Mapper.Map<TrnSlipTimbanganDto>(db);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void DeleteById(object Id, LoginDto Login)
        {
            try
            {
                var Db =Mapper.Map<TrnSlipTimbanganDto>( _trnSlipTimbanganRepo.GetByID(Id));
                Db.STATUS = (int)StatusDocument.Cancel;
                Db.MODIFIED_BY = Login.USERNAME;
                Db.MODIFIED_DATE = DateTime.Now;
                Save(Db, Login);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
