using AzManCofee;
using System.Linq;

namespace AzMan.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            IAzManService azmanService = new AzManService();
            var roles = azmanService.GetAllRoles();

            System.Console.WriteLine("list of roles : ");

            if (roles.Count()==0)
            {
                System.Console.WriteLine("not role exist");
            }
            else
            {
                foreach (var role in roles)
                {
                    System.Console.WriteLine(role.Name);
                }
            }

            System.Console.WriteLine("please enter a key to exit ... ");
            System.Console.ReadKey();
        }
    }
}
