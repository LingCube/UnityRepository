using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CharacterMode.ICharacter;
using CharacterMode.Model;
using TaskMode;
using Engine;

public class NpcModel : Character
{

    public bool IsHavingTask = false;

    public void GiveNpcTask(Task task)
    {
        NpcTaskQueue.Enqueue(task);
    }

    public void GiveTask()
    {
        if (NpcTaskQueue.Count == 0) return;
        TaskControl.TaskGiven = NpcTaskQueue.Peek();
        NpcTaskQueue.Dequeue();
    }



    public NpcModel(Transform _root, Dialog npc_dialog)
    {
        CharacterRoot = _root;
        NpcDialog = npc_dialog;
        NpcDialogStart = GameObject.Find("Canvas/DialogStart").GetComponent<Image>();
        NpcDialogStart.gameObject.SetActive(false);
    }

    public override void Awake()
    {
        //base.Awake();
    }

    public override void Updata()
    {
        Transform dialogtrm = PhysicsCast.CastRoot(CharacterRoot.position + new Vector3(0, 0.5f, 0), 2f, "Player");
        if (dialogtrm && dialogtrm.name == "Player")
        {
            if(!IsDialog) NpcDialogStart.gameObject.SetActive(true);
            if (Input.GetKeyDown(KeyCode.F))
            {            
                NpcDialogStart.gameObject.SetActive(false);
                NpcDialog.OpenDialog();
                IsDialog = true;
            }
            NpcDialog.Updata();
            if (NpcDialog.GetIsDialogFinished)
            {
                IsDialog = false;
            }
        }
        else
        {
            NpcDialogStart.gameObject.SetActive(false);
        }
        //base.Updata();
    }

    protected Dialog NpcDialog;

    protected Queue<Task> NpcTaskQueue = new Queue<Task>();

    protected Image NpcDialogStart;

    protected bool IsDialog = false;


}