using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using GenericRepository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TestingSystem.BOL.Model;
using TestingSystem.DAL.DbModel;

namespace TestingSystem.BOL.Service
{
    public class GroupsInTestDTOService : IEntityService<GroupsInTestDTO>
    {
        IGenericRepository<GroupsInTest> repository;
        readonly IMapper mapper;

        public GroupsInTestDTOService(IGenericRepository<GroupsInTest> repos)
        {
            repository = repos;
            mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddExpressionMapping();
                cfg.CreateMap<GroupsInTest, GroupsInTestDTO>()
                    .ForMember("GroupName", opt => opt.MapFrom(git => git.Group.GroupName))
                    .ForMember("TestName", opt => opt.MapFrom(git => git.Test.TestName));
                cfg.CreateMap<GroupsInTestDTO, GroupsInTest>();
            }).CreateMapper();
        }

        public GroupsInTestDTO AddOrUpdate(GroupsInTestDTO obj)
        {
            GroupsInTest res = repository.AddOrUpdate(mapper.Map<GroupsInTest>(obj));
            return mapper.Map<GroupsInTestDTO>(res);
        }

        public GroupsInTestDTO Delete(GroupsInTestDTO obj)
        {
            GroupsInTest res = repository.Delete(mapper.Map<GroupsInTest>(obj));
            return mapper.Map<GroupsInTestDTO>(res);
        }

        public IEnumerable<GroupsInTestDTO> FindBy(Expression<Func<GroupsInTestDTO, bool>> predicate)
        {
            var expr = mapper.Map<Expression<Func<GroupsInTestDTO, bool>>, Expression<Func<GroupsInTest, bool>>>(predicate);
            return repository.FindBy(expr).Select(a => mapper.Map<GroupsInTestDTO>(a));
        }

        public GroupsInTestDTO Get(int id)
        {
            return mapper.Map<GroupsInTestDTO>(repository.Get(id));
        }

        public IEnumerable<GroupsInTestDTO> GetAll()
        {
            return repository.GetAll().ToList().Select(role => mapper.Map<GroupsInTestDTO>(role));
        }

        public Task<IEnumerable<GroupsInTestDTO>> GetAllAsync()
        {
            return Task.Run(() => GetAll());
        }

        public Task<IEnumerable<GroupsInTestDTO>> FindByAsync(Expression<Func<GroupsInTestDTO, bool>> predicate)
        {
            return Task.Run(() => FindBy(predicate));
        }

        public Task<GroupsInTestDTO> GetAsync(int id)
        {
            return Task.Run(() => Get(id));
        }

        public Task<GroupsInTestDTO> AddOrUpdateAsync(GroupsInTestDTO obj)
        {
            return Task.Run(() => AddOrUpdate(obj));
        }

        public Task<GroupsInTestDTO> DeleteAsync(GroupsInTestDTO obj)
        {
            return Task.Run(() => Delete(obj));
        }
    }
}
