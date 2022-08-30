using System.Collections;
using System.Collections.Generic;

namespace AccountManagement.Contracts
{
    public interface IRepositoryBase<T> where T : class
    {
        ICollection<T> FindAll(); // may be removed
        T FindById(int id);
        bool IsValid(int id);
        bool Create(T entity);
        bool Update(T entity);
        bool Delete(T entity);
        bool Save();


    }
}
