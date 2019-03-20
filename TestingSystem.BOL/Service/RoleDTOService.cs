using AutoMapper;
using GenericRepository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using TestingSystem.BOL.Model;
using TestingSystem.DAL.DbModel;
using AutoMapper.Extensions.ExpressionMapping;
using System.Threading.Tasks;

namespace TestingSystem.BOL.Service
{
    public class RoleDTOService : IEntityService<RoleDTO>
    {
        IGenericRepository<Role> repository;
        readonly IMapper mapper;

        public RoleDTOService(IGenericRepository<Role> repos)
        {
            repository = repos;
            mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddExpressionMapping();
                cfg.CreateMap<Role, RoleDTO>();
                cfg.CreateMap<RoleDTO, Role>();
            }).CreateMapper();
        }

        public RoleDTO AddOrUpdate(RoleDTO obj)
        {
            Role res = repository.AddOrUpdate(mapper.Map<Role>(obj));
            return mapper.Map<RoleDTO>(res);
        }

        public RoleDTO Delete(RoleDTO obj)
        {
            Role res = repository.Delete(mapper.Map<Role>(obj));
            return mapper.Map<RoleDTO>(res);
        }

        public IEnumerable<RoleDTO> FindBy(Expression<Func<RoleDTO, bool>> predicate)
        {
            var expr = mapper.Map<Expression<Func<RoleDTO, bool>>, Expression<Func<Role, bool>>>(predicate);
            return repository.FindBy(expr).Select(a => mapper.Map<RoleDTO>(a));
        }

        public RoleDTO Get(int id)
        {
            return mapper.Map<RoleDTO>(repository.Get(id));
        }

        public IEnumerable<RoleDTO> GetAll()
        {
            return repository.GetAll().Select(role => mapper.Map<RoleDTO>(role));
        }

        public Task<IEnumerable<RoleDTO>> GetAllAsync()
        {
            return Task.Run(() => GetAll());
        }

        public Task<IEnumerable<RoleDTO>> FindByAsync(Expression<Func<RoleDTO, bool>> predicate)
        {
            return Task.Run(() => FindBy(predicate));
        }

        public Task<RoleDTO> GetAsync(int id)
        {
            return Task.Run(() => Get(id));
        }

        public Task<RoleDTO> AddOrUpdateAsync(RoleDTO obj)
        {
            return Task.Run(() => AddOrUpdate(obj));
        }

        public Task<RoleDTO> DeleteAsync(RoleDTO obj)
        {
            return Task.Run(() => Delete(obj));
        }
    }
}
