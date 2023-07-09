namespace Boolqa.Rapid.App.PluginCore.Infrastructures;

public class PathScanner
{
    private readonly StringComparison _pathComparison;
    private readonly char[] _pathSeparators;

    public PathScanner(StringComparison pathComparison, params char[] customPathSeparators)
    {
        var separators = new List<char>(customPathSeparators?.Length + 2 ?? 2);

        if (customPathSeparators is not null)
        {
            separators.AddRange(customPathSeparators);
        }

        separators.Add(Path.DirectorySeparatorChar);
        separators.Add(Path.AltDirectorySeparatorChar);

        _pathSeparators = separators.ToArray();
        _pathComparison = pathComparison;
    }

    public string[] GetSegments(string path, int startIndex = 0)
    {
        var targetPath = startIndex == 0 ? path : path.Substring(startIndex);
        var segments = targetPath.Split(_pathSeparators, StringSplitOptions.None);

        return segments;
    }

    public string GetFirtSegment(string path, int startIndex = 0)
    {
        var endIndex = path.Length - 1;

        for (int i = startIndex; i < path.Length - startIndex; i++)
        {
            if (CheckSeparator(path[i]))
            {
                if (i == startIndex)
                {
                    startIndex = Math.Min(++i, path.Length - 1);
                }
                else
                {
                    endIndex = i;
                    break;
                }
            }
        }

        return path.Substring(startIndex, endIndex - startIndex);
    }

    public bool StartWith(string path, string segmentPath, int startIndex, out int lastIndex)
    {
        for (int i = startIndex; i < path.Length - startIndex; i++)
        {
            if (CheckSeparator(path[i]))
            {
                continue;
            }

            var foundIndex = path.IndexOf(segmentPath, i, _pathComparison);

            if (foundIndex >= 0)
            {
                lastIndex = foundIndex + segmentPath.Length;
                return true;
            }
            else
            {
                lastIndex = -1;
                return false;
            }
        }

        lastIndex = -1;
        return false;
    }

    private bool CheckSeparator(char pathChar)
    {
        foreach (var separator in _pathSeparators)
        {
            if (separator.Equals(pathChar))
            {
                return true;
            }
        }

        return false;
    }
}
