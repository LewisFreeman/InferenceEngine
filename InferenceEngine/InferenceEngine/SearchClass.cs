using System;
using System.Collections.Generic;

namespace InferenceEngine
{
	public abstract class SearchClass
	{
		/// <summary>
		/// An abstract class to bind the search methods together
		/// </summary>
		public SearchClass ()
		{
		}

		/// <summary>
		/// An abstract method to bind the execute methods of the search classes
		/// </summary>
		/// <param name="statements">A list of statements in the program</param>
		/// <param name="terms">A list of terms in the program</param>
		/// <param name="extras">A list of the extra terms in the program</param>
		/// <param name="goal">A string of the goal name</param>
		/// <returns>bool</returns>
		public abstract bool Execute (List<Statement> statements, List<Term> terms, List<Term> extras, string goal);

		/// <summary>
		/// An abstract method to return the KB string to output to the user
		/// </summary>
		/// <returns>string</returns>
		public abstract string GetKBString ();

		/// <summary>
		/// Method used by FC and BC to check the status of the terms before and after their functions
		/// </summary>
		/// <param name="terms">A list of the terms in the program</param>
		/// <returns>string</returns>
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