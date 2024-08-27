namespace Blackbird.ConnectorService.Model.DTO.Library.Incoming;

public class LibraryCellDto
{
	public string Id { get; set; }
	
	public string RowId { get; set; }
	
	public string ColumnId { get; set; }
	
	public string? Value { get; set; }
}