namespace Api.Models.UserJournal;

public static class UserJournalErrors
{
    public const string SkipOnlyPositive = "skip can't be lower then 0";
    public const string TakeOnlyPositive = "take can't be lower then 0";
    public const string MessageNotFound = "Journal Message not found";
}
