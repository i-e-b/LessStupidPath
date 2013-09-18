using System.Collections.Generic;
using System.Linq;

namespace System.IO
{
	public class FilePath : IEquatable<FilePath>
	{
		readonly List<string> _parts;
		readonly bool _rooted;
		readonly bool _empty;

		/// <summary> Create a file path from a path string </summary>
		public FilePath(string input)
		{
			_parts = new List<string>(
				input.Split(new[] { '/', '\\', ':' }, StringSplitOptions.RemoveEmptyEntries)
				);
			_rooted = IsRooted(input);
			_empty = string.IsNullOrWhiteSpace(input);
		}

		/// <summary> Append 'right' to this path, ignoring navigation semantics </summary>
		public FilePath Append(FilePath right)
		{
			return _empty ? right : new FilePath(_parts.Concat(right._parts), _rooted);
		}

		/// <summary> Append 'right' to this path, obeying standard navigation semantics </summary>
		public FilePath Navigate(FilePath navigation)
		{
			if (navigation._rooted) return navigation.Normalise();
			return new FilePath(_parts.Concat(navigation._parts), _rooted).Normalise();
		}

		/// <summary> Remove a common root from this path and return a relative path </summary>
		public FilePath Unroot(FilePath root)
		{
			for (int i = 0; i < root._parts.Count; i++)
			{
				if (_parts.Count <= i) throw new InvalidOperationException("Root supplied is longer than full path");
				if (_parts[i] != root._parts[i]) throw new InvalidOperationException("Full path is not a subpath of root");
			}
			return new FilePath(_parts.Skip(root._parts.Count), false);
		}

		/// <summary> Returns a minimal relative path from source to current path </summary>
		public FilePath RelativeTo(FilePath source)
		{
			int shorter = Math.Min(_parts.Count, source._parts.Count);
			int common;
			for (common = 0; common < shorter; common++)
				if (_parts[common] != source._parts[common]) break;

			if (common == 0)
			{
				if (_rooted) return this;
				return source.Navigate(this);
			}

			int differences = shorter - common;

			var result = new List<string>();
			for (int i = 0; i < differences; i++)
				result.Add("..");

			result.AddRange(_parts.Skip(common));

			return new FilePath(result, false);
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

		/// <summary>
		/// Returns true if the path specified was empty, false otherwise
		/// </summary>
		public bool IsEmpty()
		{
			return _empty;
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
				? RootedWindowsPath()
				: string.Join("\\", Normalise()._parts);
		}

		string RootedWindowsPath()
		{
			if (_parts.Count < 2) return "\\" + WindowsDriveSpecOrFolder();

			return string.Join("\\", WindowsDriveSpecOrFolder(), string.Join("\\", _parts.Skip(1)));
		}

		public string ToEnvironmentalPathWithoutFileName()
		{
			var path = ToEnvironmentalPath();
			return path.Substring(0, path.Count() - FileNameWithExtension().Count() - 1);
		}

		/// <summary> Returns a string representation of the path using path separators for the current execution environment </summary>
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

		public static bool IsRooted(string input)
		{
			return (input.Length >= 1
				&& (input[0] == '/' || input[0] == '\\'))

				|| (input.Length >= 2 && input[1] == ':');
		}

		protected FilePath(IEnumerable<string> orderedElements, bool rooted)
		{
			_parts = orderedElements.ToList();
			_rooted = rooted;
		}

// ReSharper disable StringLastIndexOfIsCultureSpecific.1
		public string Extension()
		{
			var fullPath = ToEnvironmentalPath();
			if (DirectorySeperatorAfterDot(fullPath))
				throw new InvalidOperationException(fullPath+" does not have an extension");
			return fullPath.Substring(fullPath.LastIndexOf(".")+1).ToLower();
		}

		static bool DirectorySeperatorAfterDot(string fullPath)
		{
			return fullPath.LastIndexOf("\\") > fullPath.LastIndexOf(".") || 
				fullPath.LastIndexOf("/") > fullPath.LastIndexOf(".");
		}
// ReSharper restore StringLastIndexOfIsCultureSpecific.1

		public string LastElement()
		{
            return _parts.LastOrDefault();
		}

		public string FileNameWithoutExtension()
		{
			var fileNameWithExtension = _parts.Last();

			var lastIndexOf = fileNameWithExtension.LastIndexOf(Extension(), StringComparison.Ordinal);

			return fileNameWithExtension.Substring(0, lastIndexOf - 1);
		}

		public string FileNameWithExtension()
		{
			return FileNameWithoutExtension() + "." + Extension();
		}

		#region Operators, equality and other such fluff
		public static explicit operator FilePath(string src)
		{
			return new FilePath(src);
		}

		public bool Equals(FilePath other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return Normalise().ToPosixPath() == other.Normalise().ToPosixPath();
		}

		public override string ToString() { return ToPosixPath(); }

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != GetType()) return false;
			return Equals((FilePath) obj);
		}
		public override int GetHashCode()
		{
			unchecked
			{
				return ((_parts != null ? _parts.GetHashCode() : 0)*397) ^ _rooted.GetHashCode();
			}
		}
		public static bool operator ==(FilePath a, FilePath b)
		{
			if (ReferenceEquals(a, b)) return true;
			if (((object)a == null) || ((object)b == null)) return false;
			return a.Normalise().ToPosixPath() == b.Normalise().ToPosixPath();
		}

		public static bool operator !=(FilePath a, FilePath b)
		{
			return !(a == b);
		}
		#endregion
	}
}
