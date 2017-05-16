using System;
using System.Collections.Generic;

namespace InferenceEngine
{
	public class ForwardsChaining : SearchClass
	{
		List<string> KB;

		public ForwardsChaining ()
		{
			KB = new List<string> ();
		}

		public override bool Execute (List<Statement> statements, List<Term> terms, List<Term> extras, string goal)
		{
			foreach (Term t in extras)
			{
				t.Value = true;
			}
			string before = "";
			string after = "";
			do {
				before = CreateCondition(terms);
				foreach (Statement s in statements)
				{
					s.ResolveForward ();
				}
				after = CreateCondition(terms);
			} while (before != after);
			try
			{
				if (terms.Find (p => p.Name == goal).Value == true)
				{
					KB.Add(goal);
					ConstructKB (statements, terms, terms.Find (p => p.Name == goal));
					AddExtrasToKB (extras);
				}
				else
				{
					return false;
				}	
			}
			catch
			{
				if (extras.Contains (terms.Find (p => p.Name == goal)))
				{
					return true;
				}
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

		private void AddExtrasToKB (List<Term> extras)
		{
			foreach (Term t in extras)
			{
				if ((!(KB.Contains (t.Name)))&&(t.Value == true))
				{
					KB.Add(t.Name);	
				}
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

