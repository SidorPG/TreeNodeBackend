namespace Api.Models.UserTreeNode;
/// <summary>
/// Represents journal API
/// </summary>

public class UserTreeNode
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public List<UserTreeNode> Children { get; set; }
}