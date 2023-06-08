using BagMode.IBag;
using CharacterMode.ICharacter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataStruct;

namespace CharacterMode
{
    namespace Model
    {
        public class Character
        {

            public Transform GetCharacterRoot => CharacterRoot;

            public Camera GetCharacterCamera => CharacterCamera;

            public void SetMoveSpeed(float SetSpeed) => CharacterMoveSpeed = SetSpeed;

            public Character()
            {

            }

            public Character(Transform _character_root, Camera _character_camera)
            {
                CharacterRoot = _character_root;
                CharacterCamera = _character_camera;
                Awake();
            }

            public virtual void Awake()
            {
                CharacterRig = CharacterRoot.GetComponent<Rigidbody>() ? CharacterRoot.GetComponent<Rigidbody>() : CharacterRoot.gameObject.AddComponent<Rigidbody>();
                HideCharacterCameraTransform = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
                HideCharacterCameraTransform.name = "HideCharacterCameraTransform";
                HideCharacterCameraTransform.position = CharacterCamera.transform.position;
                HideCharacterCameraTransform.localScale = Vector3.zero;
                HideCharacterCameraTransform.GetComponent<Collider>().isTrigger = true;
                if (CharacterCamera.transform.parent)
                {
                    HideCharacterCameraTransform.SetParent(CharacterCamera.transform.parent);
                }
                CharacterCameraToCharacterOffset = CharacterCamera.transform.position - CharacterRoot.position;
            }

            public virtual void Updata()
            {
                if (!CharacterRoot || !CharacterCamera) return;
                CharacterMove();
                HideCharacterCameraTransformControl();
                CharacterCameraControl();
            }

            protected Transform CharacterRoot = null;

            protected Transform HideCharacterCameraTransform = null;

            protected Rigidbody CharacterRig;

            protected Camera CharacterCamera = null;

            protected Vector3 CharacterTar = Vector3.zero;

            protected Vector3 CharacterCameraToCharacterOffset;

            protected float CharacterMoveSpeed = 2.0f;

            protected virtual void CharacterMove()
            {
                if (Input.GetKey(KeyCode.W))
                {
                    Vector3 wtar = Vector3.Cross(CharacterRoot.up, -CharacterCamera.transform.right);
                    CharacterTar += wtar - new Vector3(0, wtar.y, 0);
                }
                if (Input.GetKey(KeyCode.S))
                {
                    Vector3 star = Vector3.Cross(CharacterRoot.up, CharacterCamera.transform.right);
                    CharacterTar += star - new Vector3(0, star.y, 0);
                }
                if (Input.GetKey(KeyCode.A))
                {
                    Vector3 atar = Vector3.Cross(CharacterRoot.up, -CharacterCamera.transform.forward);
                    CharacterTar += atar - new Vector3(0, atar.y, 0);
                }
                if (Input.GetKey(KeyCode.D))
                {
                    Vector3 dtar = Vector3.Cross(CharacterRoot.up, CharacterCamera.transform.forward);
                    CharacterTar += dtar - new Vector3(0, dtar.y, 0);
                }
                CharacterRoot.LookAt(CharacterTar / 2 + CharacterRoot.position);
                CharacterRoot.Translate((CharacterTar == Vector3.zero ? 0 : CharacterMoveSpeed) * Time.deltaTime * Vector3.forward);
                CharacterTar = Vector3.zero;
            }

