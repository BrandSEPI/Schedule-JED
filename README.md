# JustEnoughDrugs

[![MelonLoader](https://img.shields.io/badge/MelonLoader-0.7.0-brightgreen.svg?style=flat-square)](https://melonwiki.xyz/#/)
[![Schedule I](https://img.shields.io/badge/Schedule%20I-Mono-blue.svg?style=flat-square)]()
[![Version](https://img.shields.io/badge/Version-2.0.0-blueviolet.svg?style=flat-square)]()

A MelonLoader mod for **Schedule I** that enhances the drugs tab with powerful search, sorting, and detailed recipe features.

## Preview

![Mod Preview](https://i.imgur.com/lEf6ngF.png)

## Features

- **Search Bar**: Quickly filter drugs by typing a keyword.
- **Dropdown Filter**: Select whether to filter by **Name**, **Ingredients**, **Effects**, or search **Any** field.
- **Sorting Options**: Sort drugs by **Newest**, **Addictiveness**, **Cost**, **Price**, or **Profit**.
- **Real-Time Filtering and Sorting**: Updates instantly as you type or change filters.
- **Full Recipe Display**: See the complete chain of ingredients used to craft a product, visually.
- **Drug Cost Display**: View the estimated production cost of each drug.
- **Configurable Settings**: Easily enable or disable features through MelonPreferences.
- **Shortcuts Disabled**: While typing, game shortcuts are temporarily disabled (except Tab, Escape, and Right Click).
- **Zero Configuration**: Works instantly — just drop the file into the mod folder.

## Quick Start Guide

1. Install the mod by following the instructions below.
2. Launch **Schedule I** and open the drugs tab.
3. Use the search bar and dropdown to quickly find and sort products.
4. Click a drug to view its full ingredient recipe and estimated production cost.

Everything updates live as you type or interact with the interface.

## Why You Need This

Tired of scrolling through endless substances?  
**JustEnoughDrugs** saves you time and boosts your experience by:

- Letting you search drugs by **name**, **ingredients**, or **effects**.
- Instantly sorting drugs by **cost**, **profit**, **addictiveness**, and more.
- Showing detailed recipes and production costs at a glance.
- Creating a smoother, faster, and more informative way to manage drugs.

If you have suggestions for new features or improvements, feel free to open an issue!

## Installation

1. Install **MelonLoader** for **Schedule I** if you haven't already.
2. Download the latest version of **JustEnoughDrugs**.
3. Place `JustEnoughDrugs.dll` into your **Schedule I/Mods** folder.
4. Launch the game — the mod will be automatically loaded and ready to use.

## Configuration

JustEnoughDrugs automatically creates a configuration section in your `Schedule I/UserData/MelonPreferences.cfg` file.  
You can customize the mod behavior without needing to modify the code.

The config section looks like this:

```ini
[JustEnoughDrugs]
# Enables the sorting functionality for drugs in the product manager.
EnableDrugSorting = true
# Displays the full recursive recipe for products when selected.
ShowFullRecipe = true
```

## How It Works

When you open the drugs tab:

- **A search bar and dropdown filter** are added to the UI.
- Typing into the search bar filters the drug list **in real-time**.
- The dropdown menu lets you pick whether to filter by **Name**, **Ingredients**, **Effects**, or **Any**.
- Clicking on a drug reveals its **full ingredient recipe** and **cost**.
- Hovering over ingredients or results displays **tooltips** with detailed information.
- Preferences like sorting and cost display are managed via **MelonPreferences** for easy customization.

## Advanced Search Syntax

You can use special characters to refine your search:

| Syntax      | Behavior                                     | Example                                                                           |
| :---------- | :------------------------------------------- | :-------------------------------------------------------------------------------- |
| `,` (comma) | **AND** search: results must match all terms | `Refreshing,Sedating` → finds drugs matching both "Refreshing" **and** "Sedating" |
| `\|` (pipe) | **OR** search: results can match any term    | `Refreshing\|OG` → finds drugs matching either "Refreshing" **or** "OG"           |

## Known Issues

- **Advanced "Any" Filter**:  
  When using both the "Any" filter and the `,` (AND) separator, the search might incorrectly return "None" even if matching entries exist.  
  _This will be addressed in a future update._

- **Full Recipe Icon Size**:  
  The size of ingredient and result icons in the full recipe view can sometimes appear too large or too small depending on the layout.  
  _A dynamic scaling system is planned for future versions._

## Future Updates & Ideas

- **UI Readability Improvements**:  
  Plans to further enhance the UI layout for better clarity and accessibility, especially when displaying large or complex recipes.

- **Dynamic Recipe Scaling**:  
  Adjust the size of ingredient and result icons dynamically based on the number of elements, improving visual balance.

- **Weighted "Any" Filter**:  
  Improve the "Any" filter by prioritizing fields (e.g., Name over Ingredients or Effects) to return more accurate search results.

- **IL2CPP Compatibility**:  
  Work in progress to support Schedule I's IL2CPP builds, ensuring compatibility with future game versions and broader mod support.

- **Performance Optimization**:  
  Optimize UI generation for extremely large drug lists to maintain real-time responsiveness even with heavy mods or custom content.

---

> _These features are planned for future versions and may evolve based on player feedback and Schedule I updates._
