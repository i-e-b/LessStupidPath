using System.IO;
using NUnit.Framework;

namespace LessStupidPath.Unit.Tests
{
	[TestFixture]
	public class RelativeToTests
	{
		[Test]
        [TestCase("/home/john/files/moreFiles/testFiles", "/home/marge/files", "../../../../marge/files")]
		[TestCase("/home/john/files", "/home/marge/files", "../../marge/files")]
		[TestCase("/srv/widget/files", "/srv/widget/files/myfile.txt", "myfile.txt")]
		[TestCase("/srv/widget/files", "/srv/widget/files/logs/today.txt", "logs/today.txt")]
		[TestCase("/home/john/files", "marge/files", "/home/john/files/marge/files")] // nothing in common, destination is relative
		[TestCase("home/john/files", "marge/files", "home/john/files/marge/files")] // nothing in common, destination is relative
		[TestCase("/home/john/files", "/marge/files", "/marge/files")] // nothing in common, destination is a root
		[TestCase("home/john/files", "/marge/files", "/marge/files")] // nothing in common, destination is a root
		public void relative_to_tries_to_return_a_minimal_relative_path_between_two_paths (string source, string destination, string expected)
		{
			Assert.That(
				new FilePath(destination).RelativeTo((FilePath)source).ToPosixPath(),
				Is.EqualTo(expected)
				);
		}
	}
}
