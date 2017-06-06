using NetSqlAzMan.Interfaces;
using System;
using System.Security.Principal;

namespace AzManCofee.Config
{
   public class AzmanSid : IAzManSid
    {
        byte[] _BinaryValue;
        string _StringValue;

        public byte[] BinaryValue
        {
            get { return _BinaryValue; }
        }

        public string StringValue
        {
            get { return _StringValue; }
        }

        public AzmanSid(WindowsIdentity CustomSid)
        {
            try
            {
                _BinaryValue = new byte[CustomSid.User.BinaryLength];
                CustomSid.User.GetBinaryForm(_BinaryValue, 0);
                _StringValue = CustomSid.User.Value;
            }
            catch (Exception ex)
            {
                // long 
            }

        }

        public bool Equals(IAzManSid other)
        {
            throw new NotImplementedException();
        }
    }
}
