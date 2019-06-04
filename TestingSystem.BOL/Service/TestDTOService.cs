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
    public class TestDTOService : IEntityService<TestDTO>
    {
        IGenericRepository<Test> repository;
        readonly IMapper mapper;

        public TestDTOService(IGenericRepository<Test> repos)
        {
            repository = repos;
            mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddExpressionMapping();
                cfg.CreateMap<Test, TestDTO>()
                    .ForMember("SpecializationName", opt => opt.MapFrom(test => test.Specialization.SpecializationName));
                cfg.CreateMap<TestDTO, Test>();
            }).CreateMapper();
        }

        public TestDTO AddOrUpdate(TestDTO obj)
        {
            Test res = repository.AddOrUpdate(mapper.Map<Test>(obj));
            return mapper.Map<TestDTO>(res);
        }

        public TestDTO Delete(TestDTO obj)
        {
            Test res = repository.Delete(mapper.Map<Test>(obj));
            return mapper.Map<TestDTO>(res);
        }

        public IEnumerable<TestDTO> FindBy(Expression<Func<TestDTO, bool>> predicate)
        {
            var expr = mapper.Map<Expression<Func<TestDTO, bool>>, Expression<Func<Test, bool>>>(predicate);
            return repository.FindBy(expr).Select(a => mapper.Map<TestDTO>(a));
        }

        public TestDTO Get(int id)
        {
            return mapper.Map<TestDTO>(repository.Get(id));
        }

        public IEnumerable<TestDTO> GetAll()
        {
            return repository.GetAll().Select(test => mapper.Map<TestDTO>(test));
        }

        public Task<IEnumerable<TestDTO>> GetAllAsync()
        {
            return Task.Run(() => GetAll());
        }

        public Task<IEnumerable<TestDTO>> FindByAsync(Expression<Func<TestDTO, bool>> predicate)
        {
            return Task.Run(() => FindBy(predicate));
        }

        public Task<TestDTO> GetAsync(int id)
        {
            return Task.Run(() => Get(id));
        }

        public Task<TestDTO> AddOrUpdateAsync(TestDTO obj)
        {
            return Task.Run(() => AddOrUpdate(obj));
        }

        public Task<TestDTO> DeleteAsync(TestDTO obj)
        {
            return Task.Run(() => Delete(obj));
        }
    }
}
