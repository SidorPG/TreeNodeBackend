using System.ComponentModel.DataAnnotations;

namespace Api.Models.UserJournal;

public class UserJournalGetSingleQueryStringParameters
{
    [Required]
    public int id { get; set; }

}