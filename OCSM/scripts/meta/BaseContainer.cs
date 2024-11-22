using System.Collections.Generic;

namespace Ocsm.Meta;

public abstract class BaseContainer() : IMetadataContainer
{
	public List<Metadata> Metadata { get; set; } = [];
	
	public abstract void Deserialize(string json);
	public virtual void InitializeWithDefaultValues() {}
	public virtual bool IsEmpty() => Metadata.Count == 0;
	public abstract string Serialize();
}
