using System.ComponentModel.DataAnnotations;

namespace Api.Models.UserJournal;

public class UserJournalGetSingleQueryStringParameters : IParameters
{
    [Required]
    public int id { get; set; }

}