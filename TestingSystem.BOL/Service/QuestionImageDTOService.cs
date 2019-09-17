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
    public class QuestionImageDTOService : IEntityService<QuestionImageDTO>
    {
        IGenericRepository<QuestionImage> repository;
        readonly IMapper mapper;

        public QuestionImageDTOService(IGenericRepository<QuestionImage> repos)
        {
            repository = repos;
            mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddExpressionMapping();
                cfg.CreateMap<QuestionImage, QuestionImageDTO>();
                cfg.CreateMap<QuestionImageDTO, QuestionImage>();
            }).CreateMapper();
        }

        public void Save()
        {
            repository.Save();
        }

        public QuestionImageDTO AddOrUpdate(QuestionImageDTO obj)
        {
            QuestionImage res = repository.AddOrUpdate(mapper.Map<QuestionImage>(obj));
            return mapper.Map<QuestionImageDTO>(res);
        }

        public QuestionImageDTO Delete(QuestionImageDTO obj)
        {
            QuestionImage res = repository.Delete(mapper.Map<QuestionImage>(obj));
            return mapper.Map<QuestionImageDTO>(res);
        }

        public IEnumerable<QuestionImageDTO> FindBy(Expression<Func<QuestionImageDTO, bool>> predicate)
        {
            var expr = mapper.Map<Expression<Func<QuestionImageDTO, bool>>, Expression<Func<QuestionImage, bool>>>(predicate);
            return repository.FindBy(expr).Select(a => mapper.Map<QuestionImageDTO>(a));
        }

        public QuestionImageDTO Get(int id)
        {
            return mapper.Map<QuestionImageDTO>(repository.Get(id));
        }

        public IEnumerable<QuestionImageDTO> GetAll()
        {
            return repository.GetAll().Select(role => mapper.Map<QuestionImageDTO>(role));
        }

        public void DeleteRange(IEnumerable<QuestionImageDTO> items)
        {
            repository.DeleteRange(items.Select(item => mapper.Map<QuestionImage>(item)));
        }

        public Task DeleteRangeAsync(IEnumerable<QuestionImageDTO> items)
        {
            return Task.Run(() => DeleteRange(items));
        }

        public Task<IEnumerable<QuestionImageDTO>> GetAllAsync()
        {
            return Task.Run(() => GetAll());
        }

        public Task<IEnumerable<QuestionImageDTO>> FindByAsync(Expression<Func<QuestionImageDTO, bool>> predicate)
        {
            return Task.Run(() => FindBy(predicate));
        }

        public Task<QuestionImageDTO> GetAsync(int id)
        {
            return Task.Run(() => Get(id));
        }

        public Task<QuestionImageDTO> AddOrUpdateAsync(QuestionImageDTO obj)
        {
            return Task.Run(() => AddOrUpdate(obj));
        }

        public Task<QuestionImageDTO> DeleteAsync(QuestionImageDTO obj)
        {
            return Task.Run(() => Delete(obj));
        }
    }
}
