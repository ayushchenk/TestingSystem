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
    public class SubjectDTOService : IEntityService<SubjectDTO>
    {
        IGenericRepository<Subject> repository;
        readonly IMapper mapper;

        public SubjectDTOService(IGenericRepository<Subject> repos)
        {
            repository = repos;
            mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddExpressionMapping();
                cfg.CreateMap<Subject, SubjectDTO>()
                    .ForMember("SpecializationName", opt => opt.MapFrom(subject => subject.Specialization.SpecializationName))
                    .ForMember("Questions", opt => opt.MapFrom(subject => subject.Questions.Count));
                cfg.CreateMap<SubjectDTO, Subject>()
                    .ForMember("Questions", opt => opt.MapFrom(subject => new HashSet<Question>()));
            }).CreateMapper();
        }

        public SubjectDTO AddOrUpdate(SubjectDTO obj)
        {
            Subject res = repository.AddOrUpdate(mapper.Map<Subject>(obj));
            return mapper.Map<SubjectDTO>(res);
        }

        public SubjectDTO Delete(SubjectDTO obj)
        {
            Subject res = repository.Delete(mapper.Map<Subject>(obj));
            return mapper.Map<SubjectDTO>(res);
        }

        public IEnumerable<SubjectDTO> FindBy(Expression<Func<SubjectDTO, bool>> predicate)
        {
            var expr = mapper.Map<Expression<Func<SubjectDTO, bool>>, Expression<Func<Subject, bool>>>(predicate);
            return repository.FindBy(expr).ToList().Select(a => mapper.Map<SubjectDTO>(a));
        }

        public SubjectDTO Get(int id)
        {
            return mapper.Map<SubjectDTO>(repository.Get(id));
        }

        public IEnumerable<SubjectDTO> GetAll()
        {
            return repository.GetAll().Select(role => mapper.Map<SubjectDTO>(role));
        }

        public Task<IEnumerable<SubjectDTO>> GetAllAsync()
        {
            return Task.Run(() => GetAll());
        }

        public Task<IEnumerable<SubjectDTO>> FindByAsync(Expression<Func<SubjectDTO, bool>> predicate)
        {
            return Task.Run(() => FindBy(predicate));
        }

        public Task<SubjectDTO> GetAsync(int id)
        {
            return Task.Run(() => Get(id));
        }

        public Task<SubjectDTO> AddOrUpdateAsync(SubjectDTO obj)
        {
            return Task.Run(() => AddOrUpdate(obj));
        }

        public Task<SubjectDTO> DeleteAsync(SubjectDTO obj)
        {
            return Task.Run(() => Delete(obj));
        }
    }
}
