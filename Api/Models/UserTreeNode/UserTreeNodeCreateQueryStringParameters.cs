using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Api.Models.UserTreeNode;

public class UserTreeNodeCreateQueryStringParameters
{
    [Required]
    public string treeName { get; set; }
    [Required]
    public Guid parentNodeId { get; set; }
    [Required]
    public string nodeName { get; set; }


}