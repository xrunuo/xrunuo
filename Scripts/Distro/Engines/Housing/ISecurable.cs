using Server.Engines.Housing.Multis;

namespace Server.Engines.Housing
{
	public interface ISecurable
	{
		SecureLevel Level { get; set; }
	}
}