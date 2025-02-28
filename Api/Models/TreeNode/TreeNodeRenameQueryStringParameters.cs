﻿using System.ComponentModel.DataAnnotations;

namespace Api.Models.TreeNode;

public class TreeNodeRenameQueryStringParameters
{
    [Required]
    public string treeName { get; set; }
    [Required]
    public Guid nodeId { get; set; }
    [Required]
    public string newNodeName { get; set; }
}