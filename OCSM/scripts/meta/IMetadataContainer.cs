namespace Ocsm.Meta;

public interface IMetadataContainer
{
	void InitializeWithDefaultValues();
	void Deserialize(string json);
	bool IsEmpty();
	string Serialize();
}
