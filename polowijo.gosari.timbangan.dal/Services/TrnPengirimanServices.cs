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
    public class TrnPengirimanServices : ITrnPengirimanServices
    {
        private IGenericRepository<TRN_PENGIRIMAN> _trnPengirimanRepo;
        private IGenericRepository<DOCUMENT_NUMBER> _docNumberRepo;
        private IUnitOfWork _uow;
        public TrnPengirimanServices()
        {
            _uow = new SqlUnitOfWork();
            _trnPengirimanRepo = _uow.GetGenericRepository<TRN_PENGIRIMAN>();
            _docNumberRepo = _uow.GetGenericRepository<DOCUMENT_NUMBER>();
        }
        public List<TrnPengirimanDto> GetAll()
        {
            try
            {
                var Data = _trnPengirimanRepo.Get();
                var ReData = Mapper.Map<List<TrnPengirimanDto>>(Data);

                return ReData;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<TrnPengirimanDto> GetActiveAll()
        {
            try
            {
                var Data = _trnPengirimanRepo.Get().Where(x => x.STATUS != (int)StatusDocument.Cancel);
                var ReData = Mapper.Map<List<TrnPengirimanDto>>(Data);

                return ReData;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<TrnPengirimanDto> GetAllByDoAndSPB(int Do, string NoSPB)
        {
            try
            {

                var Data = _trnPengirimanRepo.Get().Where(x => x.NO_DO == Do && x.NO_SPB.ToUpper() == NoSPB.ToUpper()).ToList();
                var ReData = Mapper.Map<List<TrnPengirimanDto>>(Data);

                return ReData;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public TrnPengirimanDto GetById(object Id)
        {
            try
            {
                var Data = _trnPengirimanRepo.GetByID(Id);
                var ReData = Mapper.Map<TrnPengirimanDto>(Data);

                return ReData;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public void Save(TrnPengirimanDto Dto)
        {
            try
            {
                _trnPengirimanRepo.InsertOrUpdate(Mapper.Map<TRN_PENGIRIMAN>(Dto));
                _uow.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public TrnPengirimanDto Save(TrnPengirimanDto Dto, LoginDto Login)
        {
            try
            {
                var Db = Mapper.Map<TRN_PENGIRIMAN>(Dto);
                if (Db.ID == 0)
                {
                    int Id = 0;
                    var GetLatestNumber = _docNumberRepo.Get().Where(x => x.TANGGAL.Month == DateTime.Now.Month && x.TANGGAL.Year == DateTime.Now.Year && x.FORM_ID == (int)MenuList.TrnPengiriman).ToList();
                    if (GetLatestNumber.Count() > 0)
                    {
                        Id = GetLatestNumber.Max(x => x.NO.Value);
                    }

                    Db.NO_SURAT_JALAN = (Id + 1).ToString() + "/SJ/PPIC/" + IntToRomanConverter.ToRoman(DateTime.Now.Month) + "/" + DateTime.Now.Year;

                    var GetLatestRit = _trnPengirimanRepo.Get(x => x.NO_DO == Db.NO_DO && x.NO_SPB.ToUpper() == Db.NO_SPB.ToUpper() && x.STATUS != (int)StatusDocument.Cancel).ToList();
                    if (GetLatestRit.Count > 0)
                    {
                        Db.NO_RIT = GetLatestRit.Max(x => x.NO_RIT);
                    }
                    Db.NO_RIT = Db.NO_RIT + 1;

                    DOCUMENT_NUMBER DbDocNumber = new DOCUMENT_NUMBER();
                    DbDocNumber.NO = Id + 1;
                    DbDocNumber.FORM_ID = (int)MenuList.TrnPengiriman;
                    DbDocNumber.TANGGAL = DateTime.Now;

                    _docNumberRepo.InsertOrUpdate(DbDocNumber, Mapper.Map<Login>(Login), MenuList.TrnPengiriman);
                    _trnPengirimanRepo.InsertOrUpdate(Db, Mapper.Map<Login>(Login), MenuList.TrnPengiriman);

                }
                else
                {
                    _trnPengirimanRepo.InsertOrUpdate(Db, Mapper.Map<Login>(Login), MenuList.TrnPengiriman);
                }
                _uow.SaveChanges();
                return Mapper.Map<TrnPengirimanDto>(Db);
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
                var Db = Mapper.Map<TrnPengirimanDto>(_trnPengirimanRepo.GetByID(Id));
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
        public decimal? GetAkumulasi(int Do, string NoSPB)
        {
            try
            {
                var GetAll = _trnPengirimanRepo.Get().Where(x => x.NO_DO == Do && x.NO_SPB.ToUpper() == NoSPB.ToUpper() && x
                                        .STATUS != (int)StatusDocument.Cancel);
                var data = GetAll.Sum(x => x.KUANTUM);
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
