using System.ComponentModel.DataAnnotations;

namespace Api.Models.UserTree;

public class UserTreeQueryStringParameters
{
    [Required]
    public string treeName { get; set; }
}
