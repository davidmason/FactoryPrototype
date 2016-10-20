using System;

namespace FactoryPrototype
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Console.WriteLine ("Hello Factory!\n");

			Map map = new Map ();

			GameManager gm = new GameManager (map);

			// TODO add a machine at some position
			//      probably want an array of machines, in the map?

			EggBoiler boiler = new EggBoiler ();
			EggPeeler peeler = new EggPeeler ();
			gm.machines [4, 2] = boiler;
			gm.machines [4, 3] = peeler;

			gm.Update ();


			Console.WriteLine ("Insert 3 eggs into the boiler (with updates after each).");

			boiler.inputs [(int)Port.UpperNorth] = new Egg ();
			gm.Update ();
			boiler.inputs [(int)Port.UpperNorth] = new Egg ();
			gm.Update ();
			boiler.inputs [(int)Port.UpperNorth] = new Egg ();
			gm.Update ();

			Console.WriteLine ("Running simulation for a while...");

			gm.Update ();
			map.Print ();

			gm.Update ();
			map.Print ();

			gm.Update ();
			map.Print ();

			gm.Update ();
			map.Print ();

			gm.Update ();
			map.Print ();



			Console.WriteLine ("Mess on floor at [4,4]:");
			foreach (Item item in map.Tiles [4, 4].items) {
				Console.Write (item.ToString () + ", ");
			}
			Console.WriteLine ();

			Console.WriteLine ("Mess on floor at [5,3]:");
			foreach (Item item in map.Tiles [5, 3].items) {
				Console.Write (item.ToString () + ", ");
			}
			Console.WriteLine ();

			Console.ReadLine ();
		}
	}
}
