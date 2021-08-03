using System;
using System.Linq;
using System.Collections.Generic;
using Xunit;
using Moq;
using NSubstitute;
using Parser;
using Phase05;
using Xunit.Abstractions;
using Xunit.Extensions;
using Xunit.Sdk;


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
        [InlineData("coming", "come")]
        [InlineData("ran", "run")]
        [InlineData("had", "have")]
        [InlineData("Playing", "play")]
        public void WordParserTest(string word, string lemma)
        {
            var parser = new WordParser();
            Assert.Equal(lemma, parser.Parse(word));
        }

        [Theory]
        [InlineData("Hi I am Coming Home.", new[] {"come", "home"})]
        [InlineData("Pasha Is Running Late.", new[] {"pasha", "run", "late"})]
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
        [InlineData("Hi, I am Home. Go to your room.", new[] {"home", "go", "room"})]
        [InlineData("Hi, I am Home. Go to Pasha@Monti.com email address.",
            new[] {"home", "go", "pasha", "monti", "com", "email", "address"})]
        public void DocumentParserTest(string document, string[] sentences)
        {
            var parser = new DocumentParser();
            var mockSentenceParser = Substitute.For<SentenceParser>();
            mockSentenceParser.Parse("Hi, I am Home.").Returns(new[] {"home"});
            mockSentenceParser.Parse("Go to your room.").Returns(new[] {"go", "room"});
            mockSentenceParser.Parse("Go to Pasha@Monti.com email address.")
                .Returns(new[] {"go", "pasha", "monti", "com", "email", "address"});
            parser.SentenceParser = mockSentenceParser;
            Assert.True(sentences.SequenceEqual(parser.Parse(document)));
        }
    }

    public class SearchTest
    {
        public static IEnumerable<object[]> GetSearchEngineData()
        {
            return new[]
            {
                new object[] {"pasha +sia -go",new HashSet<Document>()
                {
                    new Document(1,""),
                    new Document(7,"")
                }},
                new object[] {"+sia +pasha -went",new HashSet<Document>()
                {
                    new Document(1,""),
                    new Document(7,"")
                }},
                new object[] {"+hi pasha sia +go",new HashSet<Document>()
                {
                    new Document(2,""),
                    new Document(17,"")
                }}
            };
        }
        
        [Theory,MemberData(nameof(GetSearchEngineData))]
        public void SearchEngineTest(string command, HashSet<Document> result)
        {
            
            var searcher = new SearchEngine();
            var invertedMock = Substitute.For<ISearcher<int>>();
            invertedMock.Search("sia").Returns(new HashSet<int>() {1, 2});
            invertedMock.Search("pasha").Returns(new HashSet<int>() {2, 7});
            invertedMock.Search("go").Returns(new HashSet<int>() {2, 17});
            invertedMock.Search("went").Returns(new HashSet<int>() {2, 17});
            invertedMock.Search("hi").Returns(new HashSet<int>());
            searcher.Searcher = invertedMock;
            Assert.True(searcher.Search(command).SetEquals(result));
        }

        public void InvertedIndexSearcher()
        {
            
        }
        
        
    }
}