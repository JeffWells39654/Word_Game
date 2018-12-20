using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using rm.MyTrie;
using System.Collections;

namespace rm.MyTrie.Test
{
	[TestClass]
	public class UnitTest1
	{
		private static ITrie trie;

		[TestMethod]
		public void TestMethod1()
		{
		}

		#region TestClassOverhead
		[ClassInitialize]
		public static void SetUp(TestContext context)
		{
			trie = BuildSampleTrie();
		}
		[ClassCleanup]
		public static void TearDown()
		{
			trie = null;
		}
		private static ITrie BuildSampleTrie()
		{
			ITrie trie = new rm.MyTrie.Trie();
			string[] strings =
			{
					"123", "1", "23", "1",
					"this", "test", "the", "TEMP", "TOKEN", "TAKE", "THUMP"
				};

			foreach (string s in strings)
			{
				trie.AddWord(s);
			}
			return trie;
		}
		#endregion

		#region Tests

		[TestMethod]
		public void GetWords01()
		{
			var words = trie.GetWords();
			Assert.AreEqual(10, words.Count);
		}

		[TestMethod]
		public void GetWords_Prefix01()
		{
			var prefixWordsEmpty = trie.GetWords("");
			Assert.AreEqual(10, prefixWordsEmpty.Count);
		}

		[TestMethod]
		public void GetWords_Prefix02()
		{
			var prefixWords1 = trie.GetWords("th");
			Assert.AreEqual(2, prefixWords1.Count);
		}

		[TestMethod]
		public void GetWords_Prefix03()
		{
			var prefixWords1Upper = trie.GetWords("TH");
			Assert.AreEqual(1, prefixWords1Upper.Count);
		}

		[TestMethod]
		public void GetWords_Prefix04()
		{
			var prefixWords2 = trie.GetWords("z");
			Assert.AreEqual(0, prefixWords2.Count);
		}

		[TestMethod]
		public void GetWords_Prefix05()
		{
			var prefixWords2Upper = trie.GetWords("Z");
			Assert.AreEqual(0, prefixWords2Upper.Count);
		}

		[TestMethod]
		public void GetWords_Prefix06()
		{
			var prefixWords3Digits = trie.GetWords("1");
			Assert.AreEqual(2, prefixWords3Digits.Count);
		}

		[TestMethod]
		public void HasWord01()
		{
			bool hasWord1 = trie.HasWord("test");
			Assert.IsTrue(hasWord1);
		}

		[TestMethod]
		public void HasWord02()
		{
			bool hasWord1Upper = trie.HasWord("TEST");
			Assert.IsFalse(hasWord1Upper);
		}

		[TestMethod]
		public void HasWord03()
		{
			bool hasWord2 = trie.HasWord("zz");
			Assert.IsFalse(hasWord2);
		}

		[TestMethod]
		public void HasWord04()
		{
			bool hasWord2Upper = trie.HasWord("ZZ");
			Assert.IsFalse(hasWord2Upper);
		}

		[TestMethod]
		public void HasWord05()
		{
			bool prefixButNotWord = trie.HasWord("tes");
			Assert.IsFalse(prefixButNotWord);
		}

		[TestMethod]
		public void HasPrefix01()
		{
			bool hasPrefix1 = trie.HasPrefix("tes");
			Assert.IsTrue(hasPrefix1);
		}

		[TestMethod]
		public void HasPrefix02()
		{
			bool hasPrefix2 = trie.HasPrefix("test");
			Assert.IsTrue(hasPrefix2);
		}

		[TestMethod]
		public void HasPrefix03()
		{
			bool hasPrefix1Upper = trie.HasPrefix("TES");
			Assert.IsFalse(hasPrefix1Upper);
		}

		[TestMethod]
		public void HasPrefix04()
		{
			bool hasPrefix2Upper = trie.HasPrefix("TEST");
			Assert.IsFalse(hasPrefix2Upper);
		}


