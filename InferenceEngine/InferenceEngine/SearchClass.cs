using System;
using System.Collections.Generic;

namespace InferenceEngine
{
	public abstract class SearchClass
	{
		public SearchClass ()
		{
		}

		public abstract bool Execute (List<Statement> statements, List<Term> terms, List<Term> extras, string goal);
		public abstract string GetKBString ();

		public string CreateCondition (List<Term> terms)
		{
			string result = "";
			foreach (Term t in terms)
			{
				result += t.Value;
			}
			return result;
		}
	}
}