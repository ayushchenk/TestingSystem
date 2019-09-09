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
    public class TeacherDTOService : IEntityService<TeacherDTO>
    {
        IGenericRepository<Teacher> repository;
        readonly IMapper mapper;

        public TeacherDTOService(IGenericRepository<Teacher> repos)
        {
            repository = repos;
            mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddExpressionMapping();
                cfg.CreateMap<Teacher, TeacherDTO>()
                    .ForMember("SpecializationId", opt => opt.MapFrom(teacher => teacher.Specialization.Id))
                    .ForMember("SpecializationName", opt => opt.MapFrom(teacher =>  teacher.Specialization.SpecializationName ))
                    .ForMember("EducationUnitId", opt => opt.MapFrom(teacher => teacher.EducationUnit.Id))
                    .ForMember("EducationUnitName", opt => opt.MapFrom(teacher => teacher.EducationUnit.EducationUnitName));
                cfg.CreateMap<TeacherDTO, Teacher>();
            }).CreateMapper();
        }

        public TeacherDTO AddOrUpdate(TeacherDTO obj)
        {
            Teacher res = repository.AddOrUpdate(mapper.Map<Teacher>(obj));
            return mapper.Map<TeacherDTO>(res);
        }

        public TeacherDTO Delete(TeacherDTO obj)
        {
            Teacher res = repository.Delete(mapper.Map<Teacher>(obj));
            return mapper.Map<TeacherDTO>(res);
        }

        public IEnumerable<TeacherDTO> FindBy(Expression<Func<TeacherDTO, bool>> predicate)
        {
            var expr = mapper.Map<Expression<Func<TeacherDTO, bool>>, Expression<Func<Teacher, bool>>>(predicate);
            return repository.FindBy(expr).Select(a => mapper.Map<TeacherDTO>(a));
        }

        public TeacherDTO Get(int id)
        {
            return mapper.Map<TeacherDTO>(repository.Get(id));
        }

        public IEnumerable<TeacherDTO> GetAll()
        {
            return repository.GetAll().Select(role => mapper.Map<TeacherDTO>(role));
        }

        public Task<IEnumerable<TeacherDTO>> GetAllAsync()
        {
            return Task.Run(() => GetAll());
        }

        public Task<IEnumerable<TeacherDTO>> FindByAsync(Expression<Func<TeacherDTO, bool>> predicate)
        {
            return Task.Run(() => FindBy(predicate));
        }

        public Task<TeacherDTO> GetAsync(int id)
        {
            return Task.Run(() => Get(id));
        }

        public Task<TeacherDTO> AddOrUpdateAsync(TeacherDTO obj)
        {
            return Task.Run(() => AddOrUpdate(obj));
        }

        public Task<TeacherDTO> DeleteAsync(TeacherDTO obj)
        {
            return Task.Run(() => Delete(obj));
        }
    }
}
