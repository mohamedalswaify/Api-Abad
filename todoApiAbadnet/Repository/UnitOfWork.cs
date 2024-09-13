

using todoApiAbadnet.Data;
using todoApiAbadnet.Models;
using WebApplicationAbad.Repository.RepositoryInterface;

namespace WebApplicationAbad.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public ApplicationDbContext Context;
        public UnitOfWork(ApplicationDbContext context)
        {
            Context = context;

            CoursesData = new Repository<CoursesData>(Context);
            CoursesIsonline = new Repository<CoursesIsonline>(Context);
            CoursesReserved = new Repository<CoursesReserved>(Context);
            CoursesScheduless = new Repository<CoursesScheduless>(Context);
            CoursesStatus = new Repository<CoursesStatus>(Context);
            CoursesType = new Repository<CoursesType>(Context);
            TypePaid = new Repository<TypePaid>(Context);
            Student = new Repository<Student>(Context);
            Room = new Repository<Room>(Context);
            sessionRecords = new Repository<SessionsRecord>(Context);


        }
        public IRepository<CoursesData> CoursesData { set; get; }
        public IRepository<CoursesIsonline> CoursesIsonline { set; get; }
        public IRepository<CoursesReserved> CoursesReserved { set; get; }
        public IRepository<CoursesScheduless> CoursesScheduless { set; get; }
        public IRepository<CoursesStatus> CoursesStatus { set; get; }
        public IRepository<CoursesType> CoursesType { set; get; }
        public IRepository<TypePaid> TypePaid { set; get; }

        public IRepository<Student> Student { set; get; }

        public IRepository<Room> Room { set; get; }

        public IRepository<SessionsRecord> sessionRecords { get; set; }

        public int ComitChanges()
        {
            return Context.SaveChanges();
        }

        public void Dispose()
        {
            Context.Dispose();
        }


    }
}
