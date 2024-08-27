using System.Collections.Generic;
using Blackbird.ConnectorService.Model.DTO.Library.Incoming;

namespace Blackbird.ConnectorService.Model.DTO.Library.Outgoing;

public record LibraryMappingTableDto
{
	public IEnumerable<LibraryColumnDto> Columns { get; set; }
	
	public IEnumerable<LibraryRowDto> Rows { get; set; }
	
	public IEnumerable<LibraryCellDto> Cells { get; set; }
}
