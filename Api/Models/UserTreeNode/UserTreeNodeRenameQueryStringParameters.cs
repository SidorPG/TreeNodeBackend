using System.ComponentModel.DataAnnotations;

namespace Api.Models.UserTreeNode;

public class UserTreeNodeRenameQueryStringParameters
{
    [Required]
    public string treeName { get; set; }
    [Required]
    public Guid nodeId { get; set; }
    [Required]
    public string newNodeName { get; set; }
}