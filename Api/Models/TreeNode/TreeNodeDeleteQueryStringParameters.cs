using System.ComponentModel.DataAnnotations;

namespace Api.Models.TreeNode;

public class TreeNodeDeleteQueryStringParameters : IParameters
{
    [Required]
    public string treeName { get; set; }
    [Required]
    public Guid nodeId { get; set; }
}