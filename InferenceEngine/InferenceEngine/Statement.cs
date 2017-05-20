using System;
using System.Collections.Generic;
using System.Linq;

namespace InferenceEngine
{
	public class Statement
	{
		private List<Term> implies;
		private List<Term> implied;
		private bool TTValue;

		/// <summary>
		/// Class to represent a logical statement
		/// </summary>
		/// <param name="statement">a string containing the unformated statement</param>
		/// <param name="terms">A list of the terms the program has listed so far</param>
		public Statement (string statement, List<Term> terms)
		{
			implies = new List<Term> ();
			implied = new List<Term> ();
			TTValue = false;
			Create (statement, terms);
		}

		/// <summary>
		/// Method to turn the unformated string into a statement an find terms within to add to the list
		/// </summary>
		/// <param name="statement">a string containing the unformated statement</param>
		/// <param name="terms">A list of the terms the program has listed so far</param>
		private void Create (string statement, List<Term> terms)
		{
			//Initialize a list to store string fragments
			List<string> Parts = new List<string> ();

			//Split the statement string via the "=>" operator and put the fragments in the list
			Parts = (statement.Split (new String [] { "=>" }, StringSplitOptions.None)).ToList();

			//Foreach string peice in the list
			foreach (string str in Parts)
			{
				//If the string peice contains an "&" symbol
				if (str.Contains ('&'))
				{
					//Initialize a string array with length equal to the number of "&"s * 2 - 1, this allows the program to support any number of terms connected by an "&"
					string[] seperate = new string[(str.Count (x => x == '&'))* 2 - 1];

					//Actually fill the array with the values found from slitting the string via the "&"
					seperate = str.Split(new char[] { '&' });

					//Foreach element in the string array
					for (int i = 0; i < seperate.Length; i++)
					{
						//If a term with the name does not exist, then create it
						if (!(terms.Exists(x => x.Name == seperate[i])))
						{
							terms.Add(new Term(seperate[i]));
						}

						//If the term is on the left side of the statement it implies
						if (Parts.IndexOf (str) == 0)
						{
							implies.Add (terms.Find (p => p.Name == seperate[i]));
						}

						//Else if the term is on the right side of the statement it is implied
						else
						{
							implied.Add (terms.Find (p => p.Name == seperate[i]));
						}
					}
				}
				else
				{
					//If the statement does not contain a "&", check if the terms already exist, if not, create them
					if (!(terms.Exists(x => x.Name == str)))
					{
						terms.Add(new Term(str));
					}

					//If the term is on the left side of the statement it implies
					if (Parts.IndexOf (str) == 0)
					{
						implies.Add (terms.Find (p => p.Name == str));
					}

					//Else if the term is on the right side of the statement it is implied
					else
					{
						implied.Add (terms.Find (p => p.Name == str));
					}
				}
			}
		}

		/// <summary>
		/// Method calculate the truth table value of this statement
		/// </summary>
		public void ResolveTT ()
		{
			//If the left side is all true but the right side is all false then the statement is false
			if ((IsAllTrue (implies)) && (!(IsAllTrue (implied))))
			{
				TTValue = false;
			}
			else
			{
				TTValue = true;
			}
		}

		/// <summary>
		/// Method calculate the FC outcome of this statement
		/// </summary>
		public void ResolveForward ()
		{
			if (IsAllTrue (implies))
			{
				SetAllTrue (implied);
			}
		}

		/// <summary>
		/// Method calculate the BC outcome of this statement
		/// </summary>
		public void ResolveBackward ()
		{
			if (IsAllTrue (implied))
			{
				SetAllTrue (implies);
			}
		}

		/// <summary>
		/// Utility method to check if a set of terms is all true or not
		/// </summary>
		private bool IsAllTrue (List<Term> Terms)
		{
			bool result = true;
			foreach (Term t in Terms)
			{
				if (t.Value == false)
				{
					result = false;
				}
			}
			return result;
		}

		/// <summary>
		/// Utility method to set a list of terms to all true
		/// </summary>
		private void SetAllTrue (List<Term> Terms)
		{
			foreach (Term t in Terms)
			{
				t.Value = true;
			}
		}

		/// <summary>
		/// readonly property to get the list of impling terms
		/// </summary>
		/// <returns>List<Term></returns>
		public List<Term> Implies
		{
			get
			{
				return implies;
			}
		}

		/// <summary>
		/// readonly property to get the list of implied terms
		/// </summary>
		/// <returns>List<Term></returns>
		public List<Term> Implied
		{
			get
			{
				return implied;
			}
		}

		/// <summary>
		/// Utility method to get the statement in string form to be output to the console
		/// </summary>
		public String GetString ()
		{
			//Initialize result to ""
			string result = "";

			//For each impling term, add to the result. If not the last element put a "&" after.
			for (int i = 0; i < implies.Count; i++)
			{
				result += implies[i].Name;
				if (!(i == implies.Count - 1))
				{
					result += "&";
				}
			}

			//Add the "=>" between the impling and the implied
			result += "=>";

			//For each implied term, add to the result. If not the last element put a "&" after.
			for (int i = 0; i < implied.Count; i++)
			{
				result += implied[i].Name;
				if (!(i == implied.Count - 1))
				{
					result += "&";
				}
			}

			//return the result
			return result;
		}

		/// <summary>
		/// readonly property to get the truth table value of this statement
		/// </summary>
		/// <returns>bool</returns>
		public bool getTTValue
		{
			get
			{
				return TTValue;
			}
		}
	}
}

