using System.Collections.Generic;

namespace Blackbird.ConnectorService.Model.DTO.Library.Incoming
{
	public class MappingMultipleValuesFilterDto
	{
		public string SourceMappingName { get; init; }

		public List<string> SourceValues { get; init; }

		public string DestinationMappingName { get; init; }
	}
}
