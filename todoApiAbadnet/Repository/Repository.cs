using System.Collections.ObjectModel;
using todoApiAbadnet.Data;
using WebApplicationAbad.Repository.RepositoryInterface;

namespace todoApiAbadnet.Models
{
    public class Repository<Entity> : IRepository<Entity> where Entity : class
    {
        public ApplicationDbContext AppDbContext;

        public Repository(ApplicationDbContext applicationDbContext)
        {
            AppDbContext = applicationDbContext;
        }


        public void AddNewRow(Entity entity)
        {
            AppDbContext.Set<Entity>().Add(entity);
            AppDbContext.SaveChanges();
        }

        public void AddRange(Collection<Entity> items)
        {
            AppDbContext.Set<Entity>().AddRange(items); 
            AppDbContext.SaveChanges();
        }

        public void DeleteRange(Collection<Entity> items)
        {
            AppDbContext.Set<Entity>().RemoveRange(items);
            AppDbContext.SaveChanges();
        }

        public void DeleteRow(Entity entity)
        {
           AppDbContext.Set<Entity>().Remove(entity);
            AppDbContext.SaveChanges();
        }

        public Entity GetByID(int id)
        {
           return AppDbContext.Set<Entity>().Find(id);
        }


        public List<Entity> GetEntity()
        {
           return AppDbContext.Set<Entity>().ToList();

        }

        public void UpdateRange(Collection<Entity> items)
        {
            AppDbContext.Set<Entity>().UpdateRange(items);
            AppDbContext.SaveChanges();
        }

        public void UpdateRow(Entity entity)
        {
             AppDbContext.Set<Entity>().Update(entity);
            AppDbContext.SaveChanges();
        }
    }
}
