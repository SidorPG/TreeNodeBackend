using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models;

[Table("journal_events")]
public class journal_event
{
    public int Id { get; set; }
    public DateTime Created { get; set; }
    public string Path { get; set; }
    public string? RequestQuery { get; set; }
    public string? RequestBody { get; set; }
    public string? Exception { get; set; }
    public string? ExceptionMessage { get; set; }
    public string? ExceptionStackTrace { get; set; }

    public virtual journal_message Message { get; set; }
}