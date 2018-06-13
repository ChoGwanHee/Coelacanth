using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIResultEmotion : MonoBehaviour {

    private PlayerController pc;

    public void SetPlayerController(PlayerController targetPC)
    {
        pc = targetPC;
    }

    public void EmotionDance(int type)
    {
        pc.SetAnimParam("EmotionType", type - 1);
        pc.ChangeState(PlayerAniState.Emotion);
    }
}
