using System;
using System.Linq;
using System.Collections.Generic;
using Xunit;
using NSubstitute;
using Parser;
using Phase05;

namespace TestProject1
{
    public class ParserTests
    {
        

        [Theory]
        [InlineData("coming", "come")]
        [InlineData("going", "go")]
        // [InlineData("ran", "run")]
        // [InlineData("had", "have")]
        // [InlineData("Playing", "play")]
        public void WordParserTest(string word, string lemma)
        {
            var parser = new WordParser();
            Assert.Equal(lemma, parser.Parse(word));
        }

        [Theory]
        [InlineData("Hi I am Coming Home.", new[] {"hi","i","am","come", "home"})]
        [InlineData("Pasha Is Running Late.", new[] {"pasha", "is","run", "late"})]
        public void SentenceParserTest(string sentence, string[] words)
        {
            var parser = new SentenceParser();
            var mock = Substitute.For<IParser<string>>();

            {
                mock.Parse("i").Returns("i");
                mock.Parse("hi").Returns("hi");
                mock.Parse("am").Returns("am");
                mock.Parse("is").Returns("is");
                mock.Parse("coming").Returns("come");
                mock.Parse("late").Returns("late");
                mock.Parse("home").Returns("home");
                mock.Parse("pasha").Returns("pasha");
                mock.Parse("running").Returns("run");
            }
            parser.WordParser = mock;
            Assert.True(words.SequenceEqual(parser.Parse(sentence)));
        }


        [Theory]
        [InlineData("Hi, I am Home. Go to your room.", new[] {"home", "go", "room"})]
        [InlineData("Hi, I am Home. Go to Pasha@Monti email address.",
            new[] {"home", "go", "pasha", "monti", "email", "address"})]
        public void DocumentParserTest(string document, string[] sentences)
        {
            var parser = new DocumentParser();
            var mockSentenceParser = Substitute.For<IParser<string[]>>();
            mockSentenceParser.Parse("Hi, I am Home").Returns(new[] {"home"});
            mockSentenceParser.Parse("Go to your room").Returns(new[] {"go", "room"});
            mockSentenceParser.Parse("Go to Pasha@Monti email address")
                .Returns(new[] {"go", "pasha", "monti", "email", "address"});
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
                new object[]
                {
                    "pasha +sia -go", new HashSet<Document>()
                    {
                        new Document(1, ""),
                        new Document(7, "")
                    }
                },
                new object[]
                {
                    "+sia +pasha -went", new HashSet<Document>()
                    {
                        new Document(1, ""),
                        new Document(7, "")
                    }
                },
                new object[]
                {
                    "+hi pasha sia +go", new HashSet<Document>()
                    {
                        new Document(2, ""),
                        new Document(17, "")
                    }
                }
            };
        }

        [Theory, MemberData(nameof(GetSearchEngineData))]
        public void SearchEngineTest(string command, HashSet<Document> result)
        {
            var searcher = new SearchEngine();
            var invertedMock = Substitute.For<ISearcher<Document>>();
            invertedMock.Search("sia").Returns(new HashSet<Document>() { new Document(1 , "") , new Document(2 , "")});
            invertedMock.Search("pasha").Returns(new HashSet<Document>() { new Document(2 , "") , new Document(7 , "")});
            invertedMock.Search("go").Returns(new HashSet<Document>() { new Document(2 , "") , new Document(17 , "")});
            invertedMock.Search("went").Returns(new HashSet<Document>() { new Document(2 , "") , new Document(17 , "")});
            invertedMock.Search("hi").Returns(new HashSet<Document>());
            searcher.Searcher = invertedMock;
            Assert.True(searcher.Search(command).SetEquals(result));
        }

        public static IEnumerable<object[]> GetInvertedIndexSearcherData()
        {
            return new[]
            {
                new object[]
                {
                    new HashSet<Document>()
                    {
                        new Document(1, "banana orange kebab"),
                        new Document(2, "fish giraffe monkey")
                    },
                    "fish",
                    new HashSet<Document>()
                    {
                        new Document(2, "fish giraffe monkey")
                    }
                }
            };
        }

        [Theory, MemberData(nameof(GetInvertedIndexSearcherData))]
        public void InvertedIndexSearcherTest(HashSet<Document> input, String searchedWord, HashSet<Document> result)
        {
            var invertedIndex = new InvertedIndexSearcher();

            var mockedDocumentParser = Substitute.For<IParser<string[]>>();
            mockedDocumentParser.Parse("banana orange kebab").Returns(new [] {"banana", "orange", "kebab"});
            mockedDocumentParser.Parse("fish giraffe monkey").Returns(new [] {"fish", "giraffe", "monkey"});
            var mockedFileLoader = Substitute.For<IFileLoader<HashSet<Document>>>();
            mockedFileLoader.Load("").Returns(input);
            var mockedWordParser = Substitute.For<IParser<string>>();
            mockedWordParser.Parse("fish").Returns("fish");
            
            invertedIndex.WordParser = mockedWordParser;
            invertedIndex.DocumentParser = mockedDocumentParser;
            invertedIndex.FileLoader = mockedFileLoader;
            invertedIndex.LoadDictionary("");
            
            Assert.True(invertedIndex.Search(searchedWord).SetEquals(result));
        }
    }
}