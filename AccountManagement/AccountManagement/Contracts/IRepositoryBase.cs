using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccountManagement.Contracts
{
    public interface IRepositoryBase<T> where T : class
    {
        /*
        ICollection<T> FindAll(); // may be removed
        T FindById(int id);
        bool IsValid(int id);
        
        bool Update(T entity);
        bool Delete(T entity);
        bool Save();

        
        Task Create(T entity);
        */
    }
}
