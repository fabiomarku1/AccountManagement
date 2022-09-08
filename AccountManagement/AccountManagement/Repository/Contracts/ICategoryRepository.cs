using AccountManagement.Data;
using AccountManagement.Data.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccountManagement.Contracts
{
    public interface ICategoryRepository: IRepositoryBase<Category>
    {
        public Task<IEnumerable<CategoryViewModel>> GetCategories();
        public int GetCategoryId(CategoryViewModel request);

        public Category GetCategory(int id);
    }
}
