using AutoMapper;
using LicenseGenerator.License;
using LicenseGenerator.Model;
using polowijo.gosari.timbangan.core;
using polowijo.gosari.timbangan.dal.IServices;
using polowijo.gosari.timbangan.dto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace polowijo.gosari.timbangan.dal.Services
{
    public class LoginServices : ILoginServices
    {
        private IUnitOfWork _uow;
        private IGenericRepository<Login> _loginRepo;
        private ILicenseEncrypt _encrypt;
        protected const string KeyInput = "BBDk3Cj7UTOm+3izr98Gy7dhsGjtk/lzmFJp6YPVl6SPTzu5rZjNdhP6XcvKeRIlxnq3b+CgxS/LOB64wH/ywhvBOYIlJRc976smvJx7LaqDNkRxxN2bTRG9z9KfWWDBIIAhOP1ULA7XL2fF2hq0dwgWCVBD2+ZfQ212dHdFsMA1/YTYGN8jpU7NI+XT9I43BoUqbcIunTy4JmKrY+w/oKHz19IAM0z4MciUUIbZl5wy4QyxvaNmkllq0KXywD2CkG7fZTGffjRnOt2b4uQ8+h6MrqWWieSae7xCUv7zDH25H/81YsinZvmJPdadtt5WfasXF7mKcG2D5D26enidgnk5NEGWjK456KvC8hrrhrQ5XX8o89KBdb5VHNpelalXw2gywlliyTyF+06nWwiYwYqIQqRnHGnb7p/t+fuY5G/tDlsAy7o8uZ2l+x2QcPIkuHyqnrBV4Nv9GPPNLwAV0gKVd3HNQglKAcWc8eKGZyttcTnHasP5x273QGRcnZ5ZJQh7VfDAeJIw0rXUmv8UxlBBI+5pN5s72kvZ1oC2wYS5w0axzCir0PApMGz/p38xEr8vaFNi12RpoLukLXDyooZD7JMaH6Kzyy/m73iS5BL1rhXZ93JIPKwPmWD1sh0WlOURpNMzjgPusPzuL6WNfY+kS2VvMIrW7ssVq9igBAA=";

        public LoginServices()
        {
            _uow = new SqlUnitOfWork();
            _encrypt = new LicenseEncrypt();
            _loginRepo = _uow.GetGenericRepository<Login>();
        }
        public List<LoginDto> GetAll()
        {
            try
            {
                var Data = _loginRepo.Get().ToList();
                return Mapper.Map<List<LoginDto>>(Data);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public LoginDto GetById(object id)
        {
            try
            {
                var Data = _loginRepo.GetByID(id);
                return Mapper.Map<LoginDto>(Data);
            }
            catch (Exception)
            {

                throw;
            }
          
        }
        public LoginDto Save(LoginDto Login)
        {
            try
            {
                var serializedPackage = _encrypt.Encrypt(Login.PASSWORD, KeyInput);
                var xmlSerializer = new XmlSerializer(typeof(SerializedPackage));
                using (StringWriter textWriter = new StringWriter())
                {
                    xmlSerializer.Serialize(textWriter, serializedPackage);
                    var SerializeByte = Encoding.UTF8.GetBytes(textWriter.ToString());
                    var SerializeBas64 = Convert.ToBase64String(SerializeByte);

                    Login.PASSWORD = SerializeBas64;
                }

                var db = Mapper.Map<Login>(Login);
                _loginRepo.InsertOrUpdate(db);
                return Mapper.Map<LoginDto>(db);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public LoginDto VerificationLogin(LoginDto Login)
        {
            try
            {
                var Data = _loginRepo.GetByID(Login.USER_ID);
                var ReData = Mapper.Map<LoginDto>(Data);
                var decryptResult = "";
                if (Data != null)
                {
                    var dataReadByte = Convert.FromBase64String(ReData.PASSWORD);
                    var dataString = Encoding.UTF8.GetString(dataReadByte);

                    SerializedPackage serializedPackage = null;
                    var serializer = new XmlSerializer(typeof(SerializedPackage));

                    using (TextReader TextReader = new StringReader(dataString))
                    {
                        serializedPackage = (SerializedPackage)serializer.Deserialize(TextReader);
                    }

                    decryptResult = _encrypt.Decrypt(serializedPackage.KeyBase64, serializedPackage.CipherDataBase64);
                }

                if (ReData != null && Login.USER_ID.ToLower() == ReData.USER_ID.ToLower() && Login.PASSWORD == decryptResult)
                {
                    return ReData;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {

                throw;
            }
           
        }
        public void ChangePassword(string UserId, string PasswordBaru)
        {
            try
            {
                var GetData = _loginRepo.GetByID(UserId);

                var serializedPackage = _encrypt.Encrypt(PasswordBaru, KeyInput);
                var xmlSerializer = new XmlSerializer(typeof(SerializedPackage));
                using (StringWriter textWriter = new StringWriter())
                {
                    xmlSerializer.Serialize(textWriter, serializedPackage);
                    var SerializeByte = Encoding.UTF8.GetBytes(textWriter.ToString());
                    var SerializeBas64 = Convert.ToBase64String(SerializeByte);

                    GetData.PASSWORD = SerializeBas64;
                }
                GetData.PASSWORD = PasswordBaru;
                _loginRepo.InsertOrUpdate(GetData);
            }
            catch (Exception)
            {

                throw;
            }
          
        }
        public void SetLastOnline(string UserId)
        {
            try
            {
                var GetData = _loginRepo.GetByID(UserId);
                GetData.LAST_ONLINE = DateTime.Now;
                _loginRepo.InsertOrUpdate(GetData);
            }
            catch (Exception)
            {

                throw;
            }
           
        }
        public DateTime? GetLastOnline(string UserId)
        {
            try
            {
                var GetData = _loginRepo.GetByID(UserId);
                return GetData == null ? null : GetData.LAST_ONLINE;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
