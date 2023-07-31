
namespace Ocsm.Cofd;

public class Mortal : CodCore
{
	public long Age { get; set; }
	
	public Mortal() : base(Constants.GameSystem.Cofd.Mortal)
	{
		Age = -1;
	}
}
