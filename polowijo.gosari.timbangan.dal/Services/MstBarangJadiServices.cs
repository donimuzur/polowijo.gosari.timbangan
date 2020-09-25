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
    public class MstBarangJadiServices : IMstBarangJadiServices
    {
        private IUnitOfWork _uow;
        private IGenericRepository<MST_BARANG_JADI> _mstBarangJadiRepo;
        public MstBarangJadiServices()
        {
            _uow = new SqlUnitOfWork();
            _mstBarangJadiRepo = _uow.GetGenericRepository<MST_BARANG_JADI>();
        }
        public List<MstBarangJadiDto> GetAll()
        {
            try
            {
                var Data = _mstBarangJadiRepo.Get();
                var ReData = Mapper.Map<List<MstBarangJadiDto>>(Data);

                return ReData;
            }
            catch (Exception)
            {

                throw;
            }

        }
        public MstBarangJadiDto GetById(object Id)
        {
            try
            {
                var Data = _mstBarangJadiRepo.GetByID(Id);
                var ReData = Mapper.Map<MstBarangJadiDto>(Data);

                return ReData;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public MstBarangJadiDto GetByNama(string NamaBarang)
        {
            try
            {
                var Data = _mstBarangJadiRepo.Get().Where(x => x.NAMA_BARANG.ToUpper() == NamaBarang.ToUpper()).FirstOrDefault();
                var ReData = Mapper.Map<MstBarangJadiDto>(Data);

                return ReData;
            }
            catch (Exception)
            {

                throw;
            }

        }
        public void Save(MstBarangJadiDto model)
        {
            try
            {
                var db = Mapper.Map<MST_BARANG_JADI>(model);
                _mstBarangJadiRepo.InsertOrUpdate(db);
                _uow.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void Save(MstBarangJadiDto model, LoginDto LoginDto)
        {
            try
            {
                var db = Mapper.Map<MST_BARANG_JADI>(model);
                var Login = Mapper.Map<Login>(LoginDto);
                _mstBarangJadiRepo.InsertOrUpdate(db, Login, MenuList.MstBarangJadi);
                _uow.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool TambahSaldo(int IdBarang, decimal Jumlah)
        {
            try
            {
                var Db = _mstBarangJadiRepo.GetByID(IdBarang);
                Db.STOCK = Db.STOCK + Jumlah;

                _mstBarangJadiRepo.InsertOrUpdate(Db, new Login { FIRST_NAME = "SYSTEM", USERNAME = "SYSTEM" },MenuList.MstBarangJadi);
                _uow.SaveChanges();
                return true;
            }
            catch (Exception exp)
            {
                LogError.WriteError(exp);
                return false;
            }
        }

        public bool KurangSaldo(int IdBarang, decimal Jumlah)
        {
            try
            {
                var Db = _mstBarangJadiRepo.GetByID(IdBarang);
                Db.STOCK = Db.STOCK - Jumlah;

                _mstBarangJadiRepo.InsertOrUpdate(Db, new Login { FIRST_NAME = "SYSTEM", USERNAME = "SYSTEM" },MenuList.MstBarangJadi);
                _uow.SaveChanges();
                return true;
            }
            catch (Exception exp)
            {
                LogError.WriteError(exp);
                return false;
            }
        }
    }
}
