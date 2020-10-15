using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using RPG.Core;
using RPG.Attributes;
using System;
using UnityEngine.AI;

namespace RPG.Control{
    public class PlayerController : MonoBehaviour {

            
        [System.Serializable]
        struct CursorMapping
        {
            public CursorType type;
            public Texture2D texture;
            public Vector2 hotspot;
        }

        [SerializeField] CursorMapping[] cursorMappings = null;
        [SerializeField] float maxNavMeshProjectionDistance = 1f;

        void Update()
        {
            if (InteractWithUI())
            {
                return;
            }
            if (GetComponent<Health>().isDead)
            {
                SetCursor(CursorType.None);
                return;
            }
            if(InteractWithComponent()) return;
            if(InteractWithMovement()){
                return;
            }
            SetCursor(CursorType.None);

        }

        private bool InteractWithComponent()
        {
            RaycastHit[] raycastHits = RaycastSort();
            foreach(RaycastHit hit in raycastHits)
            {
                IRayCastable[] rayCastables = hit.transform.GetComponents<IRayCastable>();
                foreach(IRayCastable rayCastable in rayCastables)
                {
                    if (rayCastable.HandleRaycast(this))
                    {
                        SetCursor(rayCastable.GetCursorType());
                        return true;
                    }
                }
            }
            return false;
        }

        private RaycastHit[] RaycastSort()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            float[] distances = new float[hits.Length];
            for(int i = 0; i < hits.Length; i++)
            {
                distances[i] = hits[i].distance;
            }
            Array.Sort(distances, hits);
            return hits;

        }

        private bool InteractWithUI()
        {
            if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            {
                SetCursor(CursorType.UI);
                return true;
            }
            return false;
        }

        private bool InteractWithMovement()
        {

            Vector3 target;
            bool hasHit = RaycastNavMesh(out target);
            if (hasHit)
            {
                if (!GetComponent<Mover>().CanMoveTo(target)) return false;
                if(Input.GetMouseButton(0)){
                    GetComponent<Mover>().StartMoveAction(target, 1f);
                }
                SetCursor(CursorType.Movement);
                return true;
            }
            return false;
        }

        private bool RaycastNavMesh(out Vector3 target)
        {
            target = new Vector3();
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            if (!hasHit) return false;
            NavMeshHit navMeshHit;
            bool hasCastNavMesh =  NavMesh.SamplePosition(hit.point, out navMeshHit, maxNavMeshProjectionDistance, NavMesh.AllAreas);
            if (!hasCastNavMesh) return false;

            target = navMeshHit.position;

            return GetComponent<Mover>().CanMoveTo(target);
        }

       

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }

        private void SetCursor(CursorType type)
        {
            CursorMapping mapping = GetCursorMapping(type);
            Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto);

        }

        private CursorMapping GetCursorMapping(CursorType type)
        {
            CursorMapping none=new CursorMapping();
            foreach(CursorMapping mapping in cursorMappings)
            {
                if(mapping.type == type)
                {
                    return mapping;
                }
                else if(mapping.type == CursorType.None)
                {
                    none = mapping;
                }
            }
            return none;

        }
    }
}
