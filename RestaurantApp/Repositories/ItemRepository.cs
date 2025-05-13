using RestaurantApp.Data;
using RestaurantApp.Models;

namespace RestaurantApp.Repositories
{
    public interface IItemRepository
    {
        List<Item> GetItems();
        void Add(Item item);
        void Update(Item item);
        void Delete(Item item);
        Item getById(int id);
    }

    public class ItemRepository : IItemRepository
    {
        RestaurantDbContext context;
       public ItemRepository(RestaurantDbContext _context)
        {
            this.context = _context;
        }
        public void Add(Item item)
        {
            context.Items.Add(item);
            context.SaveChanges();
        }

        public void Delete(Item item)
        {
           context.Remove(item);
            context.SaveChanges();
        }

        public Item getById(int id)
        {
            return context.Items.Find(id);
        }

        public List<Item> GetItems()
        {
            return context.Items.ToList();
        }

        public void Update(Item item)
        {
           context.Update(item);
            context.SaveChanges();
        }
    }
}
