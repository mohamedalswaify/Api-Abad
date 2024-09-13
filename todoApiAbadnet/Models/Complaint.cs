namespace todoApiAbadnet.Models;

public class Complaint
{
    public int Id { set; get; }

    public string? UserName { set; get; }

    public string? Email { set; get; }

    public string? Phone { set; get; }

    public string? TypeResqust { set; get; }

    public string? Nots { set; get; }

    public DateTime? Datecreate { set; get; } = DateTime.Now;

    public int Stutes { set; get; } = 0;
    public bool IsDelete { set; get; } = false;

    public string? UserEmail { set; get; }

    public DateTime Dateupdate { set; get; }
}
