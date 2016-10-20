using System;

namespace FactoryPrototype
{

	/// <summary>
	/// Handles a collection of map tiles.
	/// </summary>
	public class Map
	{
		public const int DEFAULT_WIDTH = 7;
		public const int DEFAULT_HEIGHT = 7;

		private Tile[,] tiles;

		public Tile[,] Tiles {
			get {
				return tiles;
			}
		}

		public Map ()
		{
			tiles = new Tile[DEFAULT_WIDTH,DEFAULT_HEIGHT];

			// just a simple 5*5 room with a wall around the outside
			// and a drain in the middle
			for (int i = 0; i < DEFAULT_WIDTH; i++) {
				for (int j = 0; j < DEFAULT_HEIGHT; j++) {
					if (i == 3 && j == 3) {
						tiles [i, j] = Tile.NewDrain ();
					} else if (i == 0 || j == 0 || i == DEFAULT_WIDTH - 1 || j == DEFAULT_HEIGHT - 1) {
						tiles [i,j] = Tile.NewWall();
					} else {
						tiles [i,j] = Tile.NewFloor();
					}
				}
			}
		}

		/// <summary>
		/// Print the map to the console.
		/// </summary>
		public void Print() {
			for (int i = 0; i < DEFAULT_WIDTH; i++) {
				for (int j = 0; j < DEFAULT_HEIGHT; j++) {
					string representation;
					Tile tile = tiles [i, j];
					// TODO extract this to a static method
					switch (tile.type) {

					case TileType.Drain:
						representation = "#";
						break;

					case TileType.Floor:
						if (tile.items.Count == 0) {
							representation = "";
						} else {
							representation = tile.items.Count.ToString ();
						}
						break;

					case TileType.Wall:
						representation = "+";
						break;

					default:
						// unknown type
						representation = "?";
						break;
					}
					Console.Write(representation.PadLeft(2, ' '));
				}
				Console.WriteLine ();
			}
		}
	}
}

