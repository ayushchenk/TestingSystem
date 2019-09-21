using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using GenericRepository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TestingSystem.BOL.Model;
using TestingSystem.DAL.DbModel;

namespace TestingSystem.BOL.Service
{
    public class TeachersInGroupsDTOService:IEntityService<TeachersInGroupDTO>
    {
        IGenericRepository<TeachersInGroup> repository;
        readonly IMapper mapper;

        public TeachersInGroupsDTOService(IGenericRepository<TeachersInGroup> repos)
        {
            repository = repos;
            mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddExpressionMapping();
                cfg.CreateMap<TeachersInGroup, TeachersInGroupDTO>()
                    .ForMember("GroupName", opt => opt.MapFrom(tig => tig.Group.GroupName))
                    .ForMember("SubjectName", opt => opt.MapFrom(tig => tig.TeachersInSubject.Subject.SubjectName))
                    .ForMember("SubjectId", opt => opt.MapFrom(tig => tig.TeachersInSubject.Subject.Id))
                    .ForMember("FirstName", opt => opt.MapFrom(tig => tig.TeachersInSubject.Teacher.FirstName))
                    .ForMember("Email", opt => opt.MapFrom(tig => tig.TeachersInSubject.Teacher.Email))
                    .ForMember("LastName", opt => opt.MapFrom(tig => tig.TeachersInSubject.Teacher.LastName))
                    .ForMember("TeacherId", opt => opt.MapFrom(tig => tig.TeachersInSubject.Teacher.Id))
                    .ForMember("SpecializationName", opt => opt.MapFrom(tig => tig.Group.Specialization.SpecializationName))
                    .ForMember("EducationUnitName", opt => opt.MapFrom(tig => tig.Group.EducationUnit.EducationUnitName))
                    .ForMember("SpecializationId", opt => opt.MapFrom(tig => tig.Group.Specialization.Id))
                    .ForMember("TeacherSpecialization", opt => opt.MapFrom(tig => tig.TeachersInSubject.Teacher.Specialization.Id))
                    .ForMember("EducationUnitId", opt => opt.MapFrom(tig => tig.Group.EducationUnit.Id));
                cfg.CreateMap<TeachersInGroupDTO, TeachersInGroup>();
            }).CreateMapper();
        }

        public void Save()
        {
            repository.Save();
        }

        public TeachersInGroupDTO AddOrUpdate(TeachersInGroupDTO obj)
        {
            TeachersInGroup res = repository.AddOrUpdate(mapper.Map<TeachersInGroup>(obj));
            return mapper.Map<TeachersInGroupDTO>(res);
        }

        public TeachersInGroupDTO Delete(TeachersInGroupDTO obj)
        {
            TeachersInGroup res = repository.Delete(mapper.Map<TeachersInGroup>(obj));
            return mapper.Map<TeachersInGroupDTO>(res);
        }

        public IEnumerable<TeachersInGroupDTO> FindBy(Expression<Func<TeachersInGroupDTO, bool>> predicate)
        {
            var expr = mapper.Map<Expression<Func<TeachersInGroupDTO, bool>>, Expression<Func<TeachersInGroup, bool>>>(predicate);
            return repository.FindBy(expr).Select(a => mapper.Map<TeachersInGroupDTO>(a));
        }

        public TeachersInGroupDTO Get(int id)
        {
            return mapper.Map<TeachersInGroupDTO>(repository.Get(id));
        }

        public IEnumerable<TeachersInGroupDTO> GetAll()
        {
            return repository.GetAll().Select(tig => mapper.Map<TeachersInGroupDTO>(tig));
        }

        public void DeleteRange(IEnumerable<TeachersInGroupDTO> items)
        {
            repository.DeleteRange(items.Select(item => mapper.Map<TeachersInGroup>(item)));
        }

        public Task DeleteRangeAsync(IEnumerable<TeachersInGroupDTO> items)
        {
            return Task.Run(() => DeleteRange(items));
        }

        public Task<IEnumerable<TeachersInGroupDTO>> GetAllAsync()
        {
            return Task.Run(() => GetAll());
        }

        public Task<IEnumerable<TeachersInGroupDTO>> FindByAsync(Expression<Func<TeachersInGroupDTO, bool>> predicate)
        {
            return Task.Run(() => FindBy(predicate));
        }

        public Task<TeachersInGroupDTO> GetAsync(int id)
        {
            return Task.Run(() => Get(id));
        }

        public Task<TeachersInGroupDTO> AddOrUpdateAsync(TeachersInGroupDTO obj)
        {
            return Task.Run(() => AddOrUpdate(obj));
        }

        public Task<TeachersInGroupDTO> DeleteAsync(TeachersInGroupDTO obj)
        {
            return Task.Run(() => Delete(obj));
        }
    }
}
