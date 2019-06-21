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
    public class StudentDTOService : IEntityService<StudentDTO>
    {
        IGenericRepository<Student> repository;
        readonly IMapper mapper;

        public StudentDTOService(IGenericRepository<Student> repos)
        {
            repository = repos;
            mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddExpressionMapping();
                cfg.CreateMap<Student, StudentDTO>()
                    .ForMember("SpecializationId", opt => opt.MapFrom(student => student.Group.Specialization.Id))
                    .ForMember("SpecializationName", opt => opt.MapFrom(student => student.Group.Specialization.SpecializationName))
                    .ForMember("GroupName", opt => opt.MapFrom(student => student.Group.GroupName))
                    .ForMember("EducationUnitId", opt => opt.MapFrom(student => student.Group.EducationUnit.Id))
                    .ForMember("EducationUnitName", opt => opt.MapFrom(student => student.Group.EducationUnit.EducationUnitName));
                cfg.CreateMap<StudentDTO, Student>();
            }).CreateMapper();
        }

        public StudentDTO AddOrUpdate(StudentDTO obj)
        {
            Student res = repository.AddOrUpdate(mapper.Map<Student>(obj));
            return mapper.Map<StudentDTO>(res);
        }

        public StudentDTO Delete(StudentDTO obj)
        {
            Student res = repository.Delete(mapper.Map<Student>(obj));
            return mapper.Map<StudentDTO>(res);
        }

        public IEnumerable<StudentDTO> FindBy(Expression<Func<StudentDTO, bool>> predicate)
        {
            var expr = mapper.Map<Expression<Func<StudentDTO, bool>>, Expression<Func<Student, bool>>>(predicate);
            return repository.FindBy(expr).Select(a => mapper.Map<StudentDTO>(a));
        }

        public StudentDTO Get(int id)
        {
            return mapper.Map<StudentDTO>(repository.Get(id));
        }

        public IEnumerable<StudentDTO> GetAll()
        {
            return repository.GetAll().Select(role => mapper.Map<StudentDTO>(role));
        }

        public Task<IEnumerable<StudentDTO>> GetAllAsync()
        {
            return Task.Run(() => GetAll());
        }

        public Task<IEnumerable<StudentDTO>> FindByAsync(Expression<Func<StudentDTO, bool>> predicate)
        {
            return Task.Run(() => FindBy(predicate));
        }

        public Task<StudentDTO> GetAsync(int id)
        {
            return Task.Run(() => Get(id));
        }

        public Task<StudentDTO> AddOrUpdateAsync(StudentDTO obj)
        {
            return Task.Run(() => AddOrUpdate(obj));
        }

        public Task<StudentDTO> DeleteAsync(StudentDTO obj)
        {
            return Task.Run(() => Delete(obj));
        }
    }
}
