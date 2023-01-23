namespace UpdateNote.Models;

public class Note
{
    public Guid Id { get; set; }
    public string Message { get; set; } = string.Empty;
}