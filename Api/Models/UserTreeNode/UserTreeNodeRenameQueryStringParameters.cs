using System.ComponentModel.DataAnnotations;

namespace Api.Models.UserTreeNode;

public class UserTreeNodeRenameQueryStringParameters
{
    [Required]
    public string treeName { get; set; }
    [Required]
    public int nodeId { get; set; }
    [Required]
    public string newNodeName { get; set; }
}