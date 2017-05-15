using System;
using System.Collections.Generic;

namespace InferenceEngine
{
	public class TruthTable : SearchClass
	{
		int KB;
		List<bool> states;
		bool result;

		public TruthTable ()
		{
			KB = new List<string> ();
			states = new List<bool> ();
		}

		public override bool Execute (List<Statement> statements, List<Term> terms, List<Term> extras, string goal)
		{
			result = true;
			RecursiveResolve (statements, terms, 0, goal);
			return result;
		}

		private void RecursiveResolve (List<Statement> statements, List<Term> terms, int n, string goal)
		{
			if (n < terms.Count)
			{
				terms [n].Value = true;
				foreach (Statement s in statements)
				{
					s.ResolveTT ();
				}
				RecursiveResolve (statements, terms, n + 1, goal);
				terms [n].Value = false;
				foreach (Statement s in statements)
				{
					s.ResolveTT ();
				}
				RecursiveResolve (statements, terms, n + 1, goal);	
			}
			else
			{
				bool premises = true;
				bool goalstate = true;
				foreach (Statement s in statements)
				{
					if (s.Implied.Contains (terms.Find (p => p.Name == goal)))
					{
						goalstate = s.getTTValue;
					}
					else if (s.getTTValue)
					{
						premises = false;
					}
				}
				if ((premises) && (!(goalstate)))
				{
					SearchClass BC = new BackwardsChaining ();

					foreach (Statement s in statements)
					{
						Console.Write (s.getTTValue + " ");
					}
					Console.Write (" : " + terms.Find (p => p.Name == goal).Value);
					Console.WriteLine ();
					foreach (Term t in terms)
					{
						Console.Write (t.Name + " : " + t.Value + " | ");
					}
					Console.WriteLine ();
					Console.WriteLine ("--------------------------------------------------------------------------------------");
					result = false;
				}
			}

		}

		private void ConstructKB (List<Statement> statements, List<Term> terms, Term goal)
		{
			foreach (Statement s in statements)
			{
				if (s.Implied.Contains (terms.Find (p => p.Name == goal)))
				{
					KB++;
				}
			}
		}

		private void AddExtrasToKB (List<Term> extras)
		{
			
		}

		public override string GetKBString ()
		{
			return "";
		}
	}
}

