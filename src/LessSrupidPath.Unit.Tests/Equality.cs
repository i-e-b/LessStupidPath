using System.IO;
using NUnit.Framework;

namespace LessStupidPath.Unit.Tests
{
	[TestFixture]
	public class Equality
	{
		[Test]
		[TestCase("a/b/c", "a\\b\\c")]
		[TestCase("one/../two", "two")]
		[TestCase(@"c:\path\to", @"c:\path\to")]
		public void are_equal(string left, string right)
		{
			Assert.That(new FilePath(left), Is.EqualTo(new FilePath(right)));
			Assert.That(new FilePath(left) == new FilePath(right));
		}

		[Test]
		[TestCase("/a/b/c", "a/b/c")]
		[TestCase("one/./two", "two")]
		public void not_equal(string left, string right)
		{
			Assert.That(new FilePath(left), Is.Not.EqualTo(new FilePath(right)));
			Assert.That(new FilePath(left) != new FilePath(right));
		}
	}
}
