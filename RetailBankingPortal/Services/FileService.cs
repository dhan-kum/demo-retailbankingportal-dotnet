using System.IO.Compression;

namespace RetailBankingPortal.Services;

public class FileService
{
    public void AddFilesToZip(string path, string entryName, ZipArchive archive)
    {
        if (Directory.Exists(path))
        {
            var dirName = entryName.EndsWith("/") ? entryName : entryName + "/";
            archive.CreateEntry(dirName);

            foreach (var child in Directory.GetFileSystemEntries(path))
            {
                var childName = Path.GetFileName(child);
                AddFilesToZip(child, dirName + childName, archive);
            }
        }
        else if (File.Exists(path))
        {
            archive.CreateEntryFromFile(path, entryName);
        }
    }
}
