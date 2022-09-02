using AccountManagement.Data.DTO;
using AccountManagement.Data;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccountManagement.Contracts
{
    public interface IRepositoryBase<T> where T : class
    {
        bool Create(T entity);
        ICollection<T> FindAll(); // may be removed
        T FindById(int id);
        bool Update(T entity);
        bool Delete(T entity);
        bool Save();
        bool IsValid(int id);
        /*


              */
    }
}
