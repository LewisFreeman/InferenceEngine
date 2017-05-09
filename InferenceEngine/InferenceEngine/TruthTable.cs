using System;
using System.Collections.Generic;

namespace InferenceEngine
{
	public class TruthTable : SearchClass
	{
		List<string> KB;
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
			if (n < terms.Count + 1)
			{
				terms [n].Value = true;
				foreach (Statement s in statements) {
					s.ResolveTT ();
				}
				RecursiveResolve (statements, terms, n + 1, goal);
				terms [n].Value = false;
				foreach (Statement s in statements) {
					s.ResolveTT ();
				}
				RecursiveResolve (statements, terms, n + 1, goal);	
			}
			else
			{
				bool premises = true;
				foreach (Statement s in statements)
				{
					if (!(s.getTTValue))
					{
						premises = false;
					}
				}
				if (!((premises) && (terms.Find (p => p.Name == goal).Value)))
				{
					result = false;
				}
			}

		}

		private void ConstructKB (List<Statement> statements, List<Term> terms, Term goal)
		{
			
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

