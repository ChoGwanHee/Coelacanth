using UnityEngine;

public class BuffHotSauce : Buff
{
    private ItemHotSauce item;
    private float curColorValue = 1.0f;
    private float changeValue = 0.01f;
    private Color color = new Color(1f, 1f, 1f);
    private bool isDown = true;


    public BuffHotSauce(ItemHotSauce item)
    {
        this.item = item;
        duration = item.duration;
    }

    protected override void OnStartBuff(BuffController bc)
    {
        bc.buffEfx.Play(true);
        bc.PC.maxSpeedFactor += item.addSpeed;

        curColorValue = 1.0f;
        changeValue = 0.01f;
        color = new Color(1f, 1f, 1f);
        isDown = true;
    }

    protected override void OnEndBuff(BuffController bc)
    {
        bc.onUpdateBuff -= OnUpdateBuff;
        bc.PC.maxSpeedFactor -= item.addSpeed;

        color.g = 1f;
        color.b = 1f;
        bc.SetCharacterColor(color);
    }

    public override void OnUpdateBuff(BuffController bc)
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= duration)
        {
            if(bc.photonView.isMine)
                Explosion(bc);
            GameObject.Instantiate(bc.hotSauceBoomEfx_ref, bc.transform.position, Quaternion.identity);
            OnEndBuff(bc);
            active = false;
            return;
        }

        // 캐릭터 색깔 변경
        if (isDown)
        {
            curColorValue -= changeValue;
            if (curColorValue <= 0f)
            {
                curColorValue = 0f;
                isDown = false;
            }
        }
        else
        {
            curColorValue += changeValue;
            if (curColorValue >= 1f)
            {
                curColorValue = 1f;
                isDown = true;
            }
        }
        changeValue += 0.001f;
        color.g = curColorValue;
        color.b = curColorValue;

        bc.SetCharacterColor(color);
    }

    /// <summary>
    /// 폭발하여 일정 반경 안의 물체를 밀어내고 피해를 줍니다.
    /// </summary>
    private void Explosion(BuffController bc)
    {
        bc.GetComponent<Collider>().enabled = false;

        Collider[] effectedObjects = Physics.OverlapSphere(bc.transform.position, item.hitRadius, LayerMask.GetMask("DynamicObject"));

        int totalDamage = Mathf.RoundToInt(item.damage * bc.PC.Executer.damageFactor);

        for (int i = 0; i < effectedObjects.Length; i++)
        {
            Vector3 direction = Vector3.Scale(effectedObjects[i].transform.position - bc.transform.position, new Vector3(1, 0, 1)).normalized;

            PhotonView objPhotonView = effectedObjects[i].GetComponent<PhotonView>();
            

            if (effectedObjects[i].CompareTag("Player"))
            {
                PlayerStat effectedPlayer = effectedObjects[i].GetComponent<PlayerStat>();

                if (!effectedPlayer.PC.isUnbeatable)
                {
                    effectedPlayer.PC.Pushed(direction * item.hitForce * 0.5f);
                    objPhotonView.RPC("Pushed", objPhotonView.owner, (direction * item.hitForce));
                    objPhotonView.RPC("DamageShake", objPhotonView.owner, totalDamage, 6, bc.photonView.ownerId);
                }

                // 피격 이펙트
                Vector3 efxPos = effectedObjects[i].GetComponent<CapsuleCollider>().ClosestPointOnBounds(bc.transform.position);
                PhotonNetwork.Instantiate("Prefabs/Effect_base_Hit_fx", efxPos, Quaternion.identity, 0);
            }
            else
            {
                objPhotonView.RPC("Pushed", PhotonTargets.MasterClient, (direction * item.hitForce));
            }
        }
        bc.GetComponent<Collider>().enabled = true;
        bc.photonView.RPC("Damage", bc.photonView.owner, bc.Stat.maxHP, -1);
    }
}
