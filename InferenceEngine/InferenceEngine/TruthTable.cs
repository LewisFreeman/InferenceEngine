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

		public TruthTable (bool Output)
		{
			states = new List<bool> ();
			output = Output;
		}

		public override bool Execute (List<Statement> statements, List<Term> terms, List<Term> extras, string goal)
		{
			bool check = false;
			foreach (Statement s in statements) 
			{
				if (s.Implied.Contains (terms.Find (p => p.Name == goal)))
				{
					check = true;
				}
			}
			if (extras.Contains (terms.Find (p => p.Name == goal)))
			{
				check = true;
			}
			if (check)
			{
				models = 0;
				count = 1;
				RecursiveResolve (statements, terms, extras, 0, goal);
				if (models > 0)
				{
					return true;
				}
			}
			return false;
		}

		private void RecursiveResolve (List<Statement> statements, List<Term> terms, List<Term> extras, int n, string goal)
		{
			if (n < terms.Count)
			{
				terms [n].Value = true;
				foreach (Statement s in statements)
				{
					s.ResolveTT ();
				}
				RecursiveResolve (statements, terms, extras, n + 1, goal);
				terms [n].Value = false;
				foreach (Statement s in statements)
				{
					s.ResolveTT ();
				}
				RecursiveResolve (statements, terms, extras, n + 1, goal);	
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
				foreach (Term t in extras)
				{
					if (!(t.Value))
					{
						premises = false;
					}
				}
				if (output)
				{
					Output (statements, terms, goal);
				}
				count++;
				if ((premises) && (terms.Find (p => p.Name == goal).Value))
				{
					models++;
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

		public override string GetKBString ()
		{
			return models.ToString();
		}

		public bool SetOutput {
			set
			{
				output = value;
			}
		}
	}
}

