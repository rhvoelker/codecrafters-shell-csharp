using System.Security.AccessControl;
using System.Security.Principal;
using Mono.Unix;

namespace CodeCrafters.Shell;

internal static class FilePermissionsHelper
{
    public static bool CanExecute(string path)
    {
        if (!OperatingSystem.IsLinux())
        {
            // Only implemented for Linux-based OSes currently.
            return false;
        }

        var permissions = new UnixFileInfo(path).FileAccessPermissions;
        return permissions.HasFlag(FileAccessPermissions.UserExecute)
            || permissions.HasFlag(FileAccessPermissions.GroupExecute)
            || permissions.HasFlag(FileAccessPermissions.OtherExecute);
    }
}