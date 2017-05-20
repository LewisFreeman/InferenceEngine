using System;
using System.Collections.Generic;

namespace InferenceEngine
{
	public class BackwardsChaining : SearchClass
	{
		List<string> KB;

		/// <summary>
		/// Class to control the backwards chaining method
		/// </summary>
		public BackwardsChaining ()
		{
			KB = new List<string> ();
		}

		/// <summary>
		/// Override method to execture the backwards chaining inference method
		/// </summary>
		/// <param name="statements">A list of all the statements in the KB (excluding single term statements)</param>
		/// <param name="terms">A list of all the terms in the program</param>
		/// <param name="extras">A list of the extra terms (single term statements in the KB)</param>
		/// <param name="goal">The name of the goal term</param>
		/// <returns>bool</returns>
		public override bool Execute (List<Statement> statements, List<Term> terms, List<Term> extras, string goal)
		{
			//Set the value of the goal term to true
			try 
			{
				terms.Find (p => p.Name == goal).Value = true;
			}
			catch
			{
				//If the goal term cannot be found, return false
				return false;
			}
			//Initialize strings needed for the inference loop
			string before = "";
			string after = "";

			//Loop the inference method until the state of the terms is the same before and after (thus no more chnages can be made)
			do {
				//Set the before string to the state of the terms before the loop
				before = CreateCondition(terms);
				//Resolve each statement in the list
				foreach (Statement s in statements)
				{
					s.ResolveBackward ();
				}

				//Set the after string to the state of the terms after the loop
				after = CreateCondition(terms);
			} while (before != after);

			//If the any of the extra value are true, then goal can be infered
			bool result = false;
			foreach (Term t in extras)
			{
				if (t.Value == true)
				{
					result = true;
				}
			}
			if (result)
			{
				//Construct the KB for output
				KB.Add(goal);
				ConstructKB (statements, terms, terms.Find (p => p.Name == goal));
			}
			else
			{
				return false;
			}
			return true;
		}

		/// <summary>
		/// method to construct the KB (list of terms that the program determined were linked to the goal)
		/// </summary>
		/// <param name="statements">A list of all the statements in the KB (excluding single term statements)</param>
		/// <param name="terms">A list of all the terms in the program</param>
		/// <param name="goal">The goal term</param>
		private void ConstructKB (List<Statement> statements, List<Term> terms, Term goal)
		{
			//Initialize a list to store a chain
			List<Term> Chain = new List<Term> ();

			//Search the statements for ones contain the goal term of this method, if found add them to the chain
			foreach (Statement s in statements)
			{
				if ((s.Implied.Contains (goal)))
				{
					Chain = s.Implies;
				}
			}
			//Foreach term in the chain, add them to the KB (if not already there) then recursivly call this method again passing in the loop terms as the goal to be traced
			foreach (Term t in Chain)
			{
				if (!(KB.Contains (t.Name)))
				{
					KB.Add(t.Name);	
				}
				ConstructKB (statements, terms, t);
			}
		}

		/// <summary>
		/// Utility method to turn the KB list into a string then return it
		/// </summary>
		/// <returns>string</returns>
		public override string GetKBString ()
		{
			string result = "";

			//Reverse the list as it is assembled in opposite order to the output requirement
			KB.Reverse ();
			for (int i = 0; i < KB.Count; i++)
			{
				result += KB[i];

				//Add commas after each string execpt the last one
				if (!(i == KB.Count - 1))
				{
					result += ", ";
				}
			}
			return result;
		}
	}
}

