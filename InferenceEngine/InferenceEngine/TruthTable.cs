using System;
using System.Collections.Generic;

namespace InferenceEngine
{
	public class TruthTable : SearchClass
	{
		int models;
		List<bool> states;
		int count;
		bool output;

		/// <summary>
		/// Class to control truth table checking
		/// </summary>
		/// <param name="Output">boolean representing if the user requested output or not</param>
		public TruthTable (bool Output)
		{
			states = new List<bool> ();
			output = Output;
		}

		/// <summary>
		/// Override method to execture the truthtable checking inference method
		/// </summary>
		/// <param name="statements">A list of all the statements in the KB (excluding single term statements)</param>
		/// <param name="terms">A list of all the terms in the program</param>
		/// <param name="extras">A list of the extra terms (single term statements in the KB)</param>
		/// <param name="goal">The name of the goal term</param>
		/// <returns>bool</returns>
		public override bool Execute (List<Statement> statements, List<Term> terms, List<Term> extras, string goal)
		{
			//Initialize check to false
			bool check = false;

			//Use check to see if the goal terms is actually in the statements
			foreach (Statement s in statements) 
			{
				if (s.Implied.Contains (terms.Find (p => p.Name == goal)))
				{
					check = true;
				}
			}

			//Use check to see if the goal terms is in the extras
			if (extras.Contains (terms.Find (p => p.Name == goal)))
			{
				check = true;
			}

			//If check is true, contine with the truth table checking
			if (check)
			{
				//Set models to 0 and count to 1
				models = 0;
				count = 1;

				//Call RecursiveResolve to solve the inference question
				RecursiveResolve (statements, terms, extras, 0, goal);

				//If models were found then return true
				if (models > 0)
				{
					return true;
				}
			}

			//If check is false, the goal is not within the statement and truthtable checking is automatically false
			return false;
		}

		/// <summary>
		/// Method that actually does the truth table solving
		/// </summary>
		/// <param name="statements">A list of all the statements in the KB (excluding single term statements)</param>
		/// <param name="terms">A list of all the terms in the program</param>
		/// <param name="extras">A list of the extra terms (single term statements in the KB)</param>
		/// <param name="n">level of the tree to be investigated</param>
		/// <param name="goal">The name of the goal term</param>
		private void RecursiveResolve (List<Statement> statements, List<Term> terms, List<Term> extras, int n, string goal)
		{
			//If n is less than the number of terms (e.g. not at the bottom of the tree)
			if (n < terms.Count)
			{
				//Set the term corresponding to n to true
				terms [n].Value = true;

				//Resolve each statement
				foreach (Statement s in statements)
				{
					s.ResolveTT ();
				}

				//Recursivly call this method, passing in n + 1 (the next level)
				RecursiveResolve (statements, terms, extras, n + 1, goal);

				//Set the term corresponding to n to false
				terms [n].Value = false;

				//Resolve each statement
				foreach (Statement s in statements)
				{
					s.ResolveTT ();
				}

				//Recursivly call this method, passing in n + 1 (the next level)
				RecursiveResolve (statements, terms, extras, n + 1, goal);

				//Ultimately this will create a tree structure using recursion with each term having 2 possiblities
			}
			//Else if the n value is not less than count (e.g. the method has reached the bottem of a tree branch)
			else
			{
				//Initialize premises to true
				bool premises = true;

				//If a statement with a false TTvalue is found, set premises to false
				foreach (Statement s in statements)
				{
					if (!(s.getTTValue))
					{
						premises = false;
					}
				}

				//If a term in extras is false, set premises to false
				foreach (Term t in extras)
				{
					if (!(t.Value))
					{
						premises = false;
					}
				}

				//If output is true, call the output method to output this branch to the console
				if (output)
				{
					Output (statements, terms, goal);
				}

				//Increment count
				count++;

				//If premises is true and the goal term is true, increment models
				if ((premises) && (terms.Find (p => p.Name == goal).Value))
				{
					models++;
				}
			}
		}

		/// <summary>
		/// Method to output the truth table to the console
		/// </summary>
		/// <param name="statements">A list of all the statements in the KB (excluding single term statements)</param>
		/// <param name="terms">A list of all the terms in the program</param>
		/// <param name="goal">The name of the goal term</param>
		private void Output (List<Statement> statements, List<Term> terms, string goal)
		{
			//Output the statement count and lefthand spacing
			Console.Write (count + ":" + "\t");

			//Output each statement
			foreach (Statement s in statements)
			{
				//Output the impling side of the statement, including a "&" for more than one term
				for (int i = 0; i < s.Implies.Count ; i++)
				{
					Console.Write (s.Implies[i].Name);
					if (i < s.Implies.Count - 1)
					{
						Console.Write ("&");
					}
				}

				//Output the "=>" inbetween the impliy and implied
				Console.Write ("=>");

				//Output the implied side of the statement, including a "&" for more than one term
				for (int i = 0; i < s.Implied.Count ; i++)
				{
					Console.Write (s.Implied[i].Name);
					if (i < s.Implied.Count - 1)
					{
						Console.Write ("&");
					}
				}

				//Output a "|" between statements for readability
				Console.Write (" : " + s.getTTValue + " | ");
			}

			//Output some formatting
			Console.WriteLine ();
			Console.Write ("\t");

			//Output the terms, including ":" and "|" for formatting
			foreach (Term t in terms)
			{
				Console.Write (t.Name + " : " + t.Value + " | ");
			}
			Console.WriteLine ();
			Console.WriteLine ("---------------------------------------------------------------------------------------------------------------------------------------------------");
		}

		/// <summary>
		/// Utility method to get the models count
		/// </summary>
		/// <returns>string</returns>
		public override string GetKBString ()
		{
			return models.ToString();
		}
	}
}

