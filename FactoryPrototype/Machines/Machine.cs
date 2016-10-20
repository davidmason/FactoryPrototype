using System;

namespace FactoryPrototype
{

	/// <summary>
	/// Each of the possible input and output port locations.
	/// 
	/// Machines may have any combination of these, and tiles
	/// will accept dropped items from any of them.
	/// </summary>
	public enum Port {
		UpperNorth, UpperEast, UpperSouth, UpperWest, UpperCenter,
		LowerNorth, LowerEast, LowerSouth, LowerWest, LowerCenter
	}

	/// <summary>
	/// Helper methods for dealing with ports.
	/// </summary>
	public class Ports {

		static Port[] allPorts = (Port[])Enum.GetValues(typeof(Port));

		public static Port[] All {
			get {
				return allPorts;
			}
		}

		// could optimize by just adding 5 to values under 5.
		public static Port Lower(Port port) {
			switch (port) {
			case Port.UpperNorth:
				return Port.LowerNorth;
			case Port.UpperEast:
				return Port.LowerEast;
			case Port.UpperSouth:
				return Port.LowerSouth;
			case Port.UpperWest:
				return Port.LowerWest;
			default:
				// must already be a lower
				return port;
			}
		}

		public static Port Complementary(Port port) {
			switch (port) {
			case Port.UpperNorth:
				return Port.UpperSouth;
			case Port.LowerNorth:
				return Port.LowerSouth;
			case Port.UpperEast:
				return Port.UpperWest;
			case Port.LowerEast:
				return Port.LowerWest;
			case Port.UpperSouth:
				return Port.UpperNorth;
			case Port.LowerSouth:
				return Port.LowerNorth;
			case Port.UpperWest:
				return Port.UpperEast;
			case Port.LowerWest:
				return Port.LowerEast;
			default:
				throw new Exception ("David wrote Ports.Complementary wrong, no complementary port for " + port);
			}
		}
	}

	public interface Machine
	{
		Item[] inputs { get; }
		Item[] outputs { get; }

		void Update ();
	}
}

