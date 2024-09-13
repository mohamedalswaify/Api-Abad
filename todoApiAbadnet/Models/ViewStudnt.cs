

namespace todoApiAbadnet.Models
{
    public class ViewStudnt
    {
        public int Id { set; get; }

        public bool isSelected { set; get; } = true;

        public int StudentId { set; get; }

        public  virtual Student student { set; get; }

        public int  CoursesSchedulessId { set; get; }

        public virtual CoursesScheduless CoursesScheduless { set; get; }

        public int ViewsNowId { set; get; }

        public virtual ViewsNow ViewsNow { set; get; } 
        






    }
}
