using UnityEngine;

/// <summary>
/// 캐릭터의 목소리 사운드의 연결정보를 가지고 있는 클래스
/// </summary>
[CreateAssetMenu(menuName = "CharacterVoiceString")]
public class CharacterVoicePack : ScriptableObject {

    /// <summary>
    /// 일반 폭죽 발사 목소리 (로망, 나비)
    /// </summary>
    [FMODUnity.EventRef]
    public string commonFireVoice;

    /// <summary>
    /// 분수 폭죽 설치 목소리
    /// </summary>
    [FMODUnity.EventRef]
    public string fountainVoice;

    /// <summary>
    /// 로켓 폭죽 발사 목소리
    /// </summary>
    [FMODUnity.EventRef]
    public string rocketVoice;

    /// <summary>
    /// 파티 폭죽 발사 목소리
    /// </summary>
    [FMODUnity.EventRef]
    public string partyVoice;

    /// <summary>
    /// 충전 목소리
    /// </summary>
    [FMODUnity.EventRef]
    public string chargingVoice;

    /// <summary>
    /// 피격 목소리
    /// </summary>
    [FMODUnity.EventRef]
    public string hitVoice;

    /// <summary>
    /// 기절 목소리
    /// </summary>
    [FMODUnity.EventRef]
    public string stunVoice;

    /// <summary>
    /// 낙하 목소리
    /// </summary>
    [FMODUnity.EventRef]
    public string fallingVoice;

    /// <summary>
    /// 승리 목소리
    /// </summary>
    [FMODUnity.EventRef]
    public string victoryVoice;
}