            protected virtual void CharacterCameraControl()
            {
                Vector3 pos = HideCharacterCameraTransform.position;
                CharacterCamera.transform.position = Vector3.Lerp(CharacterCamera.transform.position, pos, 5.0f * Time.deltaTime);
                CharacterCamera.transform.rotation = HideCharacterCameraTransform.rotation;
                Vector3 startpos = CharacterCamera.transform.position;
                //CharacterCamera.transform.Translate(Vector3.forward * Input.GetAxis("Mouse ScrollWheel"));
                CharacterCamera.fieldOfView -= 500 * Time.deltaTime * Input.GetAxis("Mouse ScrollWheel");
                CharacterCamera.fieldOfView = Mathf.Clamp(CharacterCamera.fieldOfView, 20, 100);
                CharacterCamera.transform.RotateAround(CharacterRoot.position, Vector3.up, Input.GetAxis("Mouse X"));
                Quaternion q = CharacterCamera.transform.rotation;
                CharacterCamera.transform.RotateAround(CharacterRoot.position, Vector3.Cross(CharacterCamera.transform.forward, CharacterRoot.up), Input.GetAxis("Mouse Y"));
                if (CharacterCamera.transform.eulerAngles.x > 75)
                {
                    CharacterCamera.transform.SetPositionAndRotation(startpos, q);
                }
                CharacterCamera.transform.LookAt(CharacterRoot);
            }

            protected virtual void HideCharacterCameraTransformControl()
            {
                HideCharacterCameraTransform.position = CharacterRoot.position + CharacterCameraToCharacterOffset;
                HideCharacterCameraTransform.RotateAround(CharacterRoot.position, Vector3.up, Input.GetAxis("Mouse X"));
                Vector3 startpos = HideCharacterCameraTransform.position;
                Quaternion q = HideCharacterCameraTransform.rotation;
                HideCharacterCameraTransform.RotateAround(CharacterRoot.position, Vector3.Cross(HideCharacterCameraTransform.forward, CharacterRoot.up), Input.GetAxis("Mouse Y"));
                //HideCharacterCameraTransform.Translate(Vector3.forward * Input.GetAxis("Mouse ScrollWheel"));
                //if (CharacterCamera.fieldOfView < 20 || CharacterCamera.fieldOfView > 100)
                //{
                //    HideCharacterCameraTransform.position = startpos;
                //}
                if (HideCharacterCameraTransform.eulerAngles.x > 75)
                {
                    HideCharacterCameraTransform.SetPositionAndRotation(startpos, q);
                }
                HideCharacterCameraTransform.LookAt(CharacterRoot);
                CharacterCameraToCharacterOffset = HideCharacterCameraTransform.position - CharacterRoot.position;
            }

        }
    }

    namespace ICharacter
    {
        public interface ICharacterState
        {
            void Move();
        }

        public interface ICharacterProp
        {
            void PropControl();
        }
    }
}

namespace BagMode
{
    namespace Model
    {
        public class BagItem
        {

            public enum BagItemType
            {
                Equip,
                Drug,
                Tool,
            }

            public enum BagItemValue
            {
                N,
                R,
                SR,
                SSR,
                SP
            }

            public Transform GetBagItemRoot => Root;

            public string GetBagItemName => Name;

            public BagItemType GetBagItemType => Type;

            public Sprite GetBagItemSprite => BagItemSprite;

            public BagItem()
            {

            }

            public BagItem(Transform _root, BagItemValue value)
            {
                Root = _root;
                Value = value;
            }

            public BagItem(Transform _root, BagItemType type, BagItemValue value)
            {
                Type = type;
                Value = value;
                Root = _root;
            }

            protected Transform Root = null;

            protected string Name;

            protected BagItemType Type;

            protected BagItemValue Value;

            protected Sprite BagItemSprite;

        }

        public class Bag
        {

            public Bag()
            {

            }

            protected List<BagItem> BagItemList = new List<BagItem>();


        }
    }

    namespace IBag
    {
        public interface IBagItem
        {
            protected void Effect();
        }
    }
}



namespace TaskMode
{

    public class Task
    {
        

        public virtual void Updata()
        {
            if (list == null) return;
            if (list.val.GetTar) list = list.next;
        }

        public virtual void AddTastStep(TaskStep step)
        {
            ListNode<TaskStep> tmpnode = new ListNode<TaskStep>(step);
            tmpnode.next = list.next;
            list.next = tmpnode;
        }

        protected ListNode<TaskStep> list = new ListNode<TaskStep>();

    }

    public class TaskStep
    {

        public bool GetTar => IsGetTaskTar();

        public int InTaskCnt = 0;

        public TaskStepType GetTaskStepType => TSType;

