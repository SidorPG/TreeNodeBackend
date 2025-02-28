using System.ComponentModel.DataAnnotations;

namespace Api.Models.UserTree;

public class TreeQueryStringParameters
{
    [Required]
    public string treeName { get; set; }
}
