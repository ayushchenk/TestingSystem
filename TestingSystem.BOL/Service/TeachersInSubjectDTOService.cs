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
    public class TeachersInSubjectDTOService:IEntityService<TeachersInSubjectDTO>
    {
        IGenericRepository<TeachersInSubject> repository;
        readonly IMapper mapper;

        public TeachersInSubjectDTOService(IGenericRepository<TeachersInSubject> repos)
        {
            repository = repos;
            mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddExpressionMapping();
                cfg.CreateMap<TeachersInSubject, TeachersInSubjectDTO>()
                    .ForMember("Email", opt => opt.MapFrom(tig => tig.Teacher.Email))
                    .ForMember("IsDeleted", opt => opt.MapFrom(tig => tig.Teacher.IsDeleted))
                    .ForMember("SpecializationId", opt => opt.MapFrom(tig => tig.Teacher.SpecializationId))
                    .ForMember("LastName", opt => opt.MapFrom(tig => tig.Teacher.FirstName))
                    .ForMember("FirstName", opt => opt.MapFrom(tig => tig.Teacher.LastName))
                    .ForMember("SubjectName", opt => opt.MapFrom(tig => tig.Subject.SubjectName))
                    .ForMember("SubjectId", opt => opt.MapFrom(tig => tig.Subject.Id));
                cfg.CreateMap<TeachersInSubjectDTO, TeachersInSubject>();
            }).CreateMapper();
        }

        public TeachersInSubjectDTO AddOrUpdate(TeachersInSubjectDTO obj)
        {
            TeachersInSubject res = repository.AddOrUpdate(mapper.Map<TeachersInSubject>(obj));
            return mapper.Map<TeachersInSubjectDTO>(res);
        }

        public TeachersInSubjectDTO Delete(TeachersInSubjectDTO obj)
        {
            TeachersInSubject res = repository.Delete(mapper.Map<TeachersInSubject>(obj));
            return mapper.Map<TeachersInSubjectDTO>(res);
        }

        public IEnumerable<TeachersInSubjectDTO> FindBy(Expression<Func<TeachersInSubjectDTO, bool>> predicate)
        {
            var expr = mapper.Map<Expression<Func<TeachersInSubjectDTO, bool>>, Expression<Func<TeachersInSubject, bool>>>(predicate);
            return repository.FindBy(expr).Select(a => mapper.Map<TeachersInSubjectDTO>(a));
        }

        public TeachersInSubjectDTO Get(int id)
        {
            return mapper.Map<TeachersInSubjectDTO>(repository.Get(id));
        }

        public IEnumerable<TeachersInSubjectDTO> GetAll()
        {
            return repository.GetAll().Select(tig => mapper.Map<TeachersInSubjectDTO>(tig));
        }

        public Task<IEnumerable<TeachersInSubjectDTO>> GetAllAsync()
        {
            return Task.Run(() => GetAll());
        }

        public Task<IEnumerable<TeachersInSubjectDTO>> FindByAsync(Expression<Func<TeachersInSubjectDTO, bool>> predicate)
        {
            return Task.Run(() => FindBy(predicate));
        }

        public Task<TeachersInSubjectDTO> GetAsync(int id)
        {
            return Task.Run(() => Get(id));
        }

        public Task<TeachersInSubjectDTO> AddOrUpdateAsync(TeachersInSubjectDTO obj)
        {
            return Task.Run(() => AddOrUpdate(obj));
        }

        public Task<TeachersInSubjectDTO> DeleteAsync(TeachersInSubjectDTO obj)
        {
            return Task.Run(() => Delete(obj));
        }
    }
}
