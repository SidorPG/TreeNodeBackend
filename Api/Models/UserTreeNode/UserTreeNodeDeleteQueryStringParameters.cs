using System.ComponentModel.DataAnnotations;

namespace Api.Models.UserTreeNode;

public class UserTreeNodeDeleteQueryStringParameters
{
    [Required]
    public string treeName { get; set; }
    [Required]
    public Guid nodeId { get; set; }
}