using System;
using System.Windows.Threading;

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
			map.Machines [4, 2] = boiler;
			map.Machines [4, 3] = peeler;

			gm.Update ();


			Console.WriteLine ("Insert 3 eggs into the boiler (with updates after each).");

			// insert 10 eggs
			for (int i = 0; i < 10; i++) {
				boiler.inputs [(int)Port.UpperNorth] = new Egg ();
				gm.Update ();
				map.Print ();
			}

			Console.WriteLine ("Running simulation for a while...");

			for (int i = 0; i < 10; i++) {
				gm.Update ();
				Console.Clear ();
				map.Print ();
			}

			Console.WriteLine ();
			map.PrintLegend ();
			Console.WriteLine ();

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


		public static void DelayAction(int millisecond, Action action)
		{
			var timer = new DispatcherTimer();
			timer.Tick += delegate

			{
				action.Invoke();
				timer.Stop();
			};

			timer.Interval = TimeSpan.FromMilliseconds(millisecond);
			timer.Start();
		}
	}
}
