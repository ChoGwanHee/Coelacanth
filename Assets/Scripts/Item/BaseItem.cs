using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseItem : Photon.PunBehaviour {

    public int tableIndex;  // 아이템 매니저에 등록되어 있는 아이템 테이블의 인덱스
    public bool alive;
}
