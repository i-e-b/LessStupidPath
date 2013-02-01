using System.Collections.Generic;
using System.Linq;

namespace System.IO
{
    public class FilePath
    {
	    readonly List<string> _parts;
	    readonly bool _rooted;

	    /// <summary> Create a file path from a path string </summary>
	    public FilePath(string input)
	    {
			_parts = new List<string>(
				input.Split(new []{Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar, Path.PathSeparator, Path.VolumeSeparatorChar}, StringSplitOptions.RemoveEmptyEntries)
				);
			_rooted = Path.IsPathRooted(input);
	    }

	    protected FilePath(IEnumerable<string> orderedElements, bool rooted)
	    {
			_parts = orderedElements.ToList();
		    _rooted = rooted;
	    }

		/// <summary> Append 'right' to this path, ignoring navigation semantics </summary>
	    public FilePath Append(FilePath right)
	    {
		    return new FilePath(_parts.Concat(right._parts), _rooted);
	    }

		/// <summary> Returns a string representation of the path using Posix path separators </summary>
	    public string ToPosixPath()
	    {
		    return (_rooted) 
				? "/" + string.Join("/", _parts)
				: string.Join("/", _parts);
	    }

		/// <summary> Returns a string representation of the path using Windows path separators </summary>
	    public string ToWindowsPath()
	    {
		    return (_rooted) 
				? string.Join("\\", WindowsDriveSpecOrFolder(), string.Join("\\", _parts.Skip(1)))
				: string.Join("\\", _parts);
	    }

		public string ToEnvironmentalPath()
		{
			return PosixOS() ? ToPosixPath() : ToWindowsPath();
		}
		static bool PosixOS()
		{
			var p = (int)Environment.OSVersion.Platform;
			return (p == 4) || (p == 6) || (p == 128);
		}

		string WindowsDriveSpecOrFolder()
	    {
			if (_parts.Count < 1) return "";

			if (_parts[0].Length == 1) return _parts[0] + ":";
		    return "\\" + _parts.First();
	    }

		public static explicit operator FilePath(string src)
		{
			return new FilePath(src);
		}
    }
}
