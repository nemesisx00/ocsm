using Godot;
using System.Text.Json;
using OCSM;

public interface ICharacterSheet
{
	string GetJsonData();
	void SetJsonData(string json);
}

public abstract class CharacterSheet<T> : Container, ICharacterSheet
	where T: Character
{
	protected virtual T SheetData { get; set; }
	
	public string GetJsonData() { return JsonSerializer.Serialize(SheetData); }
	public void SetJsonData(string json)
	{
		var data = JsonSerializer.Deserialize<T>(json);
		if(data is T typedData)
			SheetData = typedData;
	}
}
