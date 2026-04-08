namespace Apps.Akeneo.Models.Filter;

public interface IUpdatedDateFilter : IDateFilter
{
    public DateTime? UpdatedAfter { get; set; }
    public DateTime? UpdatedBefore { get; set; }
}
