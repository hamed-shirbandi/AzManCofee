using AzManCofee.Models;
using System.Collections.Generic;

namespace AzManCofee
{
    public interface IAzManService
    {
        bool UserExists(string userName);
        bool IsInRole(string userName, string roleName);
        bool RoleExists(string roleName);
        bool AddUserToRole(string userName, string roleName);
        bool RemoveUserFromRole(string userName, string roleName);
        IEnumerable<RoleOutput> GetUserRolesByUserName(string userName);
        IEnumerable<RoleOutput> GetAllRoles();
    }
}
