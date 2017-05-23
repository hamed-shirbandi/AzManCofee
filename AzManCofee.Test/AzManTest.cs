using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AzManCofee;
using System.Linq;

namespace AzMan.Test
{
    [TestClass]
    public class AzManTest
    {
        private   IAzManService _azmanService;


        [TestInitialize]
        public  void Initialize()
        {
            _azmanService = new AzManService();
        }


        [TestMethod]
        public void Can_Check_Role_Exists()
        {
           var result= _azmanService.RoleExists("Fake_Role_Name_To_Test");

            Assert.IsFalse(result);
        }


        [TestMethod]
        public void Can_Check_User_Exists()
        {
            var result = _azmanService.UserExists("Fake_User_Name_To_Test");

            Assert.IsFalse(result);
        }



        [TestMethod]
        public void Can_Get_Roles_List()
        {
            var roles = _azmanService.GetAllRoles();
            Assert.AreNotEqual(0, roles.Count());
        }





    }
}
