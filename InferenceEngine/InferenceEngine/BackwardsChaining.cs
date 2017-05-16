using System;
using System.Collections.Generic;

namespace InferenceEngine
{
	public class BackwardsChaining : SearchClass
	{
		List<string> KB;

		public BackwardsChaining ()
		{
			KB = new List<string> ();
		}

		public override bool Execute (List<Statement> statements, List<Term> terms, List<Term> extras, string goal)
		{
			try 
			{
				terms.Find (p => p.Name == goal).Value = true;
			}
			catch
			{
				if (extras.Contains (terms.Find (p => p.Name == goal)))
				{
					return true;
				}
				return false;
			}
			string before = "";
			string after = "";
			do {
				before = CreateCondition(terms);
				foreach (Statement s in statements)
				{
					s.ResolveBackward ();
				}
				after = CreateCondition(terms);
			} while (before != after);
			if (terms.Find (p => p.Name == goal).Value == true)
			{
				KB.Add(goal);
				ConstructKB (statements, terms, terms.Find (p => p.Name == goal));
			}
			else
			{
				return false;
			}
			return true;
		}

		private void ConstructKB (List<Statement> statements, List<Term> terms, Term goal)
		{
			List<Term> Chain = new List<Term> ();
			foreach (Statement s in statements)
			{
				if ((s.Implied.Contains (goal)))
				{
					Chain = s.Implies;
				}
			}
			foreach (Term t in Chain)
			{
				if (!(KB.Contains (t.Name)))
				{
					KB.Add(t.Name);	
				}
				ConstructKB (statements, terms, t);
			}
		}

		public override string GetKBString ()
		{
			string result = "";
			KB.Reverse ();
			for (int i = 0; i < KB.Count; i++)
			{
				result += KB[i];
				if (!(i == KB.Count - 1))
				{
					result += ", ";
				}
			}
			return result;
		}
	}
}

