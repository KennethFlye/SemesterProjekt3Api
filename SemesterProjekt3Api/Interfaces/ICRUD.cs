namespace SemesterProjekt3Api.Interfaces
{
    public interface ICRUD<T>
    {

        void Create(T entity);
        T Get(int id);
        IEnumerable<T> GetAll();
        void Update(T entity);
        void Delete(int id);

    }
}
