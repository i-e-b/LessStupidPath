using System.IO;
using NUnit.Framework;

namespace LessStupidPath.Unit.Tests
{
	[TestFixture]
    public class PlatformOutputTests
    {
		[Test]
		[TestCase("","")]
		[TestCase("/","/")]
		[TestCase("relative\\path","relative/path")]
		[TestCase("C:\\absolute\\path","/C/absolute/path")] // iffy case
		[TestCase("\\absolute\\path","/absolute/path")]
		[TestCase("/absolute/path","/absolute/path")]
		[TestCase("mixed\\case/path","mixed/case/path")]
		[TestCase("too///many\\\\slashes","too/many/slashes")]
		[TestCase("final/slash/","final/slash")]
		public void can_normalise_to_posix_path (string input, string expected)
		{
			Assert.That(
				new FilePath(input).ToPosixPath(),
				Is.EqualTo(expected)
				);
		}
		
		[Test]
		[TestCase("","")]
		[TestCase("/","\\")]
		[TestCase("relative\\path","relative\\path")]
		[TestCase("C:\\absolute\\path","C:\\absolute\\path")] // iffy case
		[TestCase("\\absolute\\path","\\absolute\\path")]
		[TestCase("/absolute/path","\\absolute\\path")]
		[TestCase("mixed\\case/path","mixed\\case\\path")]
		[TestCase("too///many\\\\slashes","too\\many\\slashes")]
		[TestCase("final/slash/","final\\slash")]
		public void can_normalise_to_windows_path (string input, string expected)
		{
			Assert.That(
				new FilePath(input).ToWindowsPath(),
				Is.EqualTo(expected)
				);
		}
    }
}
