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

			EggBoiler boiler1 = new EggBoiler ();
			EggBoiler boiler2 = new EggBoiler ();
			gm.machines [3, 2] = boiler1;
			gm.machines [2, 4] = boiler2;

			gm.Update ();


			Console.WriteLine ("Insert first and second egg into each machine.");

			boiler1.inputs [(int)Port.UpperNorth] = new Egg ();
			boiler2.inputs [(int)Port.UpperNorth] = new Egg ();

			gm.Update ();

			boiler1.inputs [(int)Port.UpperNorth] = new Egg ();
			boiler2.inputs [(int)Port.UpperNorth] = new Egg ();

			gm.Update ();

			Console.WriteLine ("Attempt to insert third egg in each machine. Expect to drop them.");

			boiler1.inputs [(int)Port.UpperNorth] = new Egg ();
			boiler2.inputs [(int)Port.UpperNorth] = new Egg ();

			gm.Update ();

			boiler1.inputs [(int)Port.UpperNorth] = new Egg ();
			boiler2.inputs [(int)Port.UpperNorth] = new Egg ();

			gm.Update ();


//			for (int i = 0; i < 4; i++) {
//				Console.WriteLine ("Inserting an egg");
//				boiler.Input (0, new Egg ());
//			}
//
//			for (int i = 0; i < 10; i++) {
//				boiler2.Input (0, new Egg ());
//			}

			Console.WriteLine ();
			map.Print ();

			gm.Update ();

			Console.WriteLine ();
			map.Print ();
			Console.ReadLine ();
		}
	}
}
