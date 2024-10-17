using System;
using System.Text.Json.Serialization;

namespace Ocsm.Wod.VtmV5;

[Flags]
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum BloodResonance
{
	None = 0,
	Animal = 1 << 0,
	Choleric = 1 << 1,
	Melancholy = 1 << 2,
	Phlegmetic = 1 << 3,
	Sanguine = 1 << 4,
}
