using System.ComponentModel.DataAnnotations;

namespace Api.Models.Tree;

public class TreeQueryStringParameters
{
    [Required]
    public string treeName { get; set; }
}
