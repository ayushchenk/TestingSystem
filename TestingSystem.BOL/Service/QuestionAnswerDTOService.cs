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
    public class QuestionAnswerDTOService:IEntityService<QuestionAnswerDTO>
    {
        IGenericRepository<QuestionAnswer> repository;
        readonly IMapper mapper;

        public QuestionAnswerDTOService(IGenericRepository<QuestionAnswer> repos)
        {
            repository = repos;
            mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddExpressionMapping();
                cfg.CreateMap<QuestionAnswer, QuestionAnswerDTO>()
                    .ForMember("QuestionString", opt => opt.MapFrom(answer => answer.Question.QuestionString))
                    .ForMember("TestName", opt => opt.MapFrom(answer => answer.Question.Test.TestName));
                cfg.CreateMap<QuestionAnswerDTO, QuestionAnswer>();
            }).CreateMapper();
        }

        public QuestionAnswerDTO AddOrUpdate(QuestionAnswerDTO obj)
        {
            QuestionAnswer res = repository.AddOrUpdate(mapper.Map<QuestionAnswer>(obj));
            return mapper.Map<QuestionAnswerDTO>(res);
        }

        public QuestionAnswerDTO Delete(QuestionAnswerDTO obj)
        {
            QuestionAnswer res = repository.Delete(mapper.Map<QuestionAnswer>(obj));
            return mapper.Map<QuestionAnswerDTO>(res);
        }

        public IEnumerable<QuestionAnswerDTO> FindBy(Expression<Func<QuestionAnswerDTO, bool>> predicate)
        {
            var expr = mapper.Map<Expression<Func<QuestionAnswerDTO, bool>>, Expression<Func<QuestionAnswer, bool>>>(predicate);
            return repository.FindBy(expr).Select(a => mapper.Map<QuestionAnswerDTO>(a));
        }

        public QuestionAnswerDTO Get(int id)
        {
            return mapper.Map<QuestionAnswerDTO>(repository.Get(id));
        }

        public IEnumerable<QuestionAnswerDTO> GetAll()
        {
            return repository.GetAll().Select(role => mapper.Map<QuestionAnswerDTO>(role));
        }
    }
}
