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
    public class QuestionAnswerDTOService : IEntityService<QuestionAnswerDTO>
    {
        IGenericRepository<QuestionAnswer> repository;
        readonly IMapper mapper;

        public QuestionAnswerDTOService(IGenericRepository<QuestionAnswer> repos)
        {
            repository = repos;
            mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddExpressionMapping();
                cfg.CreateMap<QuestionAnswer, QuestionAnswerDTO>();
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

        public Task<IEnumerable<QuestionAnswerDTO>> GetAllAsync()
        {
            return Task.Run(() => GetAll());
        }

        public Task<IEnumerable<QuestionAnswerDTO>> FindByAsync(Expression<Func<QuestionAnswerDTO, bool>> predicate)
        {
            return Task.Run(() => FindBy(predicate));
        }

        public Task<QuestionAnswerDTO> GetAsync(int id)
        {
            return Task.Run(() => Get(id));
        }

        public Task<QuestionAnswerDTO> AddOrUpdateAsync(QuestionAnswerDTO obj)
        {
            return Task.Run(() => AddOrUpdate(obj));
        }

        public Task<QuestionAnswerDTO> DeleteAsync(QuestionAnswerDTO obj)
        {
            return Task.Run(() => Delete(obj));
        }
    }
}
