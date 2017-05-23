using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using NetSqlAzMan.Interfaces;
using NetSqlAzMan;
using Afaran.MedicalWaste.Engine.Application.AzMan.Config;
using System.Configuration;
using AzManCofee.Models;

namespace AzManCofee
{
    public class AzManService : IAzManService
    {
        #region properties

        private readonly string _connectionStringName;
        private readonly string _azManConnectionString;
        private readonly string _applicationName;
        private readonly string _storageName;
        private readonly string _fullDomainName;
        private bool result;
        private string fullUserName;


        #endregion

        #region Ctor

        public AzManService()
        {

            _connectionStringName = ConfigurationManager.AppSettings["AzManConnectionStringName"];
            _applicationName = ConfigurationManager.AppSettings["AzManAppName"];
            _storageName = ConfigurationManager.AppSettings["AzManStorageName"];
            _fullDomainName = "@" + ConfigurationManager.AppSettings["AzManDomainName"];
            _azManConnectionString = ConfigurationManager.ConnectionStrings[_connectionStringName].ToString();

            result = false;
        }

        #endregion

        #region Public Methods




        /// <summary>
        /// بررسی معتبر بودن کاربر و مجاز بودن دسترسی او
        /// </summary>
        public bool UserExists(string userName)
        {
            try
            {
                fullUserName = GetUserNameWithoutDomain(userName) + _fullDomainName;
                //اگر یوزر در اکتیو دایرکتوری نباشد حین نمونه سازی استثنا صادر میشود
                WindowsIdentity wi = new WindowsIdentity(fullUserName);

                result = true;
            }
            catch (Exception ex)
            {
                //log ex
            }

            return result;
        }





        /// <summary>
        /// بررسی وجود نقش برای کاربر
        /// </summary>
        public bool IsInRole(string userName, string roleName)
        {

            fullUserName = GetUserNameWithoutDomain(userName) + _fullDomainName;
            try
            {
                WindowsIdentity wi = new WindowsIdentity(fullUserName);
                AzmanSid sid = new AzmanSid(wi);
                var storage = new SqlAzManStorage(_azManConnectionString);
                storage.OpenConnection();
                //اگر نقش مورد نظر موجود بود
                if (RoleExists(roleName))
                {
                    IAzManItem itemRole = storage[_storageName][_applicationName][roleName];
                    IAzManAuthorization[] authorizations = itemRole.GetAuthorizations();
                    //اگر کاربر با این نقش احراز هویت شده
                    result = authorizations.Any(i => i.SID.StringValue == sid.StringValue);
                }

                storage.CloseConnection();
            }
            catch (Exception ex)
            {
                //log ex
            }

            return result;
        }





        /// <summary>
        /// بررسی معتبر بودن نام نقش
        /// </summary>
        public bool RoleExists(string roleName)
        {
            var roles = GetAllRoles();
            return roles.Any(r => r.Name == roleName);
        }




        /// <summary>
        /// افزودن نقش به کاربر 
        /// </summary>
        public bool AddUserToRole(string userName, string roleName)
        {
            try
            {
                WindowsIdentity wi = new WindowsIdentity(GetUserNameWithoutDomain(userName) + _fullDomainName);
                AzmanSid sid = new AzmanSid(wi);
                IAzManStorage storage = new SqlAzManStorage(_azManConnectionString);
                storage.OpenConnection();
                //اگر نقش مورد نظر در ای زد من تعریف شده بود
                if (RoleExists(roleName))
                {
                    IAzManItem itemRole = storage[_storageName][_applicationName][roleName];
                    //نقش به کاربر اختصاص داده شود
                    IAzManAuthorization auth = itemRole.CreateAuthorization(sid, WhereDefined.LDAP, sid, WhereDefined.LDAP, AuthorizationType.Allow, null, null);
                }

                storage.CloseConnection();
                result = true;

            }
            catch (Exception ex)
            {
                //log ex
            }

            return result;

        }




