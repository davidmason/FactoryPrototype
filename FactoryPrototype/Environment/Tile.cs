using System;
using System.Collections.Generic;

namespace FactoryPrototype
{

	public enum TileType {
		// Impassable, nothing can flow or topple here
		Wall,
		Floor,
		// Like floor, but fluids drain away
		Drain
	}

	/// <summary>
	/// A tile on the map.
	/// 
	/// I expect this to be floor, wall or drain, but maybe other things.
	/// 
	/// Tiles will receive dropped items and spilled liquids. A manager will
	/// handle movement of items and liquids to adjacent tiles.
	/// </summary>
	public struct Tile
	{
		public static Tile NewFloor() {
			return new Tile (TileType.Floor);
		}

		public static Tile NewWall() {
			return new Tile (TileType.Wall);
		}

		public static Tile NewDrain() {
			return new Tile (TileType.Drain);
		}

		// This can change (e.g. player places a drain or wall).
		TileType _type;
		List<Item> _items;


		public Tile(TileType type) {
			_type = type;
			_items = new List<Item>();
		}

		public TileType type {
			get {
				return _type;
			}

			// Should only set from non-wall to wall if there are no items,
			// and maybe no liquids.
			set {
				_type = value;
			}
		}

		public List<Item> items {
			get {
				return _items;
			}
		}
	}
}

