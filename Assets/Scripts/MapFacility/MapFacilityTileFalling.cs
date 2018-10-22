using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapFacilityTileFalling : BaseMapFacility {

    public TileScript[] tiles;

    private List<TileScript> activeTileList = new List<TileScript>();


    private void Awake()
    {
        Init();
    }

    public override void Activate()
    {
        FallRandomTile();

    }

    public override void Deactivate()
    {
    }

    private void Init()
    {
        activeTileList.AddRange(tiles);
    }

    [PunRPC]
    private void FallTile(int index)
    {
        tiles[index].Fall();
    }

    private void FallRandomTile()
    {
        if (tiles == null) return;

        if (activeTileList.Count < 1) return;


        int random = Random.Range(0, activeTileList.Count);
        int tileIndex = -1;

        for(int i=0; i<tiles.Length; i++)
        {
            if(tiles[i] == activeTileList[random])
            {
                tileIndex = i;
                break;
            }
        }

        if(tileIndex == -1)
        {
            Debug.LogError("일치하는 타일이 없음");
            return;
        }

        activeTileList.RemoveAt(random);
        photonView.RPC("FallTile", PhotonTargets.All, tileIndex);
    }
}
