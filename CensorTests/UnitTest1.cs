using System;
using System.Runtime.InteropServices.ComTypes;
using v10s_c3ns0r;
using Xunit;
using Xunit.Abstractions;

namespace CensorTests
{
    public class UnitTest1
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public UnitTest1(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void CanCreateCensor()
        {
            _ = new Censor(null);
            // "Holy [fluffy bunny] batman!"
        }

        [Theory]
        [InlineData("badword")]
        [InlineData("8===D")]
        public void Given_BadWordInPhrase_When_Calling_HasMatch_Returns_True(string badword)
        {
            IWordDictionary dummyData = new WordDictionary()
            {
                BlackList = new[]
                {
                    badword
                }
            };
            var censor = new Censor(dummyData);
            Assert.True(censor.HasMatch($"this phrase contains a {badword}"));
        }

        [Theory]
        [InlineData("hello")]
        [InlineData("world")]
        public void Given_NoBadWordInPhrase_When_Calling_HasMatch_Returns_False(string badword)
        {
            IWordDictionary dummyData = new WordDictionary()
            {
                BlackList = new[]
                {
                    "nevergoingtomatchwoot"
                }
            };
            var censor = new Censor(dummyData);
            Assert.False(censor.HasMatch($"this phrase contains a {badword}"));
        }

        [Theory]
        [InlineData("8===D", new[] { "8===d" }, new[] { "chokidi" })]
        [InlineData("8===D", new[] { "8===D" }, new[] { "chokidi" })]
        [InlineData("badword", new[] { "badword" }, new[] { "chokidi" })]
        [InlineData("this phrase contains a botch", new [] { "botch" }, new[] { "twinkie" })]
        [InlineData("you are a badword! this phrase contains a botch!", new[] { "badword", "botch" }, new[] { "chokidi", "twinkie", "bleep", "bloop" })]
        public void Given_BadWordInPhrase_When_Calling_ReplaceAll_Returns_TheReplacedPhrase(string phrase, string[] badwords, string[] replacements)
        {
            var dummyData = new WordDictionary
            {
                BlackList = badwords,
                Replacements = replacements
            };
            var censor = new Censor(dummyData);
            var censored = censor.ReplaceAll(phrase);
            _testOutputHelper.WriteLine($"\"{phrase}\" was replaced with \"{censored}\"");
            Assert.False(censor.HasMatch(censored));
        }
    }
}
