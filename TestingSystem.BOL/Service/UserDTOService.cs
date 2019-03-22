using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using GenericRepository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TestingSystem.BOL.Model;
using TestingSystem.DAL.DbModel;

namespace TestingSystem.BOL.Service
{
    public class UserDTOService : IEntityService<UserDTO>
    {
        IGenericRepository<User> repository;
        readonly IMapper mapper;

        public UserDTOService(IGenericRepository<User> repos)
        {
            repository = repos;
            mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddExpressionMapping();
                cfg.CreateMap<User, UserDTO>()
                    .ForMember("GroupName", opt => opt.MapFrom(user => user.Group.GroupName))
                    .ForMember("SpecializationId", opt => opt.MapFrom(user => user.Group.Specialization.Id))
                    .ForMember("SpecializationName", opt => opt.MapFrom(user => user.Group.Specialization.SpecializationName));
                cfg.CreateMap<UserDTO, User>();
            }).CreateMapper();
        }

        public UserDTO AddOrUpdate(UserDTO obj)
        {
            User res = repository.AddOrUpdate(mapper.Map<User>(obj));
            return mapper.Map<UserDTO>(res);
        }

        public UserDTO Delete(UserDTO obj)
        {
            User res = repository.Delete(mapper.Map<User>(obj));
            return mapper.Map<UserDTO>(res);
        }

        public IEnumerable<UserDTO> FindBy(Expression<Func<UserDTO, bool>> predicate)
        {
            var expr = mapper.Map<Expression<Func<UserDTO, bool>>, Expression<Func<User, bool>>>(predicate);
            return repository.FindBy(expr).Select(a => mapper.Map<UserDTO>(a));
        }

        public UserDTO Get(int id)
        {
            return mapper.Map<UserDTO>(repository.Get(id));
        }

        public IEnumerable<UserDTO> GetAll()
        {
            return repository.GetAll().Select(role => mapper.Map<UserDTO>(role));
        }

        public Task<IEnumerable<UserDTO>> GetAllAsync()
        {
            return Task.Run(() => GetAll());
        }

        public Task<IEnumerable<UserDTO>> FindByAsync(Expression<Func<UserDTO, bool>> predicate)
        {
            return Task.Run(() => FindBy(predicate));
        }

        public Task<UserDTO> GetAsync(int id)
        {
            return Task.Run(() => Get(id));
        }

        public Task<UserDTO> AddOrUpdateAsync(UserDTO obj)
        {
            return Task.Run(() => AddOrUpdate(obj));
        }

        public Task<UserDTO> DeleteAsync(UserDTO obj)
        {
            return Task.Run(() => Delete(obj));
        }
    }
}
