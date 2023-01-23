using System.ComponentModel.DataAnnotations;

namespace CreateNote.Models;

public class Note
{
    public string Message { get; set; } = string.Empty;
}