namespace Ocsm.Nodes;

public interface ICharacterSheet
{
	string CharacterName { get; }
	string GetJsonData();
	void SetJsonData(string json);
}
