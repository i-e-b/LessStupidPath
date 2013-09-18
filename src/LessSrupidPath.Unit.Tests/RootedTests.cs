using System.IO;
using NUnit.Framework;

namespace LessStupidPath.Unit.Tests
{
	[TestFixture]
	public class RootedTests
	{
		[Test]
		[TestCase(@"C:\a\folder")]
		[TestCase(@"/a/folder")]
		public void Rooted_file_paths_should_return_true(string input)
		{
			Assert.That(FilePath.IsRooted(input), Is.True);
		}

		[Test]
		[TestCase(@"a\folder")]
		[TestCase(@"a/folder")]
		public void Non_rooted_file_paths_should_return_false(string input)
		{
			Assert.That(FilePath.IsRooted(input), Is.False);
		}
	}
}