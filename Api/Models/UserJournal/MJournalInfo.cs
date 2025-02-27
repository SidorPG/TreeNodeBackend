namespace Api.Models.UserJournal;

public class MJournalInfo
{
    public string Text { get; set; }
    public int Id { get; set; }
    public int EventId { get; set; }
    public DateTime CreatedAt { get; set; }
}