        /// <summary>
        /// حذف نقش از کاربر
        /// </summary>
        public bool RemoveUserFromRole(string userName, string role)
        {
            fullUserName = GetUserNameWithoutDomain(userName) + _fullDomainName;

            try
            {
                WindowsIdentity wi = new WindowsIdentity(fullUserName);
                AzmanSid sid = new AzmanSid(wi);

                IAzManStorage storage = new SqlAzManStorage(_azManConnectionString);

                storage.OpenConnection();
                //دریافت نقش
                IAzManItem itemRole = storage[_storageName][_applicationName][role];

                //دریافت اطلاعات کاربرانی که با این نقش احراز هویت شده اند
                IAzManAuthorization[] authorizations = itemRole.GetAuthorizations();
                var userAuth = authorizations.FirstOrDefault(a => a.SID.StringValue == sid.StringValue);
                if (userAuth != null)
                {
                    userAuth.Delete();
                }

                storage.CloseConnection();
                result = true;

            }
            catch (Exception ex)
            {
                //log ex
            }

            return result;

        }



        /// <summary>
        /// دریافت لیست نقش های کاربر
        /// </summary>
        public IEnumerable<RoleOutput> GetUserRolesByUserName(string Username)
        {
            var userRoles = new List<RoleOutput>();

            try
            {
                IAzManStorage storage = new SqlAzManStorage(_azManConnectionString);
                storage.OpenConnection();
                //دریافت لیست نقش ها
                IAzManItem[] azManRoles = storage[_storageName][_applicationName].GetItems(ItemType.Role);
                storage.CloseConnection();

                var fullUserName = GetUserNameWithoutDomain(Username) + _fullDomainName;
                WindowsIdentity wi = new WindowsIdentity(fullUserName);
                foreach (IAzManItem role in azManRoles)
                {

                    AuthorizationType AuthType = role.CheckAccess(wi, DateTime.Now, new KeyValuePair<string, object>[0]);

                    if (AuthType == AuthorizationType.Allow || AuthType == AuthorizationType.AllowWithDelegation)
                    {
                        AzmanSid sid = new AzmanSid(wi);
                        if (role.GetAuthorizationsOfMember(sid)[0].Attributes.Keys.Contains("IsActive"))
                            if (!Convert.ToBoolean(role.GetAuthorizationsOfMember(sid)[0].Attributes["IsActive"].Value))
                                continue;
                        //اگر نقش برای این کاربر مجاز بود به لیست اضافه شود
                        userRoles.Add(new RoleOutput { Name = role.Name, Description = role.Description });
                    }
                }

            }
            catch (Exception ex)
            {
                //log
            }

            return userRoles;
        }




        /// <summary>
        /// دریافت لیست کل نقش ها
        /// </summary>
        public IEnumerable<RoleOutput> GetAllRoles()
        {
            var roles = new List<RoleOutput>();

            try
            {
                IAzManStorage storage = new SqlAzManStorage(_azManConnectionString);
                storage.OpenConnection();
                IAzManItem[] azRoles = storage[_storageName][_applicationName].GetItems(ItemType.Role);
                storage.CloseConnection();
                roles = azRoles.Select(r => new RoleOutput { Name = r.Name, Description = r.Description }).ToList();

            }
            catch (Exception ex)
            {
                //log ex
            }

            return roles;

        }




        #endregion

        #region Private Methods

        /// <summary>
        /// نام کاربری شخص را از دامین آن جدا کرده و برمیگرداند - sepna\shirbandi ==> shirbandi || shirbandi@sepna  ==> shirbandi
        /// </summary>
        private string GetUserNameWithoutDomain(string UserNamewithDomain)
        {
            try
            {
                if (UserNamewithDomain.ToLower().IndexOf('@') > 0)
                    return UserNamewithDomain.ToLower().Replace("@" + _fullDomainName, "");
                if (UserNamewithDomain.ToLower().IndexOf('\\') > 0)
                    return UserNamewithDomain.ToLower().Split('\\')[1];
                return UserNamewithDomain.ToLower().Split('@').First().Split('\\').Last();
            }
            catch (Exception ex)
            {
                //long ex

            }
            return null;
        }


        #endregion

    }

}
