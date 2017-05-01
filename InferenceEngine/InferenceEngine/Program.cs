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
			ForwardsChaining BC = new ForwardsChaining ();
			BC.Execute (Statements, Terms, Extras, Loader.GetGoal);
			foreach (Term t in Terms)
			{
				Console.WriteLine (t.Name + " " + t.Value);
			}
		}
	}
}
