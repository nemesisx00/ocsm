using System;

namespace Ocsm.Meta;

/**
<summary>
An instance of metadata which also contains an integer value.
</summary>
*/
public class MetadataNumber() : Metadata(), IComparable<MetadataNumber>, IEquatable<MetadataNumber>
{
	public int Value { get; set; }
	
	public int CompareTo(MetadataNumber other)
	{
		var ret = base.CompareTo(other);
		
		if(ret == 0)
			ret = Value.CompareTo(other?.Value);
		
		return ret;
	}
	
	public bool Equals(MetadataNumber metadataNumber) => base.Equals(metadataNumber)
		&& Value == metadataNumber?.Value;
	
	public override bool Equals(object obj) => Equals(obj as MetadataNumber);
	public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), Value);
}
