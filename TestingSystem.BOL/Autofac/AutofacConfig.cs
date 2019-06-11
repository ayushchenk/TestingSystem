using Autofac;
using GenericRepository.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestingSystem.BOL.Model;
using TestingSystem.BOL.Service;
using TestingSystem.DAL.DbModel;
using TestingSystem.Repository.Repositories;

namespace TestingSystem.BOL.Autofac
{
    public class AutofacConfig:Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<TestingSystemContext>().As<DbContext>().InstancePerLifetimeScope();

            builder.RegisterType<GroupRepository>().As<IGenericRepository<Group>>().InstancePerRequest();
            builder.RegisterType<GroupsInTestRepository>().As<IGenericRepository<GroupsInTest>>().InstancePerRequest();
            builder.RegisterType<QuestionAnswerRepository>().As<IGenericRepository<QuestionAnswer>>().InstancePerRequest();
            builder.RegisterType<QuestionRepository>().As<IGenericRepository<Question>>().InstancePerRequest();
            builder.RegisterType<QuestionImageRepository>().As<IGenericRepository<QuestionImage>>().InstancePerRequest();
            builder.RegisterType<SpecializationRepository>().As<IGenericRepository<Specialization>>().InstancePerRequest();
            builder.RegisterType<TestRepository>().As<IGenericRepository<Test>>().InstancePerRequest();
            builder.RegisterType<UserRepository>().As<IGenericRepository<User>>().InstancePerRequest();
            builder.RegisterType<EducationUnitRepository>().As<IGenericRepository<EducationUnit>>().InstancePerRequest();
            builder.RegisterType<SubjectRepository>().As<IGenericRepository<Subject>>().InstancePerRequest();

            builder.RegisterType<GroupDTOService>().As<IEntityService<GroupDTO>>().InstancePerRequest();
            builder.RegisterType<GroupsInTestDTOService>().As<IEntityService<GroupsInTestDTO>>().InstancePerRequest();
            builder.RegisterType<QuestionAnswerDTOService>().As<IEntityService<QuestionAnswerDTO>>().InstancePerRequest();
            builder.RegisterType<QuestionDTOService>().As<IEntityService<QuestionDTO>>().InstancePerRequest();
            builder.RegisterType<QuestionImageDTOService>().As<IEntityService<QuestionImageDTO>>().InstancePerRequest();
            builder.RegisterType<SpecializationDTOService>().As<IEntityService<SpecializationDTO>>().InstancePerRequest();
            builder.RegisterType<TestDTOService>().As<IEntityService<TestDTO>>().InstancePerRequest();
            builder.RegisterType<UserDTOService>().As<IEntityService<UserDTO>>().InstancePerRequest();
            builder.RegisterType<EducationUnitDTOService>().As<IEntityService<EducationUnitDTO>>().InstancePerRequest();
            builder.RegisterType<SubjectDTOService>().As<IEntityService<SubjectDTO>>().InstancePerRequest();
        }
    }
}
