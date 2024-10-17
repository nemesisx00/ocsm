using System;
using System.Text.Json.Serialization;

namespace Ocsm.Meta;

[Flags]
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum MetadataType
{
	None = 0,
	
	CofdChangelingContractRegalia = 1 << 0,
	CofdChangelingContractType = 1 << 1,
	CofdChangelingCourt = 1 << 2,
	CofdChangelingKith = 1 << 3,
	CofdChangelingRegalia = 1 << 4,
	CofdChangelingSeeming = 1 << 5,
	
	Dnd5eBackground = 1 << 6,
	Dnd5eClass = 1 << 7,
	Dnd5eSpecies = 1 << 8,
	
	WodVtmV5Advantage = 1 << 9,
	WodVtmV5Flaw = 1 << 10,
	WodVtmV5PredatorType = 1 << 11,
}
