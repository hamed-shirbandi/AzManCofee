using AzManCofee;


namespace AzMan.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            IAzManService azmanService = new AzManService();
            var roles = azmanService.GetAllRoles();
            foreach (var role in roles)
            {
                System.Console.WriteLine(role.Name);
            }

            System.Console.ReadKey();
        }
    }
}
