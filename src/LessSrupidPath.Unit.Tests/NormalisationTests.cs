using System;
using System.IO;
using NUnit.Framework;

namespace LessStupidPath.Unit.Tests
{
	[TestFixture]
	public class NormalisationTests
	{
		[Test]
		[TestCase("../one/of/two/..","../one/of")]
		[TestCase("../one/./two/..","../one")]
		[TestCase("/one/./two/../..","/")]
		[TestCase("one/./two/../..","")]
		[TestCase("./one","one")]
		public void normalising_a_path_removes_dot_specs_where_possible (string input, string expected)
		{
			Assert.That(
				new FilePath(input).Normalise().ToPosixPath(),
				Is.EqualTo(expected)
				);
		}

		
		[Test]
		[TestCase("who/.../would./.make/a/.p.a.t.h/like../.../this?")]
		public void normalising_a_path_ignores_elements_other_than_single_and_double_dots (string crazyPath)
		{
			Assert.That(
				new FilePath(crazyPath).Normalise().ToPosixPath(),
				Is.EqualTo(crazyPath)
				);
		}

		
		[Test]
		public void normalising_an_invalid_rooted_path_throws_an_exception ()
		{
			Assert.Throws<InvalidOperationException>(() =>
				new FilePath("/very/../../../wrong").Normalise()
				);
		}
	}
}
