using System;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

//using Chaining Assertion https://github.com/neuecc/ChainingAssertion

namespace HistoryCollection.Tests
{
    [TestClass()]
    public class ColorTranslatorTests
    {
        [TestMethod()]
        public void FromHtmlTest()
        {
            AssertEx.Throws<ArgumentNullException>(() => ColorTranslator.FromHtml(null));
            AssertEx.Throws<ArgumentNullException>(() => ColorTranslator.FromHtml(""));
            AssertEx.Throws<ArgumentNullException>(() => ColorTranslator.FromHtml(" "));

            var c = Color.FromArgb(0xFF, 0x00, 0x00);
            ColorTranslator.FromHtml("Red").Is(Color.Red);
            ColorTranslator.FromHtml("Red").IsNot(c);
            ColorTranslator.FromHtml("Red").ToArgb().Is(c.ToArgb());

            ColorTranslator.FromHtml("#FFFF0000").Is(c);
            ColorTranslator.FromHtml("#FF0000").Is(c);
            ColorTranslator.FromHtml("#FF00").Is(c);
            ColorTranslator.FromHtml("#F00").Is(c);

            c = Color.FromArgb(0x88, 0x88, 0x88, 0x88);
            ColorTranslator.FromHtml("#88888888").Is(c);
            ColorTranslator.FromHtml("#8888").Is(c);

            AssertEx.Throws<ArgumentException>(() => ColorTranslator.FromHtml("#F"));
            AssertEx.Throws<ArgumentException>(() => ColorTranslator.FromHtml("#FF"));
            AssertEx.Throws<ArgumentException>(() => ColorTranslator.FromHtml("#ZZZ"));
            AssertEx.Throws<ArgumentException>(() => ColorTranslator.FromHtml("Reed"));
        }

        [TestMethod()]
        public void ToHtmlTest()
        {
            ColorTranslator.ToHtml(Color.Red).Is("Red");
            ColorTranslator.ToHtml(Color.FromArgb(0xFF, 0x00, 0x00)).Is("#FF0000");
            ColorTranslator.ToHtml(Color.FromArgb(0x88, 0x88, 0x88, 0x88)).Is("#88888888");
        }
    }
}