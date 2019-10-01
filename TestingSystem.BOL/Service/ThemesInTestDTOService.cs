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
    public class ThemesInTestDTOService : IEntityService<ThemesInTestDTO>
    {
        IGenericRepository<ThemesInTest> repository;
        readonly IMapper mapper;

        public ThemesInTestDTOService(IGenericRepository<ThemesInTest> repos)
        {
            repository = repos;
            mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddExpressionMapping();
                cfg.CreateMap<ThemesInTest, ThemesInTestDTO>().
                    ForMember("SubjectId", opt => opt.MapFrom(theme => theme.SubjectTheme.SubjectId)).
                    ForMember("ThemeName", opt => opt.MapFrom(theme => theme.SubjectTheme.ThemeName));
                cfg.CreateMap<ThemesInTestDTO, ThemesInTest>();
            }).CreateMapper();
        }

        public void Save()
        {
            repository.Save();
        }

        public ThemesInTestDTO AddOrUpdate(ThemesInTestDTO obj)
        {
            ThemesInTest res = repository.AddOrUpdate(mapper.Map<ThemesInTest>(obj));
            return mapper.Map<ThemesInTestDTO>(res);
        }

        public ThemesInTestDTO Delete(ThemesInTestDTO obj)
        {
            ThemesInTest res = repository.Delete(mapper.Map<ThemesInTest>(obj));
            return mapper.Map<ThemesInTestDTO>(res);
        }

        public IEnumerable<ThemesInTestDTO> FindBy(Expression<Func<ThemesInTestDTO, bool>> predicate)
        {
            var expr = mapper.Map<Expression<Func<ThemesInTestDTO, bool>>, Expression<Func<ThemesInTest, bool>>>(predicate);
            return repository.FindBy(expr).Select(a => mapper.Map<ThemesInTestDTO>(a));
        }

        public ThemesInTestDTO Get(int id)
        {
            return mapper.Map<ThemesInTestDTO>(repository.Get(id));
        }

        public IEnumerable<ThemesInTestDTO> GetAll()
        {
            return repository.GetAll().Select(role => mapper.Map<ThemesInTestDTO>(role));
        }

        public void DeleteRange(IEnumerable<ThemesInTestDTO> items)
        {
            repository.DeleteRange(items.Select(item => mapper.Map<ThemesInTest>(item)));
        }

        public Task DeleteRangeAsync(IEnumerable<ThemesInTestDTO> items)
        {
            return Task.Run(() => DeleteRange(items));
        }

        public Task<IEnumerable<ThemesInTestDTO>> GetAllAsync()
        {
            return Task.Run(() => GetAll());
        }

        public Task<IEnumerable<ThemesInTestDTO>> FindByAsync(Expression<Func<ThemesInTestDTO, bool>> predicate)
        {
            return Task.Run(() => FindBy(predicate));
        }

        public Task<ThemesInTestDTO> GetAsync(int id)
        {
            return Task.Run(() => Get(id));
        }

        public Task<ThemesInTestDTO> AddOrUpdateAsync(ThemesInTestDTO obj)
        {
            return Task.Run(() => AddOrUpdate(obj));
        }

        public Task<ThemesInTestDTO> DeleteAsync(ThemesInTestDTO obj)
        {
            return Task.Run(() => Delete(obj));
        }
    }
}
