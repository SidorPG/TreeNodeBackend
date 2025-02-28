using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models;

[Table("journal_events")]
public class journal_event
{
    public int Id { get; set; }
    public DateTime Created { get; set; }
    public virtual journal_message Message { get; set; }
}