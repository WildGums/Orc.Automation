namespace Orc.Automation.Tests;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Catel.Logging;

public sealed class TemporaryFileSystemContext : IDisposable
{
    private static readonly ILog Log = LogManager.GetCurrentClassLogger();

    private readonly string _moveDirectory;
    private readonly List<BackupEntry> _registeredEntries = [];

    public TemporaryFileSystemContext(string moveDirectory = null)
    {
        _moveDirectory = moveDirectory;

        if (_moveDirectory is not null && !Directory.Exists(moveDirectory))
        {
            Directory.CreateDirectory(moveDirectory);
        }
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        try
        {
            foreach (var registeredEntry in _registeredEntries.AsEnumerable().Reverse())
            {
                var oldPath = registeredEntry.OldPath;
                if (File.Exists(oldPath))
                {
                    File.Delete(oldPath);
                }

                if (Directory.Exists(oldPath))
                {
                    Directory.Delete(oldPath, true);
                }

                var newPath = registeredEntry.NewPath;
                if (File.Exists(newPath))
                {
                    File.Move(newPath, oldPath);
                }
                else if (Directory.Exists(newPath))
                {
                    CopyFilesRecursively(newPath, oldPath);
                }
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to delete temporary files");
        }
    }

    public string CreateFile(string filePath, string contents,
        FileSystemContextEntryAction action = FileSystemContextEntryAction.None)
    {
        var entry = new BackupEntry
        {
            OldPath = filePath
        };

        BackupFile(filePath, action);

        File.WriteAllText(filePath, contents);

        _registeredEntries.Add(entry);

        return filePath;
    }

    public string CreateDirectory(string directory,
        FileSystemContextEntryAction action = FileSystemContextEntryAction.None)
    {
        var entry = new BackupEntry
        {
            OldPath = directory
        };

        BackupDirectory(directory, action);

        Directory.CreateDirectory(directory);

        _registeredEntries.Add(entry);

        return directory;
    }

    public string Copy(string source, string destination,
        FileSystemContextEntryAction action = FileSystemContextEntryAction.None)
    {
        if (File.Exists(source))
        {
            BackupFile(destination, action);

            File.Copy(source, destination);

            return destination;
        }

        if (Directory.Exists(source))
        {
            BackupDirectory(destination, action);

            CopyFilesRecursively(source, destination);

            return destination;
        }

        throw Log.ErrorAndCreateException<Exception>(
            $"Can't copy because path: '{source}' doesn't exist in file system");
    }

    private string GenerateUniqueMovePath()
    {
        if (_moveDirectory is null)
        {
            throw Log.ErrorAndCreateException<Exception>("Move directory wasn't specified");
        }

        return Path.Combine(_moveDirectory, Guid.NewGuid().ToString());
    }

    private void BackupFile(string filePath, FileSystemContextEntryAction action)
    {
        if (!File.Exists(filePath))
        {
            _registeredEntries.Add(new()
            {
                OldPath = filePath
            });

            return;
        }

        switch (action)
        {
            case FileSystemContextEntryAction.None:
                throw Log.ErrorAndCreateException<Exception>($"File: '{filePath}' already exists");

            case FileSystemContextEntryAction.Delete:
                File.Delete(filePath);
                _registeredEntries.Add(new()
                {
                    OldPath = filePath
                });
                break;

            case FileSystemContextEntryAction.Move when _moveDirectory is null:
                throw Log.ErrorAndCreateException<Exception>("Move directory wasn't specified");

            case FileSystemContextEntryAction.Move:
                var newPath = GenerateUniqueMovePath();
                File.Move(filePath, newPath);

                _registeredEntries.Add(new()
                {
                    OldPath = filePath,
                    NewPath = newPath
                });

                break;
        }
    }


    public void BackupDirectory(string directory, FileSystemContextEntryAction action)
    {
        if (!Directory.Exists(directory))
        {
            _registeredEntries.Add(new()
            {
                OldPath = directory
            });

            return;
        }

        switch (action)
        {
            case FileSystemContextEntryAction.None:
                throw Log.ErrorAndCreateException<Exception>($"Directory: '{directory}' already exists");

            case FileSystemContextEntryAction.Delete:
                Directory.Delete(directory, true);
                break;

            case FileSystemContextEntryAction.Move when _moveDirectory is null:
                throw Log.ErrorAndCreateException<Exception>("Move directory wasn't specified");

            case FileSystemContextEntryAction.Move:
                var newPath = GenerateUniqueMovePath();

                Directory.Move(directory, newPath);

                _registeredEntries.Add(new()
                {
                    OldPath = directory,
                    NewPath = newPath
                });

                break;
        }
    }

    private static void CopyFilesRecursively(string sourcePath, string targetPath)
    {
        foreach (var dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
        {
            Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));
        }

        //Copy all the files & Replaces any files with the same name
        foreach (var newPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
        {
            File.Copy(newPath, newPath.Replace(sourcePath, targetPath), true);
        }
    }

    private class BackupEntry
    {
        public string OldPath { get; set; }
        public string NewPath { get; set; }
    }
}