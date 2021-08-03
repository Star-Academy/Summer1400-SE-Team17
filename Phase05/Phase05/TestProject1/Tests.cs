
using System;
using System.Linq;
using System.Collections.Generic;
using Xunit;
using Moq;
using NSubstitute;
using Parser;
using Phase05;
using Xunit.Abstractions;

namespace TestProject1
{
    public class ParserTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public ParserTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Theory]
        [InlineData("coming","come")]
        [InlineData("ran","run")]
        [InlineData("had","have")]
        [InlineData("Playing","play")]
   
        public void WordParserTest(string word,string lemma)
        {
            var parser = new WordParser();
            Assert.Equal(lemma,parser.Parse(word));
        }

        [Theory]
        [InlineData("Hi I am Coming Home.",new[]{"come","home"})]
        [InlineData("Pasha Is Running Late.",new[]{"pasha","run","late"})]
        public void SentenceParserTest(string sentence, string[] words)
        {
            var parser = new SentenceParser();
            var mock = Substitute.For<WordParser>();
            
            {
                mock.Parse("I").Returns("");
                mock.Parse("Hi").Returns("");
                mock.Parse("Is").Returns("");
                mock.Parse("Coming").Returns("come");
                mock.Parse("Late").Returns("late");
                mock.Parse("Home").Returns("home");
                mock.Parse("Pasha").Returns("pasha");
                mock.Parse("Running").Returns("run");
            }
            parser.WordParser = mock;
            Assert.True(words.SequenceEqual(parser.Parse(sentence)));
        }


        [Theory]
        [InlineData("Hi, I am Home. Go to your room.",new [] {"home","go","room"})]
        [InlineData("Hi, I am Home. Go to Pasha@Monti.com email address.",new [] {"home","go","pasha","monti","com","email","address"})]
        public void DocumentParserTest(string document,string[] sentences)
        {
            var parser = new DocumentParser();
            var mockSentenceParser =Substitute.For<SentenceParser>();
            mockSentenceParser.Parse("Hi, I am Home.").Returns(new []{"home"});
            mockSentenceParser.Parse("Go to your room.").Returns(new[] {"go","room"});
            mockSentenceParser.Parse("Go to Pasha@Monti.com email address.")
                .Returns(new[] {"go", "pasha", "monti", "com", "email", "address"});
            parser.SentenceParser = mockSentenceParser;
            Assert.True(sentences.SequenceEqual(parser.Parse(document)));
        }
        
        
        
        
    }
    
}