using System;
using System.Text.Json.Serialization;

namespace Ocsm.Meta;

[Flags]
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum MetadataType
{
	None = 0,
	
	CofdChangelingContractRegalia = 1,
	CofdChangelingContractType = 2,
	CofdChangelingCourt = 4,
	CofdChangelingKith = 8,
	CofdChangelingRegalia = 16,
	CofdChangelingSeeming = 32,
	
	Dnd5eBackground = 64,
	Dnd5eClass = 128,
	Dnd5eSpecies = 256,
}
