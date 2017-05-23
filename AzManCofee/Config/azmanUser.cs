using NetSqlAzMan.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Principal;

namespace Afaran.MedicalWaste.Engine.Application.AzMan.Config
{
    class azmanUser : IAzManDBUser
    {
        AzmanSid _CustomSid;
        string _UserName;
        public IAzManSid CustomSid
        {
            get { return _CustomSid; }
        }

        public string UserName
        {
            get { return _UserName; }
        }

        public azmanUser(WindowsIdentity CustomSid, string UserName)
        {
            try
            {
                _CustomSid = new AzmanSid(CustomSid);
                _UserName = UserName;
            }
            catch (Exception ex)
            {
                //  long 
            }

        }

        public Dictionary<string, object> CustomColumns
        {
            get { throw new NotImplementedException(); }
        }
    }
}
