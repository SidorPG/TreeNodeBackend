using System.ComponentModel.DataAnnotations;

namespace Api.Models.TreeNode;

public class TreeNodeDeleteQueryStringParameters
{
    [Required]
    public string treeName { get; set; }
    [Required]
    public Guid nodeId { get; set; }
}