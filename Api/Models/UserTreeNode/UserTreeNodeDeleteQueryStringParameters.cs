using System.ComponentModel.DataAnnotations;

namespace Api.Models.UserTreeNode;

public class UserTreeNodeDeleteQueryStringParameters
{
    [Required]
    public string treeName { get; set; }
    [Required]
    public int nodeId { get; set; }
}