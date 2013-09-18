using System.IO;
using NUnit.Framework;

namespace LessStupidPath.Unit.Tests
{
	[TestFixture]
	public class FileNameTests
	{
		[Test]
		[TestCase("/whatever/this/is.txt")]
		[TestCase("/whatever/this/is.la")]
		[TestCase("/is.la")]
		[TestCase("hello/is.la")]
		[TestCase("is.la")]
		[TestCase(@"c:\is.la")]
		[TestCase(@"c:\hello\is.la")]
		[TestCase(@"is.la")]
		[TestCase(@"a\d\is.la")]
		[TestCase(@"\is.la")]
		public void Should_get_filename(string path)
		{
			var filePath = new FilePath(path);

			Assert.That(filePath.FileNameWithoutExtension(), Is.EqualTo("is"));
		}
	}
}