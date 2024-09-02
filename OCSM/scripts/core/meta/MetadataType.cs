using System.Text.Json.Serialization;

namespace Ocsm.Meta;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum MetadataType
{
	None,
	
	CofdChangelingContractRegalia,
	CofdChangelingContractType,
	CofdChangelingCourt,
	CofdChangelingKith,
	CofdChangelingRegalia,
	CofdChangelingSeeming,
	
	Dnd5eBackground,
	Dnd5eClass,
	Dnd5eSpecies,
}
