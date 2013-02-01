using System;
using System.IO;
using NUnit.Framework;

namespace LessStupidPath.Unit.Tests
{
	[TestFixture]
	public class UnrootingTests
	{

		[Test]
		[TestCase("c:\\temp\\logs\\file.txt", "/c/temp/", "logs/file.txt")]
		[TestCase("c:\\temp\\logs\\file.txt", "c:\\temp\\", "logs/file.txt")]
		[TestCase("a/long/relative/path", "a/long", "relative/path")]
		[TestCase("/long/relative/path", "long/relative", "path")]
		[TestCase("/long/relative/path", "/long/relative/path", "")] // it is unrooted
		public void unrooting_removes_common_prefix_and_returns_relative_path (string full, string root, string expected)
		{
			Assert.That(
				new FilePath(full).Unroot((FilePath)root).ToPosixPath(),
				Is.EqualTo(expected)
				);
		}

		[Test]
		[TestCase("/one/two/three", "/alpha/beta/gamma")]
		[TestCase("wrong/end/bozo", "end/bozo")]
		[TestCase("nearly/but/not/quite/there", "nearly/but/wot/quite")]
		[TestCase("/short/path", "/short/path/fail/whale")]
		public void unrooting_with_a_non_common_prefix_throws_an_exception(string full, string root)
		{
			Assert.Throws<InvalidOperationException>(() =>
				new FilePath(full).Unroot((FilePath)root).ToPosixPath()
				);
		}

	}
}
