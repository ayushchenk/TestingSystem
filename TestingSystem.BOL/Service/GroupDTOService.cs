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
    public class GroupDTOService : IEntityService<GroupDTO>
    {
        IGenericRepository<Group> repository;
        readonly IMapper mapper;

        public GroupDTOService(IGenericRepository<Group> repos)
        {
            repository = repos;
            mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddExpressionMapping();
                cfg.CreateMap<Group, GroupDTO>()
                    .ForMember("SpecializationName", opt => opt.MapFrom(group => group.Specialization.SpecializationName));
                cfg.CreateMap<GroupDTO, Group>();
            }).CreateMapper();
        }

        public GroupDTO AddOrUpdate(GroupDTO obj)
        {
            Group res = repository.AddOrUpdate(mapper.Map<Group>(obj));
            return mapper.Map<GroupDTO>(res);
        }

        public GroupDTO Delete(GroupDTO obj)
        {
            Group res = repository.Delete(mapper.Map<Group>(obj));
            return mapper.Map<GroupDTO>(res);
        }

        public IEnumerable<GroupDTO> FindBy(Expression<Func<GroupDTO, bool>> predicate)
        {
            var expr = mapper.Map<Expression<Func<GroupDTO, bool>>, Expression<Func<Group, bool>>>(predicate);
            return repository.FindBy(expr).ToList().Select(a => mapper.Map<GroupDTO>(a));
        }

        public GroupDTO Get(int id)
        {
            return mapper.Map<GroupDTO>(repository.Get(id));
        }

        public IEnumerable<GroupDTO> GetAll()
        {
            return repository.GetAll().ToList().Select(group => mapper.Map<GroupDTO>(group));
        }

        public Task<IEnumerable<GroupDTO>> GetAllAsync()
        {
            return Task.Run(() => GetAll());
        }

        public Task<IEnumerable<GroupDTO>> FindByAsync(Expression<Func<GroupDTO, bool>> predicate)
        {
            return Task.Run(() => FindBy(predicate));
        }

        public Task<GroupDTO> GetAsync(int id)
        {
            return Task.Run(() => Get(id));
        }

        public Task<GroupDTO> AddOrUpdateAsync(GroupDTO obj)
        {
            return Task.Run(() => AddOrUpdate(obj));
        }

        public Task<GroupDTO> DeleteAsync(GroupDTO obj)
        {
            return Task.Run(() => Delete(obj));
        }
    }
}
