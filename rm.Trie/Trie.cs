﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace rm.MyTrie
{
	/// <summary>
	/// Trie data structure.
	/// </summary>
	public class Trie : ITrie
	{
		#region Data Members

		/// <summary>
		/// Root TrieNode.
		/// </summary>
		private TrieNode rootTrieNode { get; set; }

		#endregion

		#region Ctors

		/// <summary>
		/// Creates a new Trie instance.
		/// </summary>
		public Trie()
		{
			rootTrieNode = new TrieNode(' ');
		}

		#endregion

		#region ITrie methods

		/// <summary>
		/// Adds a word to the Trie.
		/// </summary>
		public void AddWord(string word)
		{
			if (word == null)
			{
				throw new ArgumentNullException(nameof(word));
			}
			AddWord(rootTrieNode, word.ToCharArray());
		}

		/// <summary>
		/// Removes word from the Trie.
		/// </summary>
		/// <returns>Count of words removed.</returns>
		public int RemoveWord(string word)
		{
			if (word == null)
			{
				throw new ArgumentNullException(nameof(word));
			}
			return RemoveWord(GetTrieNodesStack(word));
		}

		/// <summary>
		/// Removes words by prefix from the Trie.
		/// </summary>
		public void RemovePrefix(string prefix)
		{
			if (prefix == null)
			{
				throw new ArgumentNullException(nameof(prefix));
			}
			RemovePrefix(GetTrieNodesStack(prefix, false));
		}

		/// <summary>
		/// Gets all words in the Trie.
		/// </summary>
		public ICollection<string> GetWords()
		{
			return GetWords("");
		}

		/// <summary>
		/// Gets words for given prefix.
		/// </summary>
		public ICollection<string> GetWords(string prefix)
		{
			if (prefix == null)
			{
				throw new ArgumentNullException(nameof(prefix));
			}
			// Empty list if no prefix match
			var words = new List<string>();
			var buffer = new StringBuilder();
			buffer.Append(prefix);
			GetWords(GetTrieNode(prefix), words, buffer);
			return words;
		}

		/// <summary>
		/// Returns true if the word is present in the Trie.
		/// </summary>
		public bool HasWord(string word)
		{
			if (word == null)
			{
				throw new ArgumentNullException(nameof(word));
			}
			var trieNode = GetTrieNode(word);
			return trieNode?.IsWord ?? false;
		}

		/// <summary>
		/// Returns true if the prefix is present in the Trie.
		/// </summary>
		public bool HasPrefix(string prefix)
		{
			if (prefix == null)
			{
				throw new ArgumentNullException(nameof(prefix));
			}
			return GetTrieNode(prefix) != null;
		}

		/// <summary>
		/// Returns the count for the word in the Trie.
		/// </summary>
		public int WordCount(string word)
		{
			if (word == null)
			{
				throw new ArgumentNullException(nameof(word));
			}
			var trieNode = GetTrieNode(word);
			return trieNode?.WordCount ?? 0;
		}

		/// <summary>
		/// Gets longest words from the Trie.
		/// </summary>
		public ICollection<string> GetLongestWords()
		{
			var longestWords = new List<string>();
			var buffer = new StringBuilder();
			var length = new Wrapped<int>(0);
			GetLongestWords(rootTrieNode, longestWords, buffer, length);
			return longestWords;
		}

		/// <summary>
		/// Gets shortest words from the Trie.
		/// </summary>
		public ICollection<string> GetShortestWords()
		{
			var shortestWords = new List<string>();
			var buffer = new StringBuilder();
			var length = new Wrapped<int>(int.MaxValue);
			GetShortestWords(rootTrieNode, shortestWords, buffer, length);
			return shortestWords;
		}

		/// <summary>
		/// Clears all words from the Trie.
		/// </summary>
		public void Clear()
		{
			rootTrieNode.Clear();
		}

		/// <summary>
		/// Gets total word count in the Trie.
		/// </summary>
		public int Count()
		{
			var count = new Wrapped<int>(0);
			GetCount(rootTrieNode, count, false);
			return count.Value;
		}

		/// <summary>
		/// Gets unique word count in the Trie.
		/// </summary>
		public int UniqueCount()
		{
			var count = new Wrapped<int>(0);
			GetCount(rootTrieNode, count, true);
			return count.Value;
		}

		#endregion

		#region Private methods

		/// <summary>
		/// Gets the equivalent TrieNode in the Trie for given prefix. 
		/// If prefix not present, then return null.
		/// </summary>
		private TrieNode GetTrieNode(string prefix)
		{
			var trieNode = rootTrieNode;
			foreach (var prefixChar in prefix)
			{
				trieNode = trieNode.GetChild(prefixChar);
				if (trieNode == null)
				{
					break;
				}
			}
			return trieNode;
		}

		/// <summary>
		/// Adds words recursively.
		/// <para>
		/// Gets the first char of the word, creates the child TrieNode if null, 
		/// and recurses with the first char removed from the word. If the word
		/// length is 0, return.
		/// </para>
		/// </summary>
		private void AddWord(TrieNode trieNode, char[] word)
		{
			foreach (var c in word)
			{
				var child = trieNode.GetChild(c);
				if (child == null)
				{
					child = new TrieNode(c);
					trieNode.SetChild(child);
				}
				trieNode = child;
			}
			trieNode.WordCount++;
		}

		/// <summary>
		/// Gets all the words recursively starting from given TrieNode.
		/// </summary>
		private void GetWords(TrieNode trieNode, ICollection<string> words,
			StringBuilder buffer)
		{
			if (trieNode == null)
			{
				return;
			}
			if (trieNode.IsWord)
			{
				words.Add(buffer.ToString());
			}
			foreach (var child in trieNode.GetChildren())
			{
				buffer.Append(child.Character);
				GetWords(child, words, buffer);
				// Remove recent character
				buffer.Length--;
			}
		}

		/// <summary>
		/// Gets longest words recursively starting from given TrieNode.
		/// </summary>
		private void GetLongestWords(TrieNode trieNode,
			ICollection<string> longestWords, StringBuilder buffer, Wrapped<int> length)
		{
			if (trieNode.IsWord)
			{
				if (buffer.Length > length.Value)
				{
					longestWords.Clear();
					length.Value = buffer.Length;
				}
				if (buffer.Length == length.Value)
				{
					longestWords.Add(buffer.ToString());
				}
			}
			foreach (var child in trieNode.GetChildren())
			{
				buffer.Append(child.Character);
				GetLongestWords(child, longestWords, buffer, length);
				// Remove recent character
				buffer.Length--;
			}
		}

		/// <summary>
		/// Gets shortest words recursively starting from given TrieNode.
		/// </summary>
		private void GetShortestWords(TrieNode trieNode,
			ICollection<string> shortestWords, StringBuilder buffer, Wrapped<int> length)
		{
			if (trieNode.IsWord)
			{
				if (buffer.Length < length.Value)
				{
					shortestWords.Clear();
					length.Value = buffer.Length;
				}
				if (buffer.Length == length.Value)
				{
					shortestWords.Add(buffer.ToString());
				}
			}
			foreach (var child in trieNode.GetChildren())
			{
				buffer.Append(child.Character);
				GetShortestWords(child, shortestWords, buffer, length);
				// Remove recent character
				buffer.Length--;
			}
		}

		/// <summary>
		/// Gets stack of trieNodes for given string.
		/// </summary>
		private Stack<TrieNode> GetTrieNodesStack(string s, bool isWord = true)
		{
			var nodes = new Stack<TrieNode>(s.Length + 1);
			var trieNode = rootTrieNode;
			nodes.Push(trieNode);
			foreach (var c in s)
			{
				trieNode = trieNode.GetChild(c);
				if (trieNode == null)
				{
					nodes.Clear();
					break;
				}
				nodes.Push(trieNode);
			}
			if (isWord)
			{
				if (!trieNode?.IsWord ?? true)
				{
					throw new ArgumentOutOfRangeException($"{s} does not exist in trie.");
				}
			}
			return nodes;
		}

		/// <summary>
		/// Removes word and trims.
		/// </summary>
		private int RemoveWord(Stack<TrieNode> trieNodes)
		{
			var removeCount = trieNodes.Peek().WordCount;
			// Mark the last trieNode as not a word
			trieNodes.Peek().WordCount = 0;
			// Trim excess trieNodes
			Trim(trieNodes);
			return removeCount;
		}

		/// <summary>
		/// Removes prefix and trims.
		/// </summary>
		private void RemovePrefix(Stack<TrieNode> trieNodes)
		{
			if (trieNodes.Any())
			{
				// Clear the last trieNode
				trieNodes.Peek().Clear();
				// Trim excess trieNodes
				Trim(trieNodes);
			}
		}

		/// <summary>
		/// Removes unneeded trieNodes going up from a trieNode to root.
		/// </summary>
		/// <remarks>
		/// TrieNode, except root, that is not a word or has no children can be removed.
		/// </remarks>
		private void Trim(Stack<TrieNode> trieNodes)
		{
			while (trieNodes.Count > 1)
			{
				var trieNode = trieNodes.Pop();
				var parentTrieNode = trieNodes.Peek();
				if (trieNode.IsWord || trieNode.GetChildren().Any())
				{
					break;
				}
				parentTrieNode.RemoveChild(trieNode.Character);
			}
		}

		/// <summary>
		/// Gets word count in the Trie.
		/// </summary>
		private void GetCount(TrieNode trieNode, Wrapped<int> count, bool isUnique)
		{
			if (trieNode.IsWord)
			{
				count.Value += isUnique ? 1 : trieNode.WordCount;
			}
			foreach (var child in trieNode.GetChildren())
			{
				GetCount(child, count, isUnique);
			}
		}

		#endregion
	}
}
