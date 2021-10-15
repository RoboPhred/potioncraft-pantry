# Pantry

A mod for creating custom PotionCraft ingredients.

This mod is intended to be used as a base for other mods that wish to add their own ingredients to the game.

## Adding custom ingredients

Custom ingredients are read in from the `PotionCraft/pantry` folder. This folder should contain other folders provided by other ingredient mods.

Each ingredient mod folder should contain a `package.yml` file defining the ingredients, as well as any desired artwork referenced by `package.yml`.

For an example of working ingredients, see the included `glummin0us` ingredient pack.

To make an ingredient mod, have your mod create a folder inside the `pantry` folder at the root of the PotionCraft install. In this folder, include a `package.yml` yaml-formatted file. This file should have a top level `ingredients` key, containing a list of the ingredients you want to define. Each ingredient supports the following keys:

- `name`: The name of your ingredient to display
- `description`: The description of the ingredient in the tooltip
- `inventoryImage`: The path (relative to this file) of the png image to use for the ingredient. Image should be 120x120 px with a transparent background
- `recipeImage`: The path (relative to this file) of the png image to use in the recipe list for saved recipes. Image should be 70x70px with a transparent background and the actual image should take up about half the image size
- `ingredientBase`: The internal name of the ingredient you want to copy the grindable substance and inventory list artwork from.
- `grindStartPercent`: The percentage along the path of where your normal path ends and the grindable portion of the path begins. Must be greater or equal to 0, and less than 1
- `isCrystal`: If set to true, the ingredient will act as a crystal and move with the teleportation effect.
- `path`: The SVG Path for the path the ingredient takes.
- `soldBy`: A list of instructions indicating who should sell this ingredient
- - `npcClass`: The npc type that sells this ingredient. Either: Herbalist, DwarfMiner, Mushroomer, or Alchemist.
- - `chanceToAppearPercent`: The chance for this item to be stocked by the npc per visit. Value should be between 0 (never) and 1 (100%)
- - `minCount`: If the item appears, the minimum number of the item to stock. The actual number will be randomly chosen between this and `maxCount`
- - `maxCount`: If the item appears, the maximum number of the item to stock. The actual number will be randomly chosen between this and `minCount`

### Defining ingredient paths

Ingredient paths are defined using a subset of the [SVG Path](https://developer.mozilla.org/en-US/docs/Web/SVG/Tutorial/Paths) standard.
SVG Paths use letters and numbers to define lines and curves. The in-game potion will follow the path defined by this svg path.
When making SVG paths for this mod, some limitations apply:
All coordinates and commands must use their absolute coordinate form (upper case commands).
The path must start from 0,0 and must NOT include a move command at the start.
Only the following commands are supported: `H` (horizontal line), `V` (vertical line), `L` (line), `C` (cubic bezier curve).

## Using custom ingredients

At the moment, custom ingredients are not added to any spawn lists, so the garden and traders will not use them.

While eventually, this mod intends to support adding ingredients to merchants, for now a debug console should be used to spawn the ingredients.

## Development

Dependencies are placed on the `/externals` folder. See the csproj file for required files.
