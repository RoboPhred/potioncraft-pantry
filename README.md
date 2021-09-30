# Pantry

A mod for creating custom PotionCraft ingredients.

## Adding custom ingredients

Custom ingredients are read in from the `PotionCraft/pantry` folder. This folder should contain other folders provided by other ingredient mods.

Each ingredient mod folder should contain a `package.yml` file defining the ingredients, as well as any desired artwork referenced by `package.yml`.

For an example of working ingredients, see the included `glummin0us` ingredient pack.

## Using custom ingredients

At the moment, custom ingredients are not added to any spawn lists, so the garden and traders will not use them.

While eventually, this mod intends to support adding ingredients to merchants, for now a debug console should be used to spawn the ingredients.

## Development

Dependencies are placed on the `/externals` folder. See the csproj file for required files.
