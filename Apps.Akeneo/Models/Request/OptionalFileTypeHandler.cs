using Apps.Akeneo.DataSource.Static;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Akeneo.Models.Request;
public class OptionalFileTypeHandler
{
    [Display("File format")]
    [StaticDataSource(typeof(FileTypeHandler))]
    public string? FileType { get; set; }
}
