namespace GenericRepository.Common
{
    public interface Entity<T>
    {
        T Id { set; get; }
    }
}
