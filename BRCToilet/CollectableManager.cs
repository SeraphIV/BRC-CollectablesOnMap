// StageManager.OnStageInitialized += this.StageInit;

using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Reptile;
using UnityEngine;

namespace BRCToilet;

public class CollectableManager
{
    public CollectableManager()
    {
        StageManager.OnStagePostInitialization += this.StageInit;
    }

    private void StageInit()
    {
        var publicToilets = GetCollectables();

        Plugin.Log.LogDebug("Found " + publicToilets.Length + " toilets!");

        foreach (var toilet in publicToilets) {
            createCollectablePin(toilet);
        }
    }

    private MapPin createCollectablePin(Collectable toilet)
    {
        var mapController = Mapcontroller.Instance;
        var pin = Traverse.Create(mapController)
                    .Method("CreatePin", MapPin.PinType.Pin)
                    .GetValue<MapPin>();

        pin.AssignGameplayEvent(toilet.gameObject);
        pin.InitMapPin(MapPin.PinType.Pin);
        pin.OnPinEnable();

        return pin;
    }

    private Collectable[] GetCollectables()
    {
        
        return UnityEngine.Object.FindObjectsOfType<Collectable>();
    }
}