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
    public class EducationUnitDTOService : IEntityService<EducationUnitDTO>
    {
        IGenericRepository<EducationUnit> repository;
        readonly IMapper mapper;

        public EducationUnitDTOService(IGenericRepository<EducationUnit> repos)
        {
            repository = repos;
            mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddExpressionMapping();
                cfg.CreateMap<EducationUnit, EducationUnitDTO>();
                cfg.CreateMap<EducationUnitDTO, EducationUnit>();
            }).CreateMapper();
        }

        public void Save()
        {
            repository.Save();
        }

        public EducationUnitDTO AddOrUpdate(EducationUnitDTO obj)
        {
            EducationUnit res = repository.AddOrUpdate(mapper.Map<EducationUnit>(obj));
            return mapper.Map<EducationUnitDTO>(res);
        }

        public EducationUnitDTO Delete(EducationUnitDTO obj)
        {
            EducationUnit res = repository.Delete(mapper.Map<EducationUnit>(obj));
            return mapper.Map<EducationUnitDTO>(res);
        }

        public IEnumerable<EducationUnitDTO> FindBy(Expression<Func<EducationUnitDTO, bool>> predicate)
        {
            var expr = mapper.Map<Expression<Func<EducationUnitDTO, bool>>, Expression<Func<EducationUnit, bool>>>(predicate);
            return repository.FindBy(expr).Select(a => mapper.Map<EducationUnitDTO>(a));
        }

        public EducationUnitDTO Get(int id)
        {
            return mapper.Map<EducationUnitDTO>(repository.Get(id));
        }

        public IEnumerable<EducationUnitDTO> GetAll()
        {
            return repository.GetAll().Select(group => mapper.Map<EducationUnitDTO>(group));
        }

        public void DeleteRange(IEnumerable<EducationUnitDTO> items)
        {
            repository.DeleteRange(items.Select(item => mapper.Map<EducationUnit>(item)));
        }

        public Task DeleteRangeAsync(IEnumerable<EducationUnitDTO> items)
        {
            return Task.Run(() => DeleteRange(items));
        }

        public Task<IEnumerable<EducationUnitDTO>> GetAllAsync()
        {
            return Task.Run(() => GetAll());
        }

        public Task<IEnumerable<EducationUnitDTO>> FindByAsync(Expression<Func<EducationUnitDTO, bool>> predicate)
        {
            return Task.Run(() => FindBy(predicate));
        }

        public Task<EducationUnitDTO> GetAsync(int id)
        {
            return Task.Run(() => Get(id));
        }

        public Task<EducationUnitDTO> AddOrUpdateAsync(EducationUnitDTO obj)
        {
            return Task.Run(() => AddOrUpdate(obj));
        }

        public Task<EducationUnitDTO> DeleteAsync(EducationUnitDTO obj)
        {
            return Task.Run(() => Delete(obj));
        }
    }
}
