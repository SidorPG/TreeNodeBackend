using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Api.Models.TreeNode;

public class TreeNodeCreateQueryStringParameters
{
    [Required]
    public string treeName { get; set; }
    [Required]
    public Guid parentNodeId { get; set; }
    [Required]
    public string nodeName { get; set; }


}