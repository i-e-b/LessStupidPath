using System;
using System.IO;
using NUnit.Framework;

namespace LessStupidPath.Unit.Tests
{
	[TestFixture]
	public class ExtensionTests
	{
		[Test]
		[TestCase(@"c:\yourmum.txt", "txt" )]
		[TestCase(@"c:\yourmum.TXT", "txt" )]
		[TestCase(@"c:\yourmum.recent\something.exe", "exe" )]
		[TestCase(@"/yourmum.txt", "txt")]
		[TestCase(@"/yourmum.TXT", "txt")]
		[TestCase(@"/yourmum.recent/something.tar", "tar")]
		[TestCase(@"\\starfox\blah.tar", "tar")]
		public void Should_get_file_extension(string input, string expected)
		{
			var actual = new FilePath(input).Extension();
			Assert.That(actual, Is.EqualTo(expected));
		}

		[Test]
		[TestCase(@"c:\filewithoutExtension")]
		[TestCase(@"c:\directoryname.withperiod\filewithoutExtension")]
		[TestCase(@"/home/bob/directoryname.withperiod/filewithoutExtension")]
		public void Should_throw_if_file_doesnt_have_an_extension(string inputPath)
		{
			var filePath = new FilePath(inputPath);
			var ex = Assert.Throws<InvalidOperationException>(()=>filePath.Extension());
			Assert.That(ex.Message, Is.EqualTo(string.Format("{0} does not have an extension", filePath.ToEnvironmentalPath())));
		}
	}
}