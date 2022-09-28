using System;
using System.Collections.Generic;
using Data.UnityObject;
using Enums;
using ObjectPool;
using Signals;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Managers
{
    public class ObjectPoolCreator : MonoBehaviour
    {
        #region Self Variables

        #region Private Variables

        private Transform _objTransformCache;
        private CD_Pool _poolData;
        private string _objTypeCache;
        private PoolType _listCache;
        [ShowInInspector]private Dictionary<PoolType,GameObject> _poolGroup =new Dictionary<PoolType, GameObject>();

        #endregion

        #endregion

        private void Awake()
        {
            _objTypeCache = null;
            _poolData = Resources.Load<CD_Pool>("Data/CD_Pool");
            CreatGameObjectGroup();
            InitPool();
        }

        private void CreatGameObjectGroup()
        {
            foreach (var VARIABLE in _poolData.PoolValueDatas)
            {
                var gameObjectCache = new GameObject
                {
                    name = VARIABLE.Key.ToString(),
                    transform =
                    {
                        parent = transform
                    }
                };
                _poolGroup.Add(VARIABLE.Key,gameObjectCache);
            }
        }
        #region Event Subscription

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            PoolSignals.Instance.onGetPoolObject += OnGetPoolObject;
            PoolSignals.Instance.onReleasePoolObject += OnReleasePoolObject;
        }

        private void UnsubscribeEvents()
        {
            PoolSignals.Instance.onGetPoolObject -= OnGetPoolObject;
            PoolSignals.Instance.onReleasePoolObject -= OnReleasePoolObject;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        #endregion

        private GameObject OnGetPoolObject(string poolType,Transform objTransform)
        {
            _objTransformCache = objTransform;
            _objTypeCache = poolType;
            var obj = ObjectPoolManager.Instance.GetObject<GameObject>(poolType);
            return obj;
        }

        private void OnReleasePoolObject(string poolType, GameObject obj)
        {
            _objTypeCache = poolType;
            ObjectPoolManager.Instance.ReturnObject(obj,poolType);
        }
        
        #region Pool Initialization
        
        private void InitPool()
        {
            foreach (var VARIABLE in _poolData.PoolValueDatas)
            {
                _listCache = VARIABLE.Key;
                ObjectPoolManager.Instance.AddObjectPool<GameObject>(FabricateGameObject, TurnOnGameObject, TurnOffGameObject,
                    VARIABLE.Key.ToString(), VARIABLE.Value.ObjectLimit, true);
            }
        }
        
        private void TurnOnGameObject(GameObject gameObject)
        {
            gameObject.transform.localPosition = _objTransformCache.position;
            gameObject.SetActive(true);
        }
        
        private void TurnOffGameObject(GameObject gameObject)
        {
            gameObject.transform.localPosition = Vector3.zero;
            gameObject.transform.SetParent(_poolGroup[_listCache].transform);
            gameObject.SetActive(false);
        }
        
        private GameObject FabricateGameObject()
        {
            if (_objTypeCache != null){_listCache = (PoolType)Enum.Parse(typeof(PoolType), _objTypeCache);}
            return Instantiate(_poolData.PoolValueDatas[_listCache].PooledObject,Vector3.zero,
                _poolData.PoolValueDatas[_listCache].PooledObject.transform.rotation,_poolGroup[_listCache].transform);
        }
        
        #endregion
    }
}
