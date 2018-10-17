using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapFacilityTileFalling : BaseMapFacility {

    public TileScript[] tiles;
    

    public override void Activate()
    {
        FallRandomTile();

    }

    public override void Deactivate()
    {
    }

    [PunRPC]
    private void FallTile(int index)
    {
        tiles[index].Fall();
    }

    private void FallRandomTile()
    {
        if (tiles == null) return;

        int random = Random.Range(0, tiles.Length);

        photonView.RPC("FallTile", PhotonTargets.All, random);
    }
}
