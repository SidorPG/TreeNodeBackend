using System.ComponentModel.DataAnnotations;

namespace Api.Models.UserJournal;

public class UserJournalQueryStringParameters
{
    [Required]
    public int skip { get; set; }
    [Required]
    public int take { get; set; }
}