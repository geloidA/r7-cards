namespace Cardmngr.FeedbackService.Extensions;

public static class DirectoryWrapper
{
    /// <summary>
    /// Создает директорию, если ее не существует.
    /// </summary>
    /// <param name="path">Путь к создаваемой директории.</param>
    /// <returns>/null/, если директория уже существует; созданная директория, если директория не существует.</returns>
    public static DirectoryInfo? CreateIfDoesntExists(string path)
    {
        return Directory.Exists(path) ? null : Directory.CreateDirectory(path);
    }
}
