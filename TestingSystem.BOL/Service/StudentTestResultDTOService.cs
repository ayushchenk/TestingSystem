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
    public class StudentTestResultDTOService:IEntityService<StudentTestResultDTO>
    {
        IGenericRepository<StudentTestResult> repository;
        readonly IMapper mapper;

        public StudentTestResultDTOService(IGenericRepository<StudentTestResult> repos)
        {
            repository = repos;
            mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddExpressionMapping();
                cfg.CreateMap<StudentTestResult, StudentTestResultDTO>()
                    .ForMember("GroupName", opt => opt.MapFrom(str => str.GroupsInTest.Group.GroupName))
                    .ForMember("TestName", opt => opt.MapFrom(str => str.GroupsInTest.Test.TestName))
                    .ForMember("QuestionCount", opt => opt.MapFrom(str => str.GroupsInTest.Test.EasyCount + str.GroupsInTest.Test.MediumCount + str.GroupsInTest.Test.HardCount))
                    .ForMember("SubjectName", opt => opt.MapFrom(str => str.GroupsInTest.Test.Subject.SubjectName))
                    .ForMember("GroupId", opt => opt.MapFrom(str => str.GroupsInTest.Group.Id))
                    .ForMember("TestId", opt => opt.MapFrom(str => str.GroupsInTest.Test.Id))
                    .ForMember("StartDate", opt => opt.MapFrom(str => str.GroupsInTest.StartDate))
                    .ForMember("SubjectId", opt => opt.MapFrom(str => str.GroupsInTest.Test.Subject.Id));
                cfg.CreateMap<StudentTestResultDTO, StudentTestResult>();
            }).CreateMapper();
        }

        public void Save()
        {
            repository.Save();
        }

        public StudentTestResultDTO AddOrUpdate(StudentTestResultDTO obj)
        {
            StudentTestResult res = repository.AddOrUpdate(mapper.Map<StudentTestResult>(obj));
            return mapper.Map<StudentTestResultDTO>(res);
        }

        public StudentTestResultDTO Delete(StudentTestResultDTO obj)
        {
            StudentTestResult res = repository.Delete(mapper.Map<StudentTestResult>(obj));
            return mapper.Map<StudentTestResultDTO>(res);
        }

        public IEnumerable<StudentTestResultDTO> FindBy(Expression<Func<StudentTestResultDTO, bool>> predicate)
        {
            var expr = mapper.Map<Expression<Func<StudentTestResultDTO, bool>>, Expression<Func<StudentTestResult, bool>>>(predicate);
            return repository.FindBy(expr).Select(a => mapper.Map<StudentTestResultDTO>(a));
        }

        public StudentTestResultDTO Get(int id)
        {
            return mapper.Map<StudentTestResultDTO>(repository.Get(id));
        }

        public IEnumerable<StudentTestResultDTO> GetAll()
        {
            return repository.GetAll().Select(str => mapper.Map<StudentTestResultDTO>(str));
        }

        public void DeleteRange(IEnumerable<StudentTestResultDTO> items)
        {
            repository.DeleteRange(items.Select(item => mapper.Map<StudentTestResult>(item)));
        }

        public Task DeleteRangeAsync(IEnumerable<StudentTestResultDTO> items)
        {
            return Task.Run(() => DeleteRange(items));
        }

        public Task<IEnumerable<StudentTestResultDTO>> GetAllAsync()
        {
            return Task.Run(() => GetAll());
        }

        public Task<IEnumerable<StudentTestResultDTO>> FindByAsync(Expression<Func<StudentTestResultDTO, bool>> predicate)
        {
            return Task.Run(() => FindBy(predicate));
        }

        public Task<StudentTestResultDTO> GetAsync(int id)
        {
            return Task.Run(() => Get(id));
        }

        public Task<StudentTestResultDTO> AddOrUpdateAsync(StudentTestResultDTO obj)
        {
            return Task.Run(() => AddOrUpdate(obj));
        }

        public Task<StudentTestResultDTO> DeleteAsync(StudentTestResultDTO obj)
        {
            return Task.Run(() => Delete(obj));
        }
    }
}
