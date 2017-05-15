using System;
using System.Collections.Generic;

namespace InferenceEngine
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			//Check that the required number of command line arguments are entered
			if ((args.Length < 2) || (args.Length > 3))
			{
				Console.WriteLine ("----------------------------");
				Console.WriteLine ("Please enter a valid command");
				Console.WriteLine ("----------------------------");
				System.Environment.Exit (0);
			}

			//Initialize and fill the options list
			List<String> Options = new List<string> ();
			LoadOptions (Options);

			//Check that the method requested (args[0]) is valid
			if (!(Options.Contains (args [0].ToUpper())))
			{
				Console.WriteLine ("----------------------------");
				Console.WriteLine ("Please enter a valid command");
				Console.WriteLine ("----------------------------");
				System.Environment.Exit (0);
			}

			//Load in the file requested
			Loader Loader = new Loader (args [1]);

			//Initialize lists to store data from the file
			List<Term> Terms = new List<Term> ();
			List<Statement> Statements = new List<Statement> ();
			List<Term> Extras = new List<Term> ();

			//Move data from the loader class into the proper lists
			foreach (string s in Loader.GetStatements)
			{
				//If the statements has a "=>" in it then it is a statement
				if (s.Contains ("=>"))
				{
					Statements.Add (new Statement (s, Terms));
				}
				else
				{
					//If the statement does not have a "=>" then it is an extra term
					if (!(Terms.Find (p => p.Name == s) != null))
					{
						//If the term does not exist already, create it
						Terms.Add(new Term (s));
					}

					//add the extra terms to the list
					Extras.Add (Terms.Find (p => p.Name == s));	
				}
			}

			//Initialize the method to be used
			SearchClass Method = GetSearchType (args[0], false);
			try
			{
				if (args [2].ToUpper() == "OUTPUT")
				{
					//If output is requested reassign it with this value
					Method = GetSearchType (args[0], true);
				}
			}
			catch
			{
			}

			//Execute the inference and output the responce
			if (Method.Execute (Statements, Terms, Extras, Loader.GetGoal))
			{
				Console.WriteLine ("YES: " + Method.GetKBString());
			}
			else
			{
				Console.WriteLine ("NO");
			}
		}

		/// <summary>
		/// A utility method to fill the options list with possible entry values
		/// </summary>
		/// <param name="Options">The list ot be filled</param>
		private static void LoadOptions (List<String> Options)
		{
			Options.Add("FC");
			Options.Add("BC");
			Options.Add("TT");
			Options.Add("FORWARDSCHAINING");
			Options.Add("BACKWARDSCHAINING");
			Options.Add("TRUTHTABLE");
			Options.Add("TRUTHTABLECHECKING");
		}

		/// <summary>
		/// A utility method to turn the users entered method into a method type
		/// </summary>
		/// <param name="Method">The method the user entered</param>
		/// <param name="output">a boolean representing if the user selected output or not</param>
		/// <returns>SearchClass</returns>
		private static SearchClass GetSearchType (string Method, bool output)
		{
			SearchClass SearchClass = new ForwardsChaining ();
			switch (Method)
			{
			case "FC":
			case "FORWARDSCHAINING":
				break;
			case "BC":
			case "BACKWARDSCHAINING":
				SearchClass = new BackwardsChaining ();
				break;
			case "TT":
			case "TRUTHTABLE":
			case "TRUTHTABLECHECKING":
				SearchClass = new TruthTable (output);
				break;
			}
			return SearchClass;
		}
	}
}
