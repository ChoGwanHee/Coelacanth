/// <summary>
/// 기본 아이템 클래스
/// </summary>
public abstract class BaseItem : Photon.PunBehaviour
{
    /// <summary>
    /// 아이템 매니저에 등록되어 있는 아이템 테이블의 인덱스.
    /// </summary>
    public int tableIndex;

    /// <summary>
    /// 활성화 여부
    /// </summary>
    public bool alive;
}
