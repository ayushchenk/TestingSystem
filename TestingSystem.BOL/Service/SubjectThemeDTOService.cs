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
    public class SubjectThemeDTOService : IEntityService<SubjectThemeDTO>
    {
        IGenericRepository<SubjectTheme> repository;
        readonly IMapper mapper;

        public SubjectThemeDTOService(IGenericRepository<SubjectTheme> repos)
        {
            repository = repos;
            mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddExpressionMapping();
                cfg.CreateMap<SubjectTheme, SubjectThemeDTO>()
                    .ForMember("SubjectName", opt => opt.MapFrom(subject => subject.Subject.SubjectName))
                    .ForMember("Questions", opt => opt.MapFrom(subject => subject.Questions.Count));
                cfg.CreateMap<SubjectThemeDTO, SubjectTheme>();
            }).CreateMapper();
        }

        public void Save()
        {
            repository.Save();
        }

        public SubjectThemeDTO AddOrUpdate(SubjectThemeDTO obj)
        {
            SubjectTheme res = repository.AddOrUpdate(mapper.Map<SubjectTheme>(obj));
            return mapper.Map<SubjectThemeDTO>(res);
        }

        public SubjectThemeDTO Delete(SubjectThemeDTO obj)
        {
            SubjectTheme res = repository.Delete(mapper.Map<SubjectTheme>(obj));
            return mapper.Map<SubjectThemeDTO>(res);
        }

        public IEnumerable<SubjectThemeDTO> FindBy(Expression<Func<SubjectThemeDTO, bool>> predicate)
        {
            var expr = mapper.Map<Expression<Func<SubjectThemeDTO, bool>>, Expression<Func<SubjectTheme, bool>>>(predicate);
            return repository.FindBy(expr).Select(a => mapper.Map<SubjectThemeDTO>(a));
        }

        public SubjectThemeDTO Get(int id)
        {
            return mapper.Map<SubjectThemeDTO>(repository.Get(id));
        }

        public IEnumerable<SubjectThemeDTO> GetAll()
        {
            return repository.GetAll().Select(role => mapper.Map<SubjectThemeDTO>(role));
        }

        public void DeleteRange(IEnumerable<SubjectThemeDTO> items)
        {
            repository.DeleteRange(items.Select(item => mapper.Map<SubjectTheme>(item)));
        }

        public Task DeleteRangeAsync(IEnumerable<SubjectThemeDTO> items)
        {
            return Task.Run(() => DeleteRange(items));
        }

        public Task<IEnumerable<SubjectThemeDTO>> GetAllAsync()
        {
            return Task.Run(() => GetAll());
        }

        public Task<IEnumerable<SubjectThemeDTO>> FindByAsync(Expression<Func<SubjectThemeDTO, bool>> predicate)
        {
            return Task.Run(() => FindBy(predicate));
        }

        public Task<SubjectThemeDTO> GetAsync(int id)
        {
            return Task.Run(() => Get(id));
        }

        public Task<SubjectThemeDTO> AddOrUpdateAsync(SubjectThemeDTO obj)
        {
            return Task.Run(() => AddOrUpdate(obj));
        }

        public Task<SubjectThemeDTO> DeleteAsync(SubjectThemeDTO obj)
        {
            return Task.Run(() => Delete(obj));
        }
    }
}
