using System;

namespace Ocsm;

[Flags]
public enum GameSystem
{
	None = 0,
	CofdChangeling = 1,
	CofdMortal = 2,
	Dnd5e = 4,
}

public enum MetadataType
{
	None = 0,
	
	CofdChangelingContractRegalia = 10,
	CofdChangelingContractType,
	CofdChangelingCourt,
	CofdChangelingKith,
	CofdChangelingRegalia,
	CofdChangelingSeeming,
}
