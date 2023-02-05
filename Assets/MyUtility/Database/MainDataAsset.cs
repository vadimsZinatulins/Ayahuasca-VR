using UnityEngine;

namespace MyUtility.Database
{
    [CreateAssetMenu(fileName = "New Main Data Asset", menuName = "Naidio/DataAsset/MainDataAsset", order = 0)]
    public class MainDataAsset : ScriptableObject
    {
        // References to the assets
        [SerializeField] private GameObject emptyInteractor;

        // Getters to the assets
        public GameObject GetEmptyInteractorPrefab()
        {
            return emptyInteractor;
        }
    }
}