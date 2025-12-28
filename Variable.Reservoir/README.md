# Variable.Reservoir

**Variable.Reservoir** models composite resources that have an active "clip" or "tank" and a larger "reserve". This is the standard model for Ammo, Batteries, and Fuel.

## Installation

```bash
dotnet add package Variable.Reservoir
```

## Features

*   **ReservoirInt / ReservoirFloat**: Structs for Clip + Reserve.
*   **Refill()**: Logic to move resources from Reserve to Clip/Volume.
*   **Implicit Conversion**: Treats the struct as the current clip value for easy comparisons.

## Usage

### Gun Ammo
```csharp
using Variable.Reservoir;

// Clip Size 30, Current 30, Reserve 90
ReservoirInt ammo = new ReservoirInt(30, 30, 90);

public void Shoot() {
    if (ammo > 0) {
        ammo.Volume--;
        FireBullet();
    } else {
        Reload();
    }
}

public void Reload() {
    // Moves up to 30 from reserve to clip
    int amountReloaded = ammo.Refill(); 
    PlayReloadAnimation();
}
```

### Flashlight Battery
```csharp
// Capacity 100%, Current 50%, Reserve 200% (2 batteries)
ReservoirFloat battery = new ReservoirFloat(100f, 50f, 200f);

public void SwapBattery() {
    battery.Refill(); // Fills current to 100%, reduces reserve
}
```

---
**Author:** Md Ishtiaq Ahamed Fahim  
**GitHub:** [iafahim/GameVariable](https://github.com/iafahim/GameVariable)
