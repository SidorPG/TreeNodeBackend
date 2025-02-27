namespace Api.Models;

public class PagedList<T>
{
    public int Skip { get; private set; }
    public int Count { get; private set; }
    public IEnumerable<T> Items { get; private set; }
    public PagedList(List<T> items, int count, int skip)
    {
        Count = count;
        Skip = skip;
        Items = items;
    }
    public static PagedList<T> ToPagedList(IQueryable<T> source, int skip, int take)
    {
        var count = source.Count();
        var items = source.Skip(skip).Take(take).ToList();
        return new PagedList<T>(items, count, skip);
    }
}
