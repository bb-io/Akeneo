using System;

namespace Blackbird.ConnectorService.Model.DTO.Library.Outgoing
{
	public class LibraryDto
	{
		public long Id { get; set; }
		
		public string Name { get; set; }

		public string Description { get; set; }
		
		public bool IsDefault { get; set; }

		public DateTime CreatedOn { get; set; }

		public DateTime UpdatedOn { get; set; }
	}
}
