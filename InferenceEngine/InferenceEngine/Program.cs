using System;
using System.Collections.Generic;

namespace InferenceEngine
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			if (args.Length != 2)
			{
				Console.WriteLine ("----------------------------");
				Console.WriteLine ("Please enter a valid command");
				Console.WriteLine ("----------------------------");
				System.Environment.Exit (0);
			}
			List<String> Options = new List<string> ();
			LoadOptions (Options);
			if (!(Options.Contains (args [0])))
			{
				Console.WriteLine ("----------------------------");
				Console.WriteLine ("Please enter a valid command");
				Console.WriteLine ("----------------------------");
				System.Environment.Exit (0);
			}
			Loader Loader = new Loader (args [1]);
			List<Term> Terms = new List<Term> ();
			List<Statement> Statements = new List<Statement> ();
			List<Term> Extras = new List<Term> ();
			foreach (string s in Loader.GetStatements)
			{
				if (s.Contains ("=>"))
				{
					Statements.Add (new Statement (s, Terms));
				}
				else
				{
					if (Terms.Find (p => p.Name == s) != null)
					{
						Extras.Add (Terms.Find (p => p.Name == s));	
					}
					else
					{
						Terms.Add(new Term (s));
						Extras.Add (Terms.Find (p => p.Name == s));
					}
				}
			}
			SearchClass Method = GetSearchType (args[0]);
			if (Method.Execute (Statements, Terms, Extras, Loader.GetGoal))
			{
				Console.WriteLine ("YES: " + Method.GetKBString());
			}
			else
			{
				Console.WriteLine ("NO");
			}
		}

		private static void LoadOptions (List<String> Options)
		{
			Options.Add("FC");
			Options.Add("BC");
			Options.Add("TT");
			Options.Add("FORWARDSCHAINING");
			Options.Add("BACKWARDSCHAINING");
			Options.Add("TRUTHTABLE");
			Options.Add("TRUTHTABLECHECKING");
		}

		private static SearchClass GetSearchType (string Method)
		{
			SearchClass SearchClass = new ForwardsChaining ();
			switch (Method)
			{
			case "FC":
			case "FORWARDSCHAINING":
				break;
			case "BC":
			case "BACKWARDSCHAINING":
				SearchClass = new BackwardsChaining ();
				break;
			case "TT":
			case "TRUTHTABLE":
			case "TRUTHTABLECHECKING":
				SearchClass = new TruthTable ();
				break;
			}
			return SearchClass;
		}
	}
}
