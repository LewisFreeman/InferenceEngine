using System;
using System.Collections.Generic;
using System.Linq;

namespace InferenceEngine
{
	public class Statement
	{
		private List<Term> implies;
		private List<Term> implied;

		public Statement (string statement, List<Term> terms)
		{
			implies = new List<Term> ();
			implied = new List<Term> ();
			Create (statement, terms);
		}

		private void Create (string statement, List<Term> terms)
		{
			List<string> Parts = new List<string> ();
			Parts = (statement.Split (new String [] { "=>" }, StringSplitOptions.None)).ToList();
			foreach (string str in Parts)
			{
				if (str.Contains ('&'))
				{
					string[] seperate = new string[(str.Count (x => x == '&'))* 2 - 1];
					seperate = str.Split(new char[] { '&' });
					for (int i = 0; i < seperate.Length; i++)
					{
						if (!(terms.Exists(x => x.Name == seperate[i])))
						{
							terms.Add(new Term(seperate[i]));
						}
						if (Parts.IndexOf (str) == 0)
						{
							implies.Add (terms.Find (p => p.Name == seperate[i]));
						}
						else
						{
							implied.Add (terms.Find (p => p.Name == seperate[i]));
						}
					}
				}
				else
				{
					if (!(terms.Exists(x => x.Name == str)))
					{
						terms.Add(new Term(str));
					}
					if (Parts.IndexOf (str) == 0)
					{
						implies.Add (terms.Find (p => p.Name == str));
					}
					else
					{
						implied.Add (terms.Find (p => p.Name == str));
					}
				}
			}
		}

		public void ResolveForward ()
		{
			if (IsAllTrue (implies))
			{
				SetAllTrue (implied);
			}
		}

		public void ResolveBackward ()
		{
			if (IsAllTrue (implied))
			{
				SetAllTrue (implies);
			}
		}

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

		private void SetAllTrue (List<Term> Terms)
		{
			foreach (Term t in Terms)
			{
				t.Value = true;
			}
		}

		public List<Term> Implies
		{
			get
			{
				return implies;
			}
		}

		public List<Term> Implied
		{
			get
			{
				return implied;
			}
		}

		public String GetString ()
		{
			string result = "";
			for (int i = 0; i < implies.Count; i++)
			{
				result += implies[i].Name;
				if (!(i == implies.Count - 1))
				{
					result += "&";
				}
			}
			result += "=>";
			for (int i = 0; i < implied.Count; i++)
			{
				result += implied[i].Name;
				if (!(i == implied.Count - 1))
				{
					result += "&";
				}
			}
			return result;
		}
	}
}

