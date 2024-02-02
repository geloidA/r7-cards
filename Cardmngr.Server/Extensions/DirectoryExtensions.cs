namespace Cardmngr.Server.Extensions;

public static class DirectoryWrapper
{
    public static DirectoryInfo? CreateIfDoesntExists(string path)
    {
        return Directory.Exists(path) ? null : Directory.CreateDirectory(path);
    }
}
