
namespace OCSM.CoD
{
	public class Mortal : CodCore
	{
		public long Age { get; set; }
		
		public Mortal() : base(Constants.GameSystem.CoD.Mortal)
		{
			Age = -1;
		}
	}
}
