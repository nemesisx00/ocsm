using System;
using System.Text.Json.Serialization;
using OCSM.API;

namespace OCSM.CoD
{
	public struct Specialty : IEmptiable
	{
		public string Skill { get; set; }
		public string Value { get; set; }
		
		[JsonIgnore]
		public bool Empty { get { return String.IsNullOrEmpty(Skill) && String.IsNullOrEmpty(Value); } }
	}
}
