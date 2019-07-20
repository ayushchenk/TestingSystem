using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using GenericRepository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TestingSystem.BusinessModel.Model;
using TestingSystem.DAL.DbModel;

namespace TestingSystem.BOL.Service
{
    public class AdminDTOService : IEntityService<AdminDTO>
    {
        IGenericRepository<Admin> repository;
        readonly IMapper mapper;

        public AdminDTOService(IGenericRepository<Admin> repos)
        {
            repository = repos;
            mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddExpressionMapping();
                cfg.CreateMap<Admin, AdminDTO>()
                    .ForMember("EducationUnitId", opt => opt.MapFrom(admin => admin.EducationUnit.Id))
                    .ForMember("EducationUnitName", opt => opt.MapFrom(admin => admin.EducationUnit.EducationUnitName));
                cfg.CreateMap<AdminDTO, Admin>();
            }).CreateMapper();
        }

        public AdminDTO AddOrUpdate(AdminDTO obj)
        {
            Admin res = repository.AddOrUpdate(mapper.Map<Admin>(obj));
            return mapper.Map<AdminDTO>(res);
        }

        public AdminDTO Delete(AdminDTO obj)
        {
            Admin res = repository.Delete(mapper.Map<Admin>(obj));
            return mapper.Map<AdminDTO>(res);
        }

        public IEnumerable<AdminDTO> FindBy(Expression<Func<AdminDTO, bool>> predicate)
        {
            var expr = mapper.Map<Expression<Func<AdminDTO, bool>>, Expression<Func<Admin, bool>>>(predicate);
            return repository.FindBy(expr).Select(a => mapper.Map<AdminDTO>(a));
        }

        public AdminDTO Get(int id)
        {
            return mapper.Map<AdminDTO>(repository.Get(id));
        }

        public IEnumerable<AdminDTO> GetAll()
        {
            return repository.GetAll().Select(role => mapper.Map<AdminDTO>(role));
        }

        public Task<IEnumerable<AdminDTO>> GetAllAsync()
        {
            return Task.Run(() => GetAll());
        }

        public Task<IEnumerable<AdminDTO>> FindByAsync(Expression<Func<AdminDTO, bool>> predicate)
        {
            return Task.Run(() => FindBy(predicate));
        }

        public Task<AdminDTO> GetAsync(int id)
        {
            return Task.Run(() => Get(id));
        }

        public Task<AdminDTO> AddOrUpdateAsync(AdminDTO obj)
        {
            return Task.Run(() => AddOrUpdate(obj));
        }

        public Task<AdminDTO> DeleteAsync(AdminDTO obj)
        {
            return Task.Run(() => Delete(obj));
        }
    }
}
