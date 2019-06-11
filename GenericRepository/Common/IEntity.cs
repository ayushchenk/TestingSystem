namespace GenericRepository.Common
{
    public interface IEntity<T>
    {
        T Id { set; get; }
    }
}
