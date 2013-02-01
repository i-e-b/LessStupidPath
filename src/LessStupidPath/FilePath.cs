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
			_rooted = IsRooted(input);
	    }

	    static bool IsRooted(string input)
		{
			return (input.Length >= 1
				&& (input[0] == Path.DirectorySeparatorChar || input[0] == Path.AltDirectorySeparatorChar)) 
				
				|| (input.Length >= 2 && input[1] == Path.VolumeSeparatorChar);
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

		/// <summary> Append 'right' to this path, obeying standard navigation semantics </summary>
	    public FilePath Navigate(FilePath navigation)
	    {
			if (navigation._rooted) return navigation.Normalise();
			return new FilePath(_parts.Concat(navigation._parts), _rooted).Normalise();
	    }

		/// <summary> Remove single dots, remove path elements for double dots. </summary>
	    public FilePath Normalise()
	    {
			var result = new List<string>();
			uint leading = 0;

			for (int index = _parts.Count - 1; index >= 0; index--)
			{
				var part = _parts[index];
				if (part == ".") continue;
				if (part == "..")
				{
					leading++;
					continue;
				}

				if (leading > 0)
				{
					leading--;
					continue;
				}
				result.Insert(0, part);
			}

			if (_rooted && leading > 0) throw new InvalidOperationException("Tried to navigate before path root");

			for (int i = 0; i < leading; i++)
			{
				result.Insert(0, "..");
			}

			return new FilePath(result, _rooted);
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
