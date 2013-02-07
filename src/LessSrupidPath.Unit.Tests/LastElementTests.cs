using System.IO;
using NUnit.Framework;

namespace LessStupidPath.Unit.Tests
{
	[TestFixture]
	public class LastElementTests
	{
		[Test]
		[TestCase("/home/john/files",  "files")]
		[TestCase("/srv/widget/files/myfile.txt",  "myfile.txt")]
		[TestCase(@"c:\Program Files\widget\files\today.txt",  "today.txt")]
		[TestCase("files",  "files")]
		[TestCase("",  null)]
		[TestCase("/files", "files")]
		public void last_element_returns_last_path_element_or_null (string source, string expected)
		{
			Assert.That(
				new FilePath(source).LastElement(),
				Is.EqualTo(expected)
				);
		}
	}
}
