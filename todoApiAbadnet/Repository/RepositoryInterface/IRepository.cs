using System.Collections.ObjectModel;

namespace WebApplicationAbad.Repository.RepositoryInterface
{
    public interface IRepository<Entity> where Entity : class 
    {
        List<Entity> GetEntity();

        Entity GetByID(int id);


        void AddNewRow(Entity entity);

        void UpdateRow(Entity entity);

        void DeleteRow(Entity entity);

        void AddRange(Collection<Entity> items);

        void UpdateRange(Collection<Entity> items);

        void DeleteRange(Collection<Entity> items);




    }
}
