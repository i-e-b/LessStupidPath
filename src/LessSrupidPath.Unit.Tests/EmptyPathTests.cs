using System.IO;
using NUnit.Framework;

namespace LessStupidPath.Unit.Tests
{
	[TestFixture]
	public class EmptyPathTests
	{
		[Test]
		[TestCase(".")]
		[TestCase("/home")]
		[TestCase("/")]
		[TestCase("home")]
		public void non_empty_paths_give_empty_equal_to_false(string path)
		{
			Assert.That(
				new FilePath(path).IsEmpty(),
				Is.False);
		}

		[Test]
		[TestCase("")]
		[TestCase(" ")]
		public void empty_paths_give_empty_equal_to_true(string path)
		{
			Assert.That(
				new FilePath(path).IsEmpty(),
				Is.True);
		}
	}
}