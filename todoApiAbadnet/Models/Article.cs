using System.ComponentModel.DataAnnotations.Schema;

namespace todoApiAbadnet.Models;


public class Article
{
    public int Id { get; set; }

    public string Token { get; set; }= Guid.NewGuid().ToString();

    public string? Title { get; set; }

    public string? Contetnt { get; set; }

    public string? Author { get; set; }

    public string? Authorjob { get; set; }

    public DateTime PublishDate { get; set; } = DateTime.Now;

    public string? ReadTime { get; set; }

    public string? IamgeArticle { get; set; }

    [NotMapped]
    public IFormFile FormFiles { get; set; }


    public string? IamgeAuthor { get; set; }

    [NotMapped]
    public IFormFile FormFilesAuthor { get; set; }


    public bool IsHide { get; set; } = false;

    public bool IsDelete { get; set; } = false;


    public string? UserCreate { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;

    public string? LastUpdateUserCode { get; set; }

    public DateTime? LastUpdateDate { get; set; }

    public int CoursesTypeCode { get; set; }

    public virtual CoursesType? CoursesType { get; set; }

}