		public void RemoveWord01()
		{
			Assert.AreEqual(1, trie.RemoveWord("this"));
			Assert.AreEqual(9, trie.GetWords().Count);
			Assert.AreEqual(1, trie.RemoveWord("the"));
			Assert.AreEqual(8, trie.GetWords().Count);
			Assert.ThrowsException<ArgumentOutOfRangeException>(() => trie.RemoveWord("te"));
			Assert.AreEqual(8, trie.GetWords().Count);
			Assert.AreEqual(1, trie.RemoveWord("test"));
			Assert.AreEqual(7, trie.GetWords().Count);
			Assert.ThrowsException<ArgumentOutOfRangeException>(() => trie.RemoveWord("word not present"));
			Assert.AreEqual(7, trie.GetWords().Count);
			Assert.AreEqual(1, trie.RemoveWord("123"));
			Assert.AreEqual(2, trie.RemoveWord("1"));
			foreach (var word in trie.GetWords())
			{
				trie.RemoveWord(word);
			}
			Assert.AreEqual(0, trie.GetWords().Count);
		}


		public void RemovePrefix01()
		{
			trie.RemovePrefix("1");
			Assert.AreEqual(8, trie.GetWords().Count);
			trie.RemovePrefix("th");
			Assert.AreEqual(6, trie.GetWords().Count);
			trie.RemovePrefix("x");
			Assert.AreEqual(6, trie.GetWords().Count);
			trie.RemovePrefix("");
			Assert.AreEqual(0, trie.GetWords().Count);
		}


		public void AddWord_EmptyString01()
		{
			trie = new Trie();
			Assert.AreEqual(0, trie.GetWords().Count);
			trie.AddWord("");
			Assert.AreNotEqual(0, trie.GetWords().Count);
		}


		public void AddWord_RemoveWord01()
		{
			var trie = new Trie();
			trie.AddWord("");
			trie.AddWord("");
			Assert.AreEqual(1, trie.GetWords().Count);
			Assert.AreEqual(2, trie.RemoveWord(""));
			Assert.AreEqual(0, trie.GetWords().Count);
			trie.AddWord("");
			Assert.AreEqual(1, trie.GetWords().Count);
			Assert.AreEqual(1, trie.RemoveWord(""));
			Assert.AreEqual(0, trie.GetWords().Count);
		}

		[TestMethod]
		public void GetLongestWords01()
		{
			trie.AddWord("the longest word");
			var expected = new[] { "the longest word" };
			var longestWords = trie.GetLongestWords();
			Assert.AreEqual(expected.Length, longestWords.Count);
			string longWord;
			IEnumerator e1 = longestWords.GetEnumerator();
			e1.MoveNext();
			longWord = (string)e1.Current;
			Assert.AreEqual(expected[0], longWord);
		}

		[TestMethod]
		public void GetLongestWords02()
		{
			trie.AddWord("the longest word 1");
			trie.AddWord("the longest word 2");
			var expected = new[] { "the longest word 1", "the longest word 2" };
			var longestWords = trie.GetLongestWords();
			Assert.AreEqual(expected, longestWords);
		}

		[TestMethod]
		public void GetShortestWords01()
		{
			trie.AddWord("a");
			var expected = new[] { "1", "a" };
			var shortestWords = trie.GetShortestWords();
			Assert.AreEqual(expected, shortestWords);
		}

		[TestMethod]
		public void GetShortestWords02()
		{
			trie.AddWord("");
			var expected = new[] { "" };
			var shortestWords = trie.GetShortestWords();
			Assert.AreEqual(expected, shortestWords);
		}


		public void Clear01()
		{
			Assert.AreNotEqual(0, trie.GetWords().Count);
			trie.Clear();
			Assert.AreEqual(0, trie.GetWords().Count);
		}

		[TestMethod]
		public void Count01()
		{
			Assert.AreEqual(11, trie.Count());
		}

		[TestMethod]
		public void UniqueCount01()
		{
			Assert.AreEqual(10, trie.UniqueCount());
		}


		#endregion
	}
}
