using GenericRepository.Common;
using System.Linq;
using TestingSystem.BOL.Model;
using TestingSystem.BOL.Service;
using TestingSystem.DAL.DbModel;
using TestingSystem.Repository.Repositories;

namespace TestingSystem.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            TestingSystemContext context = new TestingSystemContext();
            //context.Specializations.Add(new Specialization { SpecializationName = "Somespec"});
            //context.SaveChanges();
            //context.Groups.Add(new Group { GroupName = "FOAIS 1.6", SpecializationId =  });
            //context.SaveChanges();
                
            UsingContext(context);
            System.Console.WriteLine(new string('=', 50));
            UsingRepository(context);
            System.Console.WriteLine(new string('=', 50));
            UsingServices(context);

            //var gg = service2.GetAll().First();

            //System.Console.WriteLine("Done");
            System.Console.ReadKey();
        }

        #region UsingContext
        private static void UsingContext(TestingSystemContext context)
        {
            System.Console.WriteLine("Using context:");
            System.Console.WriteLine("Speicalizations:");
            foreach (var spec in context.Specializations)
                System.Console.WriteLine($"{spec.Id} \t {spec.SpecializationName}");
            System.Console.WriteLine("Groups:");
            foreach (var group in context.Groups)
                System.Console.WriteLine($"{group.Id} \t {group.GroupName} \t {group.Specialization.SpecializationName}");
        }
        #endregion
        #region UsingRepository
        private static void UsingRepository(TestingSystemContext context)
        {
            IGenericRepository<Group> groupRepos = new GroupRepository(context);

            IGenericRepository<Specialization> specRepos = new SpecializationRepository(context);

            System.Console.WriteLine("Using repositories:");
            System.Console.WriteLine("Speicalizations:");
            foreach (var spec in specRepos.GetAll())
                System.Console.WriteLine($"{spec.Id} \t {spec.SpecializationName}");
            System.Console.WriteLine("Groups:");
            foreach (var group in groupRepos.GetAll())
                System.Console.WriteLine($"{group.Id} \t {group.GroupName} \t {group.Specialization.SpecializationName}");
        }
        #endregion
        #region UsingServicesAsync
        private static async void UsingServices(TestingSystemContext context)
        {
            IGenericRepository<Group> groupRepos = new GroupRepository(context);
            IEntityService<GroupDTO> groupService = new GroupDTOService(groupRepos);

            IGenericRepository<Specialization> specRepos = new SpecializationRepository(context);
            IEntityService<SpecializationDTO> specService = new SpecializationDTOService(specRepos);

            System.Console.WriteLine("Using services:");
            System.Console.WriteLine("Speicalizations:");
            foreach (var spec in await specService.GetAllAsync())
                System.Console.WriteLine($"{spec.Id} \t {spec.SpecializationName}");
            System.Console.WriteLine("Groups:");
            foreach (var group in await groupService.GetAllAsync())
                System.Console.WriteLine($"{group.Id} \t {group.GroupName} \t {group.SpecializationName}");
        }
        #endregion
    }
}