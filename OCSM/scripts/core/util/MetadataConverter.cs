using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Godot;

namespace Ocsm.Meta;

public class MetadataConverter : JsonConverter<Metadata>
{
	public static class PropertyNames
	{
		public const string Description = "Description";
		public const string Icon = "Icon";
		public const string Name = "Name";
		public const string Type = "Type";
	}
	
	public override Metadata Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var description = reader.GetString();
		var icon = JsonSerializer.Deserialize<Texture2D>(reader.GetString());
		var name = reader.GetString();
		var type = (MetadataType)reader.GetInt32();
		
		return new()
		{
			Description = description,
			Icon = icon,
			Name = name,
			Type = type,
		};
	}
	
	public override void Write(Utf8JsonWriter writer, Metadata value, JsonSerializerOptions options)
	{
		if(value is not null)
		{
			writer.WriteStartObject();
			
			writer.WritePropertyName(PropertyNames.Description);
			writer.WriteStringValue(value.Description);
			
			writer.WritePropertyName(PropertyNames.Icon);
			writer.WriteStringValue(JsonSerializer.Serialize(value.Icon));
			
			writer.WritePropertyName(PropertyNames.Name);
			writer.WriteStringValue(value.Name);
			
			writer.WritePropertyName(PropertyNames.Type);
			writer.WriteNumberValue((int)value.Type);
			
			writer.WriteEndObject();
		}
	}
}
