

using todoApiAbadnet.Models;

namespace WebApplicationAbad.Repository.RepositoryInterface
{
    public interface IUnitOfWork : System.IDisposable
    {
        IRepository<CoursesData> CoursesData { set; get; }
        IRepository<CoursesIsonline> CoursesIsonline { set; get; }
        IRepository<CoursesReserved> CoursesReserved { set; get; }
        IRepository<CoursesScheduless> CoursesScheduless { set; get; }
        IRepository<CoursesStatus> CoursesStatus { set; get; }
        IRepository<CoursesType> CoursesType { set; get; }
        IRepository<TypePaid> TypePaid { set; get; }

        IRepository<Student> Student { set; get; }
        IRepository<SessionsRecord> sessionRecords { set; get; }


        int ComitChanges();
        void Dispose();



    }
}
