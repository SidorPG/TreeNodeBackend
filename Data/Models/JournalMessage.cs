namespace Data.Models;

public class journal_message
{
    public string Type { get; set; }
    public int Id { get; set; }
    public int EventId { get; set; }
    public string Data { get; set; }
    public virtual journal_event JournalEvent { get; set; }
}