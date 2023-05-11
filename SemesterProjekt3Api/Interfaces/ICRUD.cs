namespace SemesterProjekt3Api.Interfaces
{
    public interface ICRUD<T>
    {

        bool Create(T entity);
        T Get(int id);
        IEnumerable<T> GetAll();
        bool Update(T entity);
        bool Delete(int id);

    }
}
