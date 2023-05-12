namespace SemesterProjekt3Api.Interfaces
{
    public interface ICRUD<T>
    {

        bool Create(T entity);
        T Get(int id);
        List<T> GetAll();
        bool Update(T entity);
        bool Delete(int id);

    }
}
