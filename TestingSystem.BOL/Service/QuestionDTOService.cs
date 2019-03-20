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
                    .ForMember("TestName", opt => opt.MapFrom(question => question.Test.TestName))
                    .ForMember("SpecializationName", opt => opt.MapFrom(question => question.Specialization.SpecializationName))
                    .ForMember("ImagePath", opt => opt.MapFrom(question => question.QuestionImage.ImagePath))
                    .ForMember("MimeType", opt => opt.MapFrom(question => question.QuestionImage.MimeType));
                cfg.CreateMap<QuestionDTO, Question>();
            }).CreateMapper();
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
