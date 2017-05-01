using System;

namespace InferenceEngine
{
	public class Term
	{
		private string _name;
		private bool _value;

		public Term (string Name)
		{
			_name = Name;
			_value = false;
		}

		public string Name
		{
			get
			{
				return _name;
			}
		}

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

