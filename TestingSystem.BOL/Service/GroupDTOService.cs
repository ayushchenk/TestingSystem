using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using GenericRepository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using TestingSystem.BOL.Model;
using TestingSystem.DAL.DbModel;

namespace TestingSystem.BOL.Service
{
    public class GroupDTOService:IEntityService<GroupDTO>
    {
        IGenericRepository<Group> repository;
        readonly IMapper mapper;

        public GroupDTOService(IGenericRepository<Group> repos)
        {
            repository = repos;
            mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddExpressionMapping();
                cfg.CreateMap<Group, GroupDTO>();
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
            return repository.FindBy(expr).Select(a => mapper.Map<GroupDTO>(a));
        }

        public GroupDTO Get(int id)
        {
            return mapper.Map<GroupDTO>(repository.Get(id));
        }

        public IEnumerable<GroupDTO> GetAll()
        {
            return repository.GetAll().Select(role => mapper.Map<GroupDTO>(role));
        }
    }
}
