﻿using System;

namespace FactoryPrototype
{

	/// <summary>
	/// Handles a collection of map tiles.
	/// </summary>
	public class Map
	{
		public const int DEFAULT_WIDTH = 7;
		public const int DEFAULT_HEIGHT = 7;

		Tile[,] tiles;
		Machine[,] machines;

		// Dimensions are [x,y].
		public Tile[,] Tiles {
			get {
				return tiles;
			}
		}

		public Machine[,] Machines {
			get {
				return machines;
			}
		}

		public Map ()
		{
			tiles = new Tile[DEFAULT_WIDTH,DEFAULT_HEIGHT];
			machines = new Machine[DEFAULT_WIDTH,DEFAULT_HEIGHT];

			// just a simple 5*5 room with a wall around the outside
			// and a drain in the middle
			for (int x = 0; x < DEFAULT_WIDTH; x++) {
				for (int y = 0; y < DEFAULT_HEIGHT; y++) {
					if (x == 3 && y == 3) {
						tiles [x, y] = Tile.NewDrain ();
					} else if (x == 0 || y == 0 || x == DEFAULT_WIDTH - 1 || y == DEFAULT_HEIGHT - 1) {
						tiles [x,y] = Tile.NewWall();
					} else {
						tiles [x,y] = Tile.NewFloor();
					}
				}
			}
		}

		// in case I want to use them on the basic map print:
		const string symbols = "❶❷❸❹❺❻❼❽❾❿➀➁➂➃➄➅➆➇➈➉➱⇐⇑⇒⇓";

		public void PrintLegend() {
			Console.WriteLine ("☖  Machine");
			Console.WriteLine ("+  Wall");
			Console.WriteLine ("#  Drain");
			Console.WriteLine ("1,2,3,etc.  How many items on floor.");
			Console.WriteLine ("?  Unknown tile type (error in program).");
		}

		/// <summary>
		/// Print the map to the console.
		/// </summary>
		public void Print() {
			for (int y = 0; y < DEFAULT_HEIGHT; y++) {
				for (int x = 0; x < DEFAULT_WIDTH; x++) {
					string representation = "";

					Machine machine = Machines [x, y];
					if (machine != null) {
						representation += "☖";
					}

					Tile tile = tiles [x, y];
					// TODO extract this to a static method
					switch (tile.type) {

					case TileType.Drain:
						representation += "#";
						break;

					case TileType.Floor:
						if (tile.items.Count == 0) {
							representation += "";
						} else {
							representation = tile.items.Count.ToString ();
						}
						break;

					case TileType.Wall:
						representation += "+";
						break;

					default:
						// unknown type
						representation += "?";
						break;
					}
					Console.Write(representation.PadLeft(2, ' '));
				}
				Console.WriteLine ();
			}
		}
	}
}

