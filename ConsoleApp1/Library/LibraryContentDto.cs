using Blackbird.ConnectorService.Model.DTO.Library.Outgoing;

namespace Blackbird.ConnectorService.Model.DTO.Library;

public class LibraryContentDto
{
	public string Name { get; set; }
	
	public string CollectionName { get; set; }
	
	public LibraryMappingTableDto Table { get; set; }
}