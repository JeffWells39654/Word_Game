using System;
using rm.MyTrie;

namespace Wordament_Libraries
{
    public class Trie_Dictionary
    {
		private ITrie m_MyPrivateTrie;
		public Trie_Dictionary()
		{
			this.BuildFromDefaultDictionary();
		}

		private void BuildFromDefaultDictionary()
		{
			m_MyPrivateTrie = new Trie();
			/*
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
			*/

			string l_Path = AppContext.BaseDirectory + "Assets\\words_alpha.txt";
			string l_Dict_text;
			Console.WriteLine("Path" + l_Path);

			l_Dict_text = System.IO.File.ReadAllText(l_Path);
			//Todo:  Decide if this is called on a thread, or use ReadAllTextAsync
			//Todo:  Handle empty string and handle no lines
			int l_start_index = 0;
			int l_end_index = 0;
			int l_length = 0;
			string l_word;
			int l_words_thrown_out = 0;

			for (l_end_index = l_Dict_text.IndexOf('\r'); 
				l_end_index > 0; 
				l_start_index = l_end_index + 2, l_end_index = l_Dict_text.IndexOf(('\r'),l_start_index))
			{
				l_length = l_end_index - l_start_index;
				if (( l_length > 2) && (l_length < 17))
				{
					l_word = (l_Dict_text.Substring(l_start_index, l_length)).Trim();
					m_MyPrivateTrie.AddWord(l_word);

				}
				else
				{
					//Todo:  Fix dictionary file if the wrong words are included
					//Todo:  Fix dictionary file for leading and trailing spaces on words
					l_words_thrown_out++;
				}
				
			}
			
			if (l_start_index > 0)
			{
				l_word = l_Dict_text.Substring(l_start_index).Trim();
				m_MyPrivateTrie.AddWord(l_word);
			}
			Console.ReadKey();
			l_Dict_text = null;
			Console.ReadKey();
		}


    }
}
