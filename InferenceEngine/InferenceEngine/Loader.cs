using System;
using System.Collections.Generic;
using System.IO;

namespace InferenceEngine
{
	public class Loader
	{
		List<string> lines;
		List<string> Statements;
		string Goal;

		/// <summary>
		/// Class to load in textfiles
		/// </summary>
		/// <param name="_filename">The name of the file to be loaded</param>
		public Loader (string _fileName)
		{
			//Initialize the lists
			lines = new List<string> ();
			Statements = new List<string> ();

			//Read the file
			Read (_fileName);

			//Deformat the values
			Deformat ();
		}
			
		/// <summary>
		/// The method to read the text of the file and store it in the lines list
		/// </summary>
		/// <param name="_fileName">the name of the file to be read</param>
		private void Read (string _fileName)
		{
			//Try/catch statement to catch errors arising if the file cannot be found
			try
			{
				using (StreamReader sr = File.OpenText(_fileName))
				{
					//Initialize a string to store a single line from the file
					string line;
					//Reads a line from the file and stores it in the lines list, continues until the line read is null (EoF)
					while ((line = sr.ReadLine()) != null)
					{
						lines.Add (line);
					}
				}
			}
			catch
			{
				//Catch for when the file is not found, outputs an error and exits the program
				Console.WriteLine ("-------------------------------------");
				Console.WriteLine ("The program cannot find the text file");
				Console.WriteLine ("-------------------------------------");
				System.Environment.Exit (0);
			}
		}

		/// <summary>
		/// The method to convert the raw data of the lines list into formatted values in the statments list
		/// </summary>
		private void Deformat ()
		{
			//Split the string via the ";" seperator into an array
			string[] parts = lines [1].Split (';');

			//Foreach value in the array of split strings
			foreach (string s in parts)
			{
				//Add the string to the statements list, removing whitespaces
				Statements.Add(s.Replace (" ", string.Empty));
			}

			//Remove an empty statement added to the end by the method
			Statements.Remove (Statements[Statements.Count - 1]);

			//Set the goal to the value of line 3
			Goal = lines [3];
		}

		/// <summary>
		/// A readonly property to get the statements list
		/// </summary>
		/// <returns>List<String></returns>
		public List<string> GetStatements
		{
			get
			{
				return Statements;
			}
		}

		/// <summary>
		/// A readonly property to get the goal value string
		/// </summary>
		/// <returns>string</returns>
		public string GetGoal
		{
			get
			{
				return Goal;
			}
		}
	}
}

