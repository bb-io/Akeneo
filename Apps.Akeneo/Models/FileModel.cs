using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.Akeneo.Models;

public class FileModel(FileReference file)
{
    public FileReference File { get; set; } = file;
}