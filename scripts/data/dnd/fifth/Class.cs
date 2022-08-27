using System;

namespace OCSM.DnD.Fifth
{
	public sealed class Class : IEquatable<Class>
	{
		
		public int Level { get; set; }
		public string Name { get; set; }
		public HitDice HitDice { get; set; }
		
		public Class(string name, Die die)
		{
			Name = name;
			Level = 1;
			HitDice = new HitDice(die, Level);
		}
		
		public bool Equals(Class c)
		{
			return c.Name.Equals(Name);
		}
	}
}
