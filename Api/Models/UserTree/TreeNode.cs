namespace Api.Models.UserTree;

public class TreeNode
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public List<TreeNode> Children { get; set; }
}