using System;
using System.IO;
using System.Text;
using NUnit.Framework;

namespace csharp
{
    [TestFixture]
    public class ApprovalTest
    {
        [Test]
        public void ThirtyDays()
        {
            // var lines = File.ReadAllLines("ThirtyDays.txt");
            var lines = File.ReadAllLines(FileToUseForVegaCollardKata());
            
            StringBuilder fakeoutput = new StringBuilder();
            Console.SetOut(new StringWriter(fakeoutput));
            Console.SetIn(new StringReader("a\n"));

            Program.Main(new string[] { });
            String output = fakeoutput.ToString();

            var outputLines = output.Replace("\r", string.Empty).Split('\n');
            for(var i = 0; i<Math.Min(lines.Length, outputLines.Length); i++) 
            {
                Assert.AreEqual(lines[i], outputLines[i]);
            }
        }

        private string FileToUseForVegaCollardKata()
        {
            return String.Format("{0}/VegaCollardKataThirtyDays.txt",
                AppDomain.CurrentDomain.BaseDirectory);
        }
    }
}
