using System;
using System.Collections.Generic;
using UnityEngine;
using SF = UnityEngine.SerializeField;

namespace Framework.Pool
{
    public class GameObjectPool : MonoBehaviour
    {
        [SF] private GameObject source;
        [SF] private Transform parent;

        private readonly List<Component> pool = new List<Component>();

        private void Awake()
        {
            if (string.IsNullOrEmpty(source.scene.name) == false) // NOTE: way of checking if it's prefab or a scene object
            {
                source.gameObject.SetActive(false);
            }
        }

        public void ReturnAll()
        {
            foreach (var obj in pool)
            {
                obj.gameObject.SetActive(false);
            }
        }

        public void CleanUp()
        {
            foreach (var obj in pool)
            {
                Destroy(obj.gameObject);
            }

            pool.Clear();
        }

        public IEnumerable<T> GetAll<T>() where T : Component
        {
            foreach (var obj in pool)
            {
                yield return obj as T;
            }
        }

        public IEnumerable<T> GetAllActive<T>() where T : Component
        {
            foreach (var obj in pool)
            {
                if (obj.gameObject.activeInHierarchy)
                {
                    yield return obj as T;
                }
            }
        }
        
        public void ReturnToPool(Component obj)
        {
            if (pool.Contains(obj))
            {
                obj.gameObject.SetActive(false);
            }
        }
        
        public void ReturnToPoolIfFound<T>(Predicate<T> match) where T : Component
        {
            ReturnToPool(Find(match));
        }

        public bool Contains<T>(Predicate<T> match) where T : Component
        {
            foreach (var obj in pool)
            {
                var result = obj as T;
                
                if (match(result))
                {
                    return true;
                }
            }
            
            return false;
        }
        
        public T Find<T>(Predicate<T> match) where T : Component
        {
            foreach (var obj in pool)
            {
                var result = obj as T;
                
                if (match(result))
                {
                    return result;
                }
            }
            
            return default;
        }

        public T Take<T>() where T : Component
        {
            foreach (var obj in pool)
            {
                if (obj.gameObject.activeSelf == false)
                {
                    obj.gameObject.SetActive(true);
                    return obj as T;
                }
            }

            var newObj = Instantiate(source, parent ? parent : transform).GetComponent<T>();
            pool.Add(newObj);

            newObj.gameObject.SetActive(true);
            return newObj;
        }
    }
}