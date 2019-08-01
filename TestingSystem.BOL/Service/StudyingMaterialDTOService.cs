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
    public class StudyingMaterialDTOService : IEntityService<StudyingMaterialDTO>
    {
        IGenericRepository<StudyingMaterial> repository;
        readonly IMapper mapper;

        public StudyingMaterialDTOService(IGenericRepository<StudyingMaterial> repos)
        {
            repository = repos;
            mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddExpressionMapping();
                cfg.CreateMap<StudyingMaterial, StudyingMaterialDTO>()
                    .ForMember("SpecializationId", opt => opt.MapFrom(group => group.Teacher.Subject.SpecializationId))
                    .ForMember("SubjectName", opt => opt.MapFrom(group => group.Teacher.Subject.SubjectName))
                    .ForMember("SubjectId", opt => opt.MapFrom(group => group.Teacher.SubjectId));
                cfg.CreateMap<StudyingMaterialDTO, StudyingMaterial>();
            }).CreateMapper();
        }

        public StudyingMaterialDTO AddOrUpdate(StudyingMaterialDTO obj)
        {
            StudyingMaterial res = repository.AddOrUpdate(mapper.Map<StudyingMaterial>(obj));
            return mapper.Map<StudyingMaterialDTO>(res);
        }

        public StudyingMaterialDTO Delete(StudyingMaterialDTO obj)
        {
            StudyingMaterial res = repository.Delete(mapper.Map<StudyingMaterial>(obj));
            return mapper.Map<StudyingMaterialDTO>(res);
        }

        public IEnumerable<StudyingMaterialDTO> FindBy(Expression<Func<StudyingMaterialDTO, bool>> predicate)
        {
            var expr = mapper.Map<Expression<Func<StudyingMaterialDTO, bool>>, Expression<Func<StudyingMaterial, bool>>>(predicate);
            return repository.FindBy(expr).Select(a => mapper.Map<StudyingMaterialDTO>(a));
        }

        public StudyingMaterialDTO Get(int id)
        {
            return mapper.Map<StudyingMaterialDTO>(repository.Get(id));
        }

        public IEnumerable<StudyingMaterialDTO> GetAll()
        {
            return repository.GetAll().Select(group => mapper.Map<StudyingMaterialDTO>(group));
        }

        public Task<IEnumerable<StudyingMaterialDTO>> GetAllAsync()
        {
            return Task.Run(() => GetAll());
        }

        public Task<IEnumerable<StudyingMaterialDTO>> FindByAsync(Expression<Func<StudyingMaterialDTO, bool>> predicate)
        {
            return Task.Run(() => FindBy(predicate));
        }

        public Task<StudyingMaterialDTO> GetAsync(int id)
        {
            return Task.Run(() => Get(id));
        }

        public Task<StudyingMaterialDTO> AddOrUpdateAsync(StudyingMaterialDTO obj)
        {
            return Task.Run(() => AddOrUpdate(obj));
        }

        public Task<StudyingMaterialDTO> DeleteAsync(StudyingMaterialDTO obj)
        {
            return Task.Run(() => Delete(obj));
        }
    }
}
