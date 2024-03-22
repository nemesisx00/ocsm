namespace Ocsm.Meta;

public interface IMetadataContainer
{
	void Deserialize(string json);
	bool IsEmpty();
	string Serialize();
}
