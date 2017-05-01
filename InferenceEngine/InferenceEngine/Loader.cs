using System;
using System.Collections.Generic;
using System.IO;

namespace InferenceEngine
{
	public class Loader
	{
		List<string> lines;
		List<string> Statements;
		string Goal;

		public Loader (string _fileName)
		{
			lines = new List<string> ();
			Statements = new List<string> ();
			Read (_fileName);
			Deformat ();
		}
			
		private void Read (string _fileName)
		{
			try
			{
				using (StreamReader sr = File.OpenText(_fileName))
				{
					string line;
					while ((line = sr.ReadLine()) != null)
					{
						lines.Add (line);
					}
				}
			}
			catch
			{
				Console.WriteLine ("-------------------------------------");
				Console.WriteLine ("The program cannot find the text file");
				Console.WriteLine ("-------------------------------------");
				System.Environment.Exit (0);
			}
		}

		private void Deformat ()
		{
			string[] parts = lines [1].Split (';');
			foreach (string s in parts)
			{
				Statements.Add(s.Replace (" ", string.Empty));
			}
			Statements.Remove (Statements[Statements.Count - 1]);
			Goal = lines [3];
		}

		public List<string> GetStatements
		{
			get
			{
				return Statements;
			}
		}

		public string GetGoal
		{
			get
			{
				return Goal;
			}
		}
	}
}

