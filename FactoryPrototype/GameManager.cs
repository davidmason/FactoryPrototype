using System;

namespace FactoryPrototype
{
	/// <summary>
	/// Responsible for setting everything up.
	/// 
	/// Initially responsible for machine placement, passing items around
	/// and fluids flowing, but these may be split out to other classes.
	/// </summary>
	public class GameManager
	{
		Map map;

		public GameManager (Map map)
		{
			this.map = map;
		}

	
		/// I figured out that machines should just have output fields that they
		/// put items and fluids into.
		/// The manager can then just do 1 pass and move every output to the
		/// appropriate input.
		/// 
		/// For a single tile, there are 10 possible outputs:
		///  - center, left, right, up, down
		///  - top and bottom for each
		/// Top inputs will go into machines, bottom inputs will be on the floor.
		/// So items dropped on the floor can just go to bottom input.

		/// Perform a single tick of update. Update cycles are not timed yet,
		/// but each should be something around 100-250ms.
		public void Update () {
			UpdateFluids ();
			UpdateSpilledItems ();
			UpdateOutputs ();
			UpdateMachines ();
		}


		/// <summary>
		/// A single pass of fluid flow on the map.
		/// </summary>
		void UpdateFluids ()
		{

			// NOTE: to collect fluid into drains effectively:
			// - give drains -5 fluid level
			// - drain-adjacent squares -2 fluid level
			// - drain-diagonal squares -1 fluid level

			// TODO calculate array of changes
			// TODO amount of flow should only be enough to make destination equal to source
			//      (so max half of difference)
			// TODO figure out how to stop multiple sources over-filling a tile?
			//         |2|1|2|  ->  |1.5|2|1.5|  ->  resonates back and forth for a bit...
			//      May actually be OK, but would make for some waves when there are large
			//      amounts of fluid dumped somewhere.
			//
			//      There is probably a calculation that can be done based on the 4 adjacent
			//      squares, to calculate how much fluid a square should "request" from its
			//      neighbours, and how much from each.
			//      Once all "request" amounts are known for a tile, it could dole them all
			//      out proportionally. So in this case:
			//
			//         |1|2|1|  requests on the middle tile are |0.5|-|0.5|
			//         There are 2 requests so divide them by 2: |0.25|-|0.25|
			// To give |1.25|1.5|1.25|

			// e.g. 2:  |5   |10|7   |
			//   diff:  |5   |- |3   |
			//    req:  |2.5 |- |1.5 |
			//    bal:  |1.25|- |.75 |
			// result:  |6.25|8 |7.75|

			//   Played with this in google sheets:
			//     https://docs.google.com/spreadsheets/d/1pYt2vU5hVzU8nXjxED4u9d2PdhcACrJvmhktQms2aRw/edit#gid=0
			//
			// Consider minecraft flow dynamics, that may be a good model.
			//  - Has maybe 7 levels.
			//  - Fluid flows out of each surface unless it is blocked by a same-or-higher level
			//  - Flowing fluid drops by 1 level for each square it flows to, even if it comes from multiple squares
			//  - That means it can drop to 0.
			// Hmmm, that doesn't really sound like what I want. I want it to:
			//  - flow fluid to all adjacent tiles that have less fluid
			//  - do not flow when fluid level is 1 or less (surface tension)
			//      1 means a thin layer of fluid. Even if you mop up the fluid next to this, it will just sit there
			//      on the tile rather than flow anywhere. 0.5 would be enough to make a puddle over half the area of
			//      a tile. 2 is enough to make a puddle that will cover 2 tiles.

			// Maybe consider working with "packets" of water that are size 0.5. That means a square with 2 would drop
			// 0.5 off in 2 random directions (or the same direction twice), then flow would stop.
			// This means it would work similar to items. Just start with a random side and check around the 4, as
			// soon as it finds one that is lower, lock in passing 0.5 fluid to that side.
			//
			// A square could start with 0 surrounded with 1.5 and end up as a 2 surrounded by 1 (water "sloshes" in there)
			// Next tick it would be 1.5 surrounded with 1,1,1,1.5
			// Next tick the couple of 0.5s would flow around to the adjacent squares - basically keep flowing around
			// until they settle on a lower point - constant motion in a filled-up area with no outflow.
			// It isn't quite ideal, but is fairly simple.

			// Items wouldn't have the problem because they would have to be +5 increaments rather than a single increment.

			// Could also do a rotating calculation, cycling every 4 ticks. First just calculate the upward flows, then
			// right, down, left.
			// - It avoids any problem of flow from multiple tiles that could overfill a tile.
			// - Tiles could equalize exactly rather than have to be damped.
			// - How dumped fluids spread out would depend on the timing.
			// - Drips would on average go in all directions, the surface tension limit should keep it from just spiralling even if the timing does make it drip only when flow is upwards.



			// TODO apply changes to map

			Console.Error.WriteLine ("TODO: implement GameManager.UpdateFluids()");

		}

