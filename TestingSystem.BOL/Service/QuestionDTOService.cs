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
    public class QuestionDTOService:IEntityService<QuestionDTO>
    {
        IGenericRepository<Question> repository;
        readonly IMapper mapper;

        public QuestionDTOService(IGenericRepository<Question> repos)
        {
            repository = repos;
            mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddExpressionMapping();
                cfg.CreateMap<Question, QuestionDTO>()
                    .ForMember("SpecializationName", opt => opt.MapFrom(question => question.Subject.Specialization.SpecializationName))
                    .ForMember("SpecializationId", opt => opt.MapFrom(question => question.Subject.Specialization.Id))
                    .ForMember("SubjectName", opt => opt.MapFrom(question => question.Subject.SubjectName))
                    .ForMember("ThemeName", opt => opt.MapFrom(question => question.SubjectTheme.ThemeName))
                    .ForMember("ImagePath", opt => opt.MapFrom(question => question.QuestionImage.ImagePath));
                cfg.CreateMap<QuestionDTO, Question>();
            }).CreateMapper();
        }

        public void Save()
        {
            repository.Save();
        }

        public QuestionDTO AddOrUpdate(QuestionDTO obj)
        {
            Question res = repository.AddOrUpdate(mapper.Map<Question>(obj));
            return mapper.Map<QuestionDTO>(res);
        }

        public QuestionDTO Delete(QuestionDTO obj)
        {
            Question res = repository.Delete(mapper.Map<Question>(obj));
            return mapper.Map<QuestionDTO>(res);
        }

        public IEnumerable<QuestionDTO> FindBy(Expression<Func<QuestionDTO, bool>> predicate)
        {
            var expr = mapper.Map<Expression<Func<QuestionDTO, bool>>, Expression<Func<Question, bool>>>(predicate);
            return repository.FindBy(expr).Select(a => mapper.Map<QuestionDTO>(a));
        }

        public QuestionDTO Get(int id)
        {
            return mapper.Map<QuestionDTO>(repository.Get(id));
        }

        public IEnumerable<QuestionDTO> GetAll()
        {
            return repository.GetAll().Select(role => mapper.Map<QuestionDTO>(role));
        }

        public void DeleteRange(IEnumerable<QuestionDTO> items)
        {
            repository.DeleteRange(items.Select(item => mapper.Map<Question>(item)));
        }

        public Task DeleteRangeAsync(IEnumerable<QuestionDTO> items)
        {
            return Task.Run(() => DeleteRange(items));
        }

        public Task<IEnumerable<QuestionDTO>> GetAllAsync()
        {
            return Task.Run(() => GetAll());
        }

        public Task<IEnumerable<QuestionDTO>> FindByAsync(Expression<Func<QuestionDTO, bool>> predicate)
        {
            return Task.Run(() => FindBy(predicate));
        }

        public Task<QuestionDTO> GetAsync(int id)
        {
            return Task.Run(() => Get(id));
        }

        public Task<QuestionDTO> AddOrUpdateAsync(QuestionDTO obj)
        {
            return Task.Run(() => AddOrUpdate(obj));
        }

        public Task<QuestionDTO> DeleteAsync(QuestionDTO obj)
        {
            return Task.Run(() => Delete(obj));
        }
    }
}
