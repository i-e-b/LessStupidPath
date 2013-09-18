using System;
using System.IO;
using NUnit.Framework;

namespace LessStupidPath.Unit.Tests
{
	[TestFixture]
	public class NavigatingTests
	{
		[Test]
		[TestCase("/home/john/", "/files", "/files")]
		[TestCase("c:\\temp\files", "c:\\files", "/c/files")]
		[TestCase("john/", "/files", "/files")]
		[TestCase("john", "/files", "/files")]
		[TestCase("john", "files", "john/files")]
		public void navigating_to_leading_slashes_reroots_the_path (string left, string right, string expected)
		{
			Assert.That(
				new FilePath(left).Navigate((FilePath)right).ToPosixPath(),
				Is.EqualTo(expected)
				);
		}

		
		[Test]
		[TestCase("/home/john/", "../files", "/home/files")]
		[TestCase("john/", "../files", "files")]
		[TestCase("/home", "../files", "/files")] // note the rooting!
		[TestCase("john/..", "../files", "../files")]
		public void navigating_obeys_all_double_dots (string left, string right, string expected)
		{
			Assert.That(
				new FilePath(left).Navigate((FilePath)right).ToPosixPath(),
				Is.EqualTo(expected)
				);
		}

		[Test]
		[TestCase("/home/john/", "./files", "/home/john/files")]
		[TestCase("john/.", "./files", "john/files")]
		[TestCase("john/.", "files", "john/files")]
		[TestCase("john/", "./files/./important/", "john/files/important")]
		public void navigating_strips_all_single_dots (string left, string right, string expected)
		{
			Assert.That(
				new FilePath(left).Navigate((FilePath)right).ToPosixPath(),
				Is.EqualTo(expected)
				);
		}

		[Test]
		public void navigating_off_the_bottom_of_a_rooted_path_throws_an_exception ()
		{
			Assert.Throws<InvalidOperationException>(() =>
				new FilePath("/home/john").Navigate((FilePath)"../../../../wrong")
				);
		}
		
		[Test]
		public void navigating_off_the_bottom_of_a_relative_path_gives_a_relative_path ()
		{
			Assert.That(
				new FilePath("john/files").Navigate((FilePath)"../../../../where").ToPosixPath(),
				Is.EqualTo("../../where")
				);
		}

		[Test]
		[TestCase("john/", "files", "john/files")]
		[TestCase("john", "files", "john/files")]
		public void navigating_treats_right_as_start_of_new_path_element (string left, string right, string expected)
		{
			Assert.That(
				new FilePath(left).Navigate((FilePath)right).ToPosixPath(),
				Is.EqualTo(expected)
				);
		}

		[Test]
		[TestCase("", "C:\\temp\\file.txt", "C:\\temp\\file.txt")]
		public void navigating_from_empty_path_to_rooted_path_gives_rooted_path(string left, string right, string expected)
		{
			Assert.That(
				new FilePath(left).Navigate((FilePath)right).ToWindowsPath(),
				Is.EqualTo(expected)
				);
		}
	}
}
