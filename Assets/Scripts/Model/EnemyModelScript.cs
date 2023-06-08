using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterMode.ICharacter;
using CharacterMode.Model;
using Engine;
public class Enemy : Character
{

    public Enemy(Transform _root)
    {
        CharacterRoot = _root;
        Awake();
    }

    public override void Awake()
    {
        EnemyAni = CharacterRoot.GetComponent<Animation>();
        foreach(AnimationState aniState in EnemyAni)
        {
            EnemyAniClipList.Add(aniState.clip);
        }
        StartEnemyPos = CharacterRoot.position;
        CharacterMove();
    }

    public override void Updata()
    {
        CharacterMove();
        EnemeyAttack();
        //base.Updata();
    }

    //protected Player player;

    protected Transform EnemyAttackTransform;

    protected Animation EnemyAni;

    protected List<AnimationClip> EnemyAniClipList = new List<AnimationClip>();

    protected override void CharacterCameraControl()
    {
        //base.CharacterCameraControl();
    }

    protected override void CharacterMove()
    {
        if (IsTTK) return; 
        if (Vector3.Distance(StartEnemyPos + CharacterTar, CharacterRoot.position) > 0.3f)
        {
            CharacterRoot.LookAt(StartEnemyPos + CharacterTar);
            CharacterRoot.Translate(2.0f * Time.deltaTime * Vector3.forward);
            if (EnemyAniClipList.Count > 1)
            {
                EnemyAni.Play(EnemyAniClipList[1].name);
            }
        }
        else
        {
            float xrand = Random.Range(Random.Range(-xMaxMoveDis, 0), Random.Range(0.1f, xMaxMoveDis));
            float zrand = Random.Range(Random.Range(-zMaxMoveDis, 0), Random.Range(0.1f, zMaxMoveDis));
            CharacterTar = new Vector3(xrand, 0, zrand);
        }
        //base.CharacterMove();
    }

    protected override void HideCharacterCameraTransformControl()
    {
        //base.HideCharacterCameraTransformControl();
    }

    protected void EnemeyAttack()
    {

        if (IsDied)
        {
            CharacterRoot = null;
            return;
        }

        if (Hp <= 0)
        {
            IsDied = true;
            return;
        }

        Hp = Mathf.Max(0, Mathf.Min(Hp, MaxHp));

        if (EnemyAttackTransform && IsTTK && Vector3.Distance(StartEnemyPos + new Vector3(0, EnemyAttackTransform.position.y - StartEnemyPos.y, 0), EnemyAttackTransform.position) <= 10f)
        {
            CharacterTar = EnemyAttackTransform.transform.position + new Vector3(0, CharacterRoot.position.y - EnemyAttackTransform.transform.position.y, 0);
            CharacterRoot.LookAt(CharacterTar);
            if (Vector3.Distance(CharacterRoot.position, EnemyAttackTransform.transform.position) < 1.5f)
            {
                if (EnemyAniClipList.Count > 3)
                {
                    bool isplay = false;
                    for (int i = 2; i < EnemyAniClipList.Count; i++)
                    {
                        if (EnemyAni.IsPlaying(EnemyAniClipList[i].name))
                        {
                            isplay = true;
                            break;
                        }
                    }
                    if (!isplay)
                    {
                        EnemyAni.Play(EnemyAniClipList[Random.Range(2, 4)].name);
                    }
                }
            }
            else
            {
                CharacterRoot.Translate(5.0f * Time.deltaTime * Vector3.forward);
                if (EnemyAniClipList.Count > 1)
                {
                    EnemyAni.Play(EnemyAniClipList[1].name);
                }
            }
        }
        Transform tmptrm = PhysicsCast.CastRoot(CharacterRoot.position + new Vector3(0, 1f, 0), 5f, "Player");
        if (tmptrm && tmptrm.name == "Player")
        {
            IsTTK = true;
            EnemyAttackTransform = tmptrm.transform;
            return;
        }
        else
        {
            if (IsTTK)
            {
                IsTTK = false;
                float xrand = Random.Range(Random.Range(-xMaxMoveDis, 0), Random.Range(0.1f, xMaxMoveDis));
                float zrand = Random.Range(Random.Range(-zMaxMoveDis, 0), Random.Range(0.1f, zMaxMoveDis));
                CharacterTar = new Vector3(xrand, 0, zrand);
            }

        }
    }

    private bool IsTTK = false;

    private bool IsDied = false;

    private float Hp = 1000, MaxHp = 1000;

    private float xMaxMoveDis = 10f, zMaxMoveDis = 10;

    private Vector3 StartEnemyPos;

}