        public enum TaskStepType
        {
            ShouldGoToSomePlace,
            ShouldHaveDialog,
            ShouldGiveSomething,
            ShouldFightEnemy
        }

        public TaskStep(Transform taskperson, Vector3 pos)
        {
            if (is_create_task) return;
            is_create_task = true;
            TaskPerson = taskperson;
            TaskPos = pos;
            TSType = TaskStepType.ShouldGoToSomePlace;
        }

        public TaskStep(Transform taskperson, NpcModel npc, Dialog dialog)
        {
            if (is_create_task) return;
            is_create_task = true;
            TaskPerson = taskperson;
            TaskNpc = npc;
            TaskDialog = dialog;
            TSType = TaskStepType.ShouldHaveDialog;
        }

        public TaskStep(Transform taskperson, Tool tool, int cnt)
        {
            if (is_create_task) return;
            is_create_task = true;
            TaskPerson = taskperson;
            TaskTool = tool;
            TaskCnt = cnt;
            TSType = TaskStepType.ShouldGiveSomething;
        }

        public TaskStep(Transform taskperson, Enemy enemy, int cnt)
        {
            if (is_create_task) return;
            is_create_task = true;
            TaskPerson = taskperson;
            TaskEnemy = enemy;
            TaskCnt = cnt;
            TSType = TaskStepType.ShouldFightEnemy;
        }

        protected TaskStepType TSType;

        protected Transform TaskPerson;

        protected Vector3 TaskPos = default;

        protected NpcModel TaskNpc = null;

        protected Dialog TaskDialog = null;

        protected Tool TaskTool = null;

        protected Enemy TaskEnemy = null;

        protected int TaskCnt = 0;

        protected virtual bool IsGetTaskTar()
        {
            switch (TSType)
            {
                case TaskStepType.ShouldGoToSomePlace:
                    return Vector3.Distance(TaskPerson.position, TaskPos) < 2f;
                    break;
                case TaskStepType.ShouldHaveDialog:
                    return TaskDialog.GetIsDialogFinished;
                    break;
                case TaskStepType.ShouldGiveSomething:
                    return TaskTool.GetCnt >= TaskCnt;
                    break;
                case TaskStepType.ShouldFightEnemy:
                    return InTaskCnt >= TaskCnt;
                    break;
                default:
                    break;
            }
            return true;
        }

        protected virtual bool TaskAboutShouldGoToSomePlace(Vector3 pos)
        {
            if (Vector3.Distance(TaskPerson.position, pos) < 2f) return true;
            return false;
        }

        protected virtual bool TaskAboutShouldHaveDialog(NpcModel npc, Dialog dialog)
        {
            if (dialog.GetIsDialogFinished) return true;
            return false;
        }

        protected virtual bool TaskAboutShouldGiveSomething(Tool tool, int cnt)
        {
            if (tool.GetCnt >= cnt) return true;
            return false;
        }

        protected virtual bool TaskAboutShouldFightEnemy(Enemy enemy, int cnt)
        {
            if (TaskCnt >= cnt) return true;
            return false;
        }

        private bool is_create_task = false;

    }
      
}


namespace Engine
{
    public class PhysicsCast
    {

        public enum CastRootType
        {
            All,
        }