		/// <summary>
		/// Simulates items toppling to adjacent tiles.
		/// 
		/// Items will topple when they are piled up too much higher than an adjacent tile.
		/// </summary>
		void UpdateSpilledItems ()
		{
			Console.Error.WriteLine ("TODO: implement GameManager.UpdateSpilledItems()");
		}

		/// <summary>
		/// A Single pass processing all the outputs of machines.
		/// 
		/// This means item being dropped to inputs or onto the
		/// floor, depending where the output is and what input
		/// is at that position.
		/// 
		/// Some machine inputs could come from several directions,
		/// like from back, left, right or center (i.e. flowing in
		/// from 3 sides, or dropped from above).
		/// 
		/// If there is no compatible machine input for an output,
		/// the item drops to the floor. Some outputs are onto
		/// the floor anyway (anything that outputs to the low
		/// outputs).
		/// </summary>
		void UpdateOutputs ()
		{
			// TODO single-pass input/output on each machine
			//       - pre: all inputs are empty
			//       - post: all outputs are empty


			// Can do the following by getting the inputs array for each position
			// (machine, or tile, depending what it is).

			// steps:

			// iterate each machine

			for (int i = 0; i < map.Machines.GetLength (0); i++) {
				for (int j = 0; j < map.Machines.GetLength (1); j++) {
					Machine machine = map.Machines [i, j];
					if (machine != null) {

						// iterate the outputs of that machine
						foreach (Port port in Ports.All) {
							int portNumber = (int)port;

							// take item out of the output
							Item output = machine.outputs [portNumber];
							machine.outputs [portNumber] = null;

							if (output != null) {
								Machine receiver = FindAdjacentMachine(i, j, port);

								if (receiver == null) {
									// put item on floor
									FindAdjacentTile(i, j, port).items.Add(output);

								} else {
									// put item in matching input of receiver
									receiver.inputs[(int)Ports.Complementary(port)] = output;

								}
							}
						}
					}
				}
			}
		}

		// TODO move to Map
		/// <summary>
		/// Find the adjacent machine, if there is one.
		/// </summary>
		/// 
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		/// <param name="port">The port that gives the direction to check.</param>
		///
		/// <returns>The adjacent machine, if one exists.</returns>
		Machine FindAdjacentMachine (int x, int y, Port port) {
			switch (port) {
			case Port.LowerNorth:
			case Port.UpperNorth:
				// get machine to the north
				return map.Machines[x, y-1];
			case Port.LowerEast:
			case Port.UpperEast:
				// get machine to the east
				return map.Machines[x+1,y];
			case Port.LowerSouth:
			case Port.UpperSouth:
				// get machine to the south
				return map.Machines[x, y+1];
			case Port.LowerWest:
			case Port.UpperWest:
				// get machine to the west
				return map.Machines[x-1,y];
			default:
				// Must be a central port.
				// Machines can't pass to themself so return nothing.
				// (it would make an infinite loop if they pass to themself).
				return null;
			}
		}

		// TODO move to Map
		Tile FindAdjacentTile (int x, int y, Port port) {
			switch (port) {
			case Port.LowerNorth:
			case Port.UpperNorth:
				// get machine to the north
				return map.Tiles[x, y-1];
			case Port.LowerEast:
			case Port.UpperEast:
				// get machine to the east
				return map.Tiles[x+1,y];
			case Port.LowerSouth:
			case Port.UpperSouth:
				// get machine to the south
				return map.Tiles[x, y+1];
			case Port.LowerWest:
			case Port.UpperWest:
				// get machine to the west
				return map.Tiles[x-1,y];
			default:
				// Must be a central port, so it is the same tile.
				return map.Tiles[x,y];
			}
		}

		/// <summary>
		/// Run Update() on each machine.
		/// 
		/// This should run the machine's internal process, which MUST leave all
		/// its item inputs empty (electricity and fluids will be different when
		/// they are added).
		/// 
		/// Items that don't fit can be immediately dropped by moving them to an
		/// appropriate low output. Something passed from the left to an already-full
		/// hopper could be put in lower left output (so it basically bounces off
		/// onto the floor).
		/// </summary>
		void UpdateMachines ()
		{
			// TODO single-pass of Update() on each machine

			for (int i = 0; i < map.Machines.GetLength (0); i++) {
				for (int j = 0; j < map.Machines.GetLength (1); j++) {
					Machine machine = map.Machines [i, j];
					if (machine != null) {
						machine.Update ();
					}
				}
			}
		}
	}
}

