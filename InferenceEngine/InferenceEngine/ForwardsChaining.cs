using System;
using System.Collections.Generic;

namespace InferenceEngine
{
	public class ForwardsChaining : SearchClass
	{
		public ForwardsChaining ()
		{
		}

		public override bool Execute (List<Statement> statements, List<Term> terms, List<String> extras, string goal)
		{
			foreach (String s in extras)
			{
				try
				{
					terms.Find (p => p.Name == s).Value = true;
				}
				catch
				{}
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
			return true;
		}
	}
}

