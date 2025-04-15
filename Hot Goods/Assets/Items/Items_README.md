# 📦 Item System Overview

This document explains the structure, data flow, and usage of the item system in this project.

---

## 🧱 Core Structure

All item types inherit from a common base class `ItemData`. Each type adds specific behavior and fields based on its role.

### Common Fields (in `ItemData`)
- `int id` – unique ID (used in JSON and command system)
- `string itemName` – display name
- `string description` – description text
- `float baseValue` – base price
- `PackageSize packageSize` – affects model scale
- `bool stackable` – if item can stack (true for most types, false for BaseItems)
- `Sprite icon` – custom or default icon
- `GameObject model` – custom or default 3D model

---

## 📦 Item Types

### 1. **Base Items** (`BaseItemData`)
Main game items (phones, chairs, monitors, etc). Not stackable.
- `List<PerkData> perks`
- `List<NegativeData> negatives`
- `TrendsData trend`

### 2. **Fix Items** (`FixItemData`)
Used to remove one `NegativeData` from a BaseItem.
- `NegativeData fixesNegative`

### 3. **DIY Items** (`DIYItemData`)
Adds a `PerkData` to a BaseItem.
- `PerkData addsPerk`
- `Vector2 bonusRange` – value boost in %

### 4. **Consumables** (`ConsumableItemData`)
Usable items that apply effects.
- `List<ConsumableEffect> effects`

### 5. **Perks** (`PerkData`)
Affects value positively. Used by BaseItems and DIY items.
- `string perkName`
- `float bonusValue`

### 6. **Negatives** (`NegativeData`)
Affects value negatively. Used by BaseItems and Fix items.
- `string negativeName`
- `float penaltyValue`

### 7. **Trends** (`TrendsData`)
Defines item category (e.g. Tech, Furniture). Used by BaseItems.
- `string trendName`

---

## 🔁 JSON Workflow
All items are defined in `/Assets/Items/JSON/*.json`.

- Run **Tools > Import Game Items From JSON**
- Script parses JSON and generates ScriptableObjects in `/Generated` folders
- Asset filenames are based on `itemName` for clarity

---

## 🆔 ID System
Every item/perk/negative/trend has a unique numeric ID:
- `1000–1999` → Base Items
- `2000–2999` → Fix Items
- `3000–3999` → DIY Items
- `4000–4999` → Consumables
- `5000–5999` → Perks
- `6000–6999` → Negatives
- `9000–9999` → Trends

Used in systems like debug console: `give 1001 1`

---

## 📁 Folder Layout
```
Items/
├── JSON/              ← source data
├── Generated/         ← auto-created ScriptableObjects
│   ├── BaseItems/
│   ├── FixItems/
│   ├── DIYItems/
│   ├── Consumables/
│   ├── Perks/
│   ├── Negatives/
│   └── Trends/
└── Scripts/
    └── ItemJsonImporter.cs
```

---

## 🧩 Extending
- Add more perks/negatives/trends in JSON
- Add new item types by extending `ItemData`
- UI systems can filter by `trend`, show bonuses from `perks`, etc.

---

## ✅ Example Use
```json
{
  "id": 1001,
  "itemName": "Used Phone",
  "description": "Cracked but functional.",
  "baseValue": 100.0,
  "packageSize": "S",
  "perkIDs": [5002],
  "negativeIDs": [6001, 6002],
  "trendID": 9001
}
```

