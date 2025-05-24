using RestaurantApp.Areas.Admin.Models;
using RestaurantApp.Data;

namespace RestaurantApp.Areas.Admin.Repositories
{
    public interface ICategoryRepository
    {
        List<Category> GetAll();
        Category GetById(int id);
        void Add(Category category);
        void Update(Category category);
        void Delete(Category category);

    }
    public class CategoryRepository : ICategoryRepository
    {
        RestaurantDbContext context;
        public CategoryRepository(RestaurantDbContext _context)
        {
            context = _context;
        }
        public async void Add(Category category)
        {
            context.Categories.Add(category);
            context.SaveChanges();
        }

        public void Delete(Category category)
        {
            context.Categories.Remove(category);
            context.SaveChanges();
        }

        public List<Category> GetAll()
        {
            return context.Categories.ToList();
        }

        public Category GetById(int id)
        {
            return context.Categories.Find(id);
        }

        public void Update(Category category)
        {
            context.Update(category);
            context.SaveChanges();
        }
    }
}
