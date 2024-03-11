namespace Banking.Application.Features;

public record PagedResult<TRecords>(IEnumerable<TRecords> Records, int CurrentPage, int PageSize, int TotalRecords)
{
    public int TotalPages => (int)Math.Ceiling((double)TotalRecords / PageSize);

    public bool FirstPage => CurrentPage == 0;

    public bool LastPage => CurrentPage == TotalPages - 1;
}