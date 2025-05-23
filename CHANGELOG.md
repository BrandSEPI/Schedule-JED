# Changelog

## [2.0.0]

### Added

- Added drug sorting functionality by:
  - Newest
  - Addictiveness
  - Cost
  - Price
  - Profit
- Added display of full drug recipes with ingredient icons.
- Added estimated production cost display for each drug.
- Added tooltips when hovering over ingredients and final products, showing their name and description.
- Added config options via MelonPreferences to enable/disable sorting and full recipe view.
- Added advanced search syntax:
  - Use `,` for **AND** searches (must match all terms).
  - Use `|` for **OR** searches (match any term).

### Changed

- Minor visual adjustments to ingredient and result icons.
- Refactored the codebase to use multiple folders instead of a single file.

---

## [1.3.1]

### Added

- BugFix: Error during search bar initialisation: System.MissingMethodException: Method not found: 'Void UnityEngine.Events.UnityAction..ctor(System.Object, IntPtr)'.

---

## [1.3.0]

### Added

- Added a dropdown menu next to the search bar to filter drugs by category: "Any", "Ingredients", "Effects", or "Name"

---

## [1.2.0]

### Added

- Shortcuts are now disabled while the search bar is focused

---

## [1.1.0]

### Changed

- Reduced the size of the clear (X) button to better fit its text

### Fixed

- Fixed an issue where clicking the middle of the search bar would accidentally trigger the clear (X) button

---

## [1.0.0]

### Added

- Added a functional search bar to the Products app
- Real-time filtering of drugs by name
- Displays "None" when no drugs match the query
