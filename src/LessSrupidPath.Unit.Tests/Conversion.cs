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

		[Test]
		public void cant_implicitly_cast_from_string ()
		{
			object x = "hello";
			Assert.That(x as FilePath, Is.Null);
		}
	}
}
