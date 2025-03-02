namespace Api.Models.UserTree;

public class UserTree
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<UserTree> Children { get; set; }
}