namespace Apps.Akeneo.Models.Filter;

public interface ICreatedDateFilter : IDateFilter
{
    public DateTime? CreatedAfter { get; set; }
    public DateTime? CreatedBefore { get; set; }
}