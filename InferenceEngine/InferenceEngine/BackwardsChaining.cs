using System;
using System.Collections.Generic;

namespace InferenceEngine
{
	public class BackwardsChaining : SearchClass
	{
		public BackwardsChaining ()
		{
		}

		public override bool Execute (List<Statement> statements, List<Term> terms, List<Term> extras, string goal)
		{
			terms.Find (p => p.Name == goal).Value = true;
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
			return true;
		}

		public override string GetKBString ()
		{
			return "Test";
		}
	}
}

