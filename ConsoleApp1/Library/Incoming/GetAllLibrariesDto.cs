namespace Blackbird.ConnectorService.Model.DTO.Library.Incoming;

public class GetAllLibrariesDto
{
	public int? PageIndex { get; set; }
	
	public int? PageSize { get; set; }
	
	public string? Name { get; set; }
}