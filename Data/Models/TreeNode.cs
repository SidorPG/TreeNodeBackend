namespace Data;

public class tree_node
{
    public Guid Id { get; set; }
    public Guid? ParentNodeId { get; set; }
    public string Name { get; set; }
    public string TreeName { get; set; }
    public virtual ICollection<tree_node> Children { get; set; }
    public virtual tree_node ParentNode { get; set; }
}