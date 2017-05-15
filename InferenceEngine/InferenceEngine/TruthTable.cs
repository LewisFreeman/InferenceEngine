using System;
using System.Collections.Generic;

namespace InferenceEngine
{
	public class TruthTable : SearchClass
	{
		int KB;
		List<bool> states;
		bool result;
		int count;
		bool output;

		public TruthTable (bool Output)
		{
			states = new List<bool> ();
			output = Output;
		}

		public override bool Execute (List<Statement> statements, List<Term> terms, List<Term> extras, string goal)
		{
			result = true;
			count = 1;
			RecursiveResolve (statements, terms, 0, goal);
			return result;
		}

		private void RecursiveResolve (List<Statement> statements, List<Term> terms, int n, string goal)
		{
			if (n < terms.Count)
			{
				terms [n].Value = true;
				foreach (Statement s in statements)
				{
					s.ResolveTT ();
				}
				RecursiveResolve (statements, terms, n + 1, goal);
				terms [n].Value = false;
				foreach (Statement s in statements)
				{
					s.ResolveTT ();
				}
				RecursiveResolve (statements, terms, n + 1, goal);	
			}
			else
			{
				bool premises = true;
				bool goalstate = true;
				foreach (Statement s in statements)
				{
					if (s.Implied.Contains (terms.Find (p => p.Name == goal)))
					{
						goalstate = s.getTTValue;
					}
					else if (s.getTTValue)
					{
						premises = false;
					}
				}
				if (output)
				{
					Output (statements, terms, goal);
				}
				count++;
				if ((premises) && (!(goalstate)))
				{
					result = false;
				}
			}

		}

		private void Output (List<Statement> statements, List<Term> terms, string goal)
		{
			Console.Write (count + ":" + "\t");
			foreach (Statement s in statements)
			{
				for (int i = 0; i < s.Implies.Count ; i++)
				{
					Console.Write (s.Implies[i].Name);
					if (i < s.Implies.Count - 1)
					{
						Console.Write ("&");
					}
				}
				Console.Write ("=>");
				for (int i = 0; i < s.Implied.Count ; i++)
				{
					Console.Write (s.Implied[i].Name);
					if (i < s.Implied.Count - 1)
					{
						Console.Write ("&");
					}
				}
				Console.Write (" : " + s.getTTValue + " | ");
			}
			Console.WriteLine ();
			Console.Write ("\t");
			foreach (Term t in terms)
			{
				Console.Write (t.Name + " : " + t.Value + " | ");
			}
			Console.WriteLine ();
			Console.WriteLine ("---------------------------------------------------------------------------------------------------------------------------------------------------");
		}

		private void ConstructKB (List<Statement> statements, List<Term> terms, Term goal)
		{
			/*
			foreach (Statement s in statements)
			{
				if (s.Implied.Contains (terms.Find (p => p.Name == goal)))
				{
					KB++;
				}
			}
			*/
		}

		private void AddExtrasToKB (List<Term> extras)
		{
			
		}

		public override string GetKBString ()
		{
			return "";
		}

		public bool SetOutput {
			set
			{
				output = value;
			}
		}
	}
}

