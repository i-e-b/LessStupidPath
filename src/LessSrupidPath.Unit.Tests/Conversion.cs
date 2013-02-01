using System.IO;
using NUnit.Framework;

namespace LessStupidPath.Unit.Tests
{
	[TestFixture]
	public class Conversion
	{
		[Test]
		public void can_explicitly_convert_from_string ()
		{
			var result = (FilePath)"Example/Path";
			Assert.That(result.ToPosixPath(), Is.EqualTo("Example/Path"));
		}
	}
}
