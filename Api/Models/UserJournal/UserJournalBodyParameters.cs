using System.ComponentModel.DataAnnotations;

namespace Api.Models.UserJournal;

public class UserJournalBodyParameters : IParameters
{
    [Required]
    public JournalBodyParameters filter { get; set; }
    public class JournalBodyParameters
    {
        public string SearchText { get; set; } = "";
        public DateTime? from { get; set; }
        public DateTime? to { get; set; }
    }
}
