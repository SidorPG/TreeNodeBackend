namespace Api.Models.UserTreeNode;

public static class UserTreeNodeErrors
{
    internal static string ParentNotFound = "Parent node not found";
    internal static string TakeOnlyPositive = "take can't be lower then 0";
    internal static string MessageNotFound = "Journal Message not found";
    internal static string NodeAlredyExist = "Node alredy exist";
    internal static string ChildNodesShouldBeDeletedFirst = "You have to delete all children nodes first";
    internal static string TreeNodeNotFound = "Tree node not found";
    internal static string TreeNotFound = "Tree not found";
}
