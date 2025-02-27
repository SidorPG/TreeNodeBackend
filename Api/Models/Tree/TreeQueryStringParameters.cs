using System.ComponentModel.DataAnnotations;

namespace Api.Models.Tree;

public class TreeQueryStringParameters : IParameters
{
    [Required]
    public string treeName { get; set; }
}