        public static Transform CastRoot(Vector3 origin, float maxlen, string name, CastRootType type = CastRootType.All)
        {

            //Dictionary<Ray, bool> RayToBoolDic = new Dictionary<Ray, bool>();

            //List<Ray> ForwardToLeftToUpRayList = new List<Ray>();
            //List<Ray> ForwardToLeftToDownRayList = new List<Ray>();
            //List<Ray> ForwardToRightToUpRayList = new List<Ray>();
            //List<Ray> ForwardToRightToDownRayList = new List<Ray>();
            //List<Ray> BackToLeftToUpRayList = new List<Ray>();
            //List<Ray> BackToLeftToDownRayList = new List<Ray>();
            //List<Ray> BackToRightToUpRayList = new List<Ray>();
            //List<Ray> BackToRightToDownRayList = new List<Ray>();

            List<Ray> RayList = new List<Ray>();

            Ray ForwardRay = new Ray(origin, new Vector3(0, 0, 1));
            RayList.Add(ForwardRay);
            Ray BackRay = new Ray(origin, new Vector3(0, 0, -1));
            RayList.Add(BackRay);

            Ray LeftRay = new Ray(origin, new Vector3(-1, 0, 0));
            RayList.Add(LeftRay);
            Ray RightRay = new Ray(origin, new Vector3(1, 0, 0));
            RayList.Add(RightRay);

            Ray UpRay = new Ray(origin, new Vector3(0, 1, 0));
            RayList.Add(UpRay);
            Ray DownRay = new Ray(origin, new Vector3(0, -1, 0));
            RayList.Add(DownRay);


            Ray ForwardToLeftRay = new Ray(origin, new Vector3(-1, 0, 1));
            RayList.Add(ForwardToLeftRay);
            Ray ForwardToRightRay = new Ray(origin, new Vector3(1, 0, 1));
            RayList.Add(ForwardToRightRay);
            Ray BackToLeftRay = new Ray(origin, new Vector3(-1, 0, -1));
            RayList.Add(BackToLeftRay);
            Ray BackToRightRay = new Ray(origin, new Vector3(1, 0, -1));
            RayList.Add(BackToRightRay);


            Ray ForwardToUpRay = new Ray(origin, new Vector3(0, 1, 1));
            RayList.Add(ForwardToUpRay);
            Ray ForwardToDownRay = new Ray(origin, new Vector3(0, -1, 1));
            RayList.Add(ForwardToDownRay);
            Ray BackToUpRay = new Ray(origin, new Vector3(0, 1, -1));
            RayList.Add(BackToUpRay);
            Ray BackToDownRay = new Ray(origin, new Vector3(0, -1, -1));
            RayList.Add(BackToDownRay);


            Ray LeftToUpRay = new Ray(origin, new Vector3(-1, 1, 0));
            RayList.Add(LeftToUpRay);
            Ray LeftToDownRay = new Ray(origin, new Vector3(-1, 1, 0));
            RayList.Add(LeftToDownRay);
            Ray RightToUpRay = new Ray(origin, new Vector3(1, 1, 0));
            RayList.Add(RightToUpRay);
            Ray RightToDownRay = new Ray(origin, new Vector3(1, -1, 0));
            RayList.Add(RightToDownRay);


            Ray ForwardToLeftToUpRay = new Ray(origin, new Vector3(-1, 1, 1));
            RayList.Add(ForwardToLeftToUpRay);
            Ray ForwardToLeftToDownRay = new Ray(origin, new Vector3(-1, -1, 1));
            RayList.Add(ForwardToLeftToDownRay);
            Ray ForwardToRightToUpRay = new Ray(origin, new Vector3(1, 1, 1));
            RayList.Add(ForwardToRightToUpRay);
            Ray ForwardToRightToDownRay = new Ray(origin, new Vector3(1, -1, 1));
            RayList.Add(ForwardToRightToDownRay);
            Ray BackToLeftToUpRay = new Ray(origin, new Vector3(-1, 1, -1));
            RayList.Add(BackToLeftToUpRay);
            Ray BackToLeftToDownRay = new Ray(origin, new Vector3(-1, -1, -1));
            RayList.Add(BackToLeftToDownRay);
            Ray BackToRightToUpRay = new Ray(origin, new Vector3(1, 1, -1));
            RayList.Add(BackToRightToUpRay);
            Ray BackToRightToDownRay = new Ray(origin, new Vector3(1, -1, -1));
            RayList.Add(BackToRightToDownRay);

            foreach (var ray in RayList) 
            {
                Debug.DrawRay(ray.origin, ray.direction * maxlen);
                if (Physics.Raycast(ray, out RaycastHit hit, maxlen))
                {
                    if (hit.transform.name == name)
                    {
                        return hit.transform;
                    }
                }
            }
            return null;
        }
    }
}




