using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Godot;

namespace Ocsm.Meta;

public class Texture2DConverter : JsonConverter<Texture2D>
{
	public override Texture2D Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		Texture2D value = null;
		
		var resourcePath = reader.GetString();
		if(!string.IsNullOrEmpty(resourcePath))
			value = GD.Load<Texture2D>(resourcePath);
		
		return value;
	}
	
	public override void Write(Utf8JsonWriter writer, Texture2D value, JsonSerializerOptions options)
	{
		if(value is not null)
			writer.WriteStringValue(value.ResourcePath);
	}
}
