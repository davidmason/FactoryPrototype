
- use an enum for map tile types, rather than typeof
  - make a generic tile class that can have a type and hold items and fluid levels
  - control tile behaviour from top-down

- make map stateless (to avoid cascade effects such as fluid being able to flow across
  the whole map in a single pass).

- handle passing items through game manager

    This is so that machines don't have to be aware of their
    position, they can just indicate to the game manager that
    they are dropping something on one of their tiles, or passing
    something towards a particular tile. The manager component
    can handle the passing of the item to whatever is on the
    adjacent square, which may be a floor.

    The map tiles could use their own manager to determine
    where fluids will flow and where items will tumble.
    - Fluids would only flow when the amount is e.g. > 1 for a tile (surface tension)
    - Fluid amounts over 1 will flow at some rate (e.g. 0.1/s) to each abutting tile
      as long as it is not blocked by a wall or by having a higher fluid level.
    - Items act a bit different, they will just tumble if the heap level is greater
      by a certain amount. e.g. if the amount is 5, it could act like this:

      5   3 8 2
      _ _ _ _ _      Left, middle and right columns are stable because only 5 or less items on them.
                     4th column won't fall left because it is only 5 higher, so stable that way.
          |          4th column has 1 item tumble right because it is 6 higher.
          v            If something could fall multiple ways, simplest just to pick a random one.
                       Could collapse just 1 level per tick, or collapse as much as possible per tick. *Decision needed*
      5   3 7>3      It is stable now, the difference between all adjacent tiles is <= 5
      _ _ _ _ _


