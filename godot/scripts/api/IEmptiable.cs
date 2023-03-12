
namespace OCSM.API
{
	/// An interface defining a means of determining if this object is empty.
	public interface IEmptiable
	{
		bool Empty { get { return false; } }
	}
}
