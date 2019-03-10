using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using GenericRepository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using TestingSystem.BOL.Model;
using TestingSystem.DAL.DbModel;

namespace TestingSystem.BOL.Service
{
    public class SpecializationDTOService : IEntityService<SpecializationDTO>
    {
        IGenericRepository<Specialization> repository;
        readonly IMapper mapper;

        public SpecializationDTOService(IGenericRepository<Specialization> repos)
        {
            repository = repos;
            mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddExpressionMapping();
                cfg.CreateMap<Specialization, SpecializationDTO>();
                cfg.CreateMap<SpecializationDTO, Specialization>();
            }).CreateMapper();
        }

        public SpecializationDTO AddOrUpdate(SpecializationDTO obj)
        {
            Specialization res = repository.AddOrUpdate(mapper.Map<Specialization>(obj));
            return mapper.Map<SpecializationDTO>(res);
        }

        public SpecializationDTO Delete(SpecializationDTO obj)
        {
            Specialization res = repository.Delete(mapper.Map<Specialization>(obj));
            return mapper.Map<SpecializationDTO>(res);
        }

        public IEnumerable<SpecializationDTO> FindBy(Expression<Func<SpecializationDTO, bool>> predicate)
        {
            var expr = mapper.Map<Expression<Func<SpecializationDTO, bool>>, Expression<Func<Specialization, bool>>>(predicate);
            return repository.FindBy(expr).Select(a => mapper.Map<SpecializationDTO>(a));
        }

        public SpecializationDTO Get(int id)
        {
            return mapper.Map<SpecializationDTO>(repository.Get(id));
        }

        public IEnumerable<SpecializationDTO> GetAll()
        {
            return repository.GetAll().Select(role => mapper.Map<SpecializationDTO>(role));
        }
    }
}
