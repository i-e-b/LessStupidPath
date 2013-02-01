using System.IO;
using NUnit.Framework;

namespace LessStupidPath.Unit.Tests
{
	[TestFixture]
	public class AppendingPathsTests
	{
		[Test]
		[TestCase("/home/john/", "/files", "/home/john/files")]
		[TestCase("john/", "/files", "john/files")]
		[TestCase("john", "/files", "john/files")]
		[TestCase("john", "files", "john/files")]
		public void appending_paths_ignores_leading_slashes (string left, string right, string expected)
		{
			Assert.That(
				new FilePath(left).Append((FilePath)right).ToPosixPath(),
				Is.EqualTo(expected)
				);
		}

		[Test]
		[TestCase("/home/john/", "./files", "/home/john/./files")]
		[TestCase("john/", "../files", "john/../files")]
		[TestCase("john", "../files", "john/../files")]
		[TestCase("john/..", "../files", "john/../../files")]
		public void appending_paths_ignores_leading_dots (string left, string right, string expected)
		{
			Assert.That(
				new FilePath(left).Append((FilePath)right).ToPosixPath(),
				Is.EqualTo(expected)
				);
		}

		[Test]
		[TestCase("john/", "files", "john/files")]
		[TestCase("john", "files", "john/files")]
		public void appending_paths_treats_right_as_start_of_new_path_element (string left, string right, string expected)
		{
			Assert.That(
				new FilePath(left).Append((FilePath)right).ToPosixPath(),
				Is.EqualTo(expected)
				);
		}
	}
}
