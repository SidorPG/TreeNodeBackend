namespace Data;

public class tree_node
{
    public int Id { get; set; }
    public int? ParentNodeId { get; set; }
    public string Name { get; set; }
    public string TreeName { get; set; }
    public virtual ICollection<tree_node> Children { get; set; }
    public virtual tree_node ParentNode { get; set; }
}