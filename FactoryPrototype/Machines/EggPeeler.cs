using System;
using System.Collections.Generic;
using System.Linq;

namespace FactoryPrototype
{
	/**
	 * A machine that peels hard-boiled eggs.
	 * 
	 * It will try to peel anything you give it though, but there's a
	 * good chance of failure.
	 */
	public class EggPeeler : Machine
	{
		// TODO standard way to specify the input ports (type, location on machine)
		// TODO standard way to receive an input (which may be dropped if machine is overloaded)
		// TODO standard way to specify output?

		// FIXME move this to abstract parent implementation, input and output ports
		//       per tile will always be the same.
		//       (may be different if a machine is 2 tiles, stick to 1 for simplicity).
//		Item[] _inputs = new Item[Enum.GetValues (typeof(Port)).Cast<int>().Max()];
		Item[] _inputs = new Item[10];
		Item[] _outputs = new Item[10];

		public Item[] inputs {
			get {
				return _inputs;
			}
		}

		public Item[] outputs {
			get {
				return _outputs;
			}
		}


		// Hopper that just accumulates some items ready to process.
		private Queue<Item> inputHopper = new Queue<Item> ();
		private const int INPUT_HOPPER_CAPACITY = 2;


		public EggPeeler ()
		{
		}

		/// <summary>
		/// Update loop.
		/// 
		/// post: all inputs must be removed
		/// </summary>
		public void Update () {
			Console.WriteLine ("EggPeeler.Update()");


			if (inputHopper.Count > 0) {
				Item itemFromHopper = inputHopper.Dequeue ();
				if (itemFromHopper.GetType () == typeof(BoiledEgg)) {
					Console.WriteLine ("Peeled an egg.");
					outputs [(int)Port.UpperSouth] = new PeeledEgg ();
					outputs [(int)Port.UpperEast] = new EggShell ();
				} else if (itemFromHopper.GetType () == typeof(Egg)) {
					Console.Error.WriteLine ("EggPeeler tried to peel " + itemFromHopper.ToString () + " and it did not go so well.");
					outputs [(int)Port.UpperEast] = new EggShell ();
					// TODO drop egg liquid on ground
				} else {
					Console.Error.WriteLine ("EggPeeler can't peel " + itemFromHopper.ToString () + ". Dumping on ground.");
					outputs [(int)Port.LowerCenter] = itemFromHopper;
				}
			}




			// iterate inputs, move mismatched things to output
			// (the dropping and filling the hopper can be abstracted to parent class)
			foreach (Port port in Ports.All) {
				int portNumber = (int)port;

				Item item = inputs [portNumber];
				inputs [portNumber] = null;

				if (item != null) {
					switch (port) {
					case Port.UpperNorth:
						if (inputHopper.Count < INPUT_HOPPER_CAPACITY) {
							inputHopper.Enqueue (item);
						} else {
							Console.WriteLine (item.ToString () + " doesn't fit. Dropped on the floor.");
							// send item to matching lower output port (just bounces off the machine and falls)
							outputs[(int)Ports.Lower(port)] = item;
						}
						break;


					default:
						int lowerPortNumber = (int)Ports.Lower (port);
						if (outputs [lowerPortNumber] == null) {
							// Bounce it off the machine since it is not a valid input.
							outputs [lowerPortNumber] = item;						
						} else {
							// There is something in the way of bouncing this off, so
							// just leave it in the input until that thing is cleared.
							// This is to avoid clobbering an item since I have only
							// allowed one item per in/out port so far.
							inputs[portNumber] = item;
						}
						break;
					}
				}
			}

			// TODO cook an egg, or continue the cooking process.
			//      first just get the inputs hopper executing correctly before doing this.
		}
	}
}

