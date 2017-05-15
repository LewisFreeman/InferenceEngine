using System;

namespace InferenceEngine
{
	public class Term
	{
		private string _name;
		private bool _value;

		/// <summary>
		/// Class to reprsent individual terms
		/// </summary>
		/// <param name="Name">The name of term</param>
		public Term (string Name)
		{
			_name = Name;

			//Default the value to false
			_value = false;
		}

		/// <summary>
		/// A readonly property to get the name of the term
		/// </summary>
		/// <returns>string</returns>
		public string Name
		{
			get
			{
				return _name;
			}
		}

		/// <summary>
		/// A property to get or set the value of the term
		/// </summary>
		/// <returns>bool</returns>
		public bool Value
		{
			get
			{
				return _value;
			}
			set
			{
				_value = value;
			}
		}
	}
}

