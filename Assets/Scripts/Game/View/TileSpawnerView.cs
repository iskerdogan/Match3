using MyBox.EditorTools;
using UnityEditor;
using UnityEngine;
using Zenject;

namespace Game.View
{
    public interface ITileSpawnerView
    {
        void InitSpawnerArray(int width);
        void CreateSpawner(int width,int id);
        Vector3 GetSpawnTilePosition(int width);
    }
    public class TileSpawnerView : MonoBehaviour,ITileSpawnerView
    {
        [Inject] private GridView _gridView;
        private Transform[] _spawners;

        public void InitSpawnerArray(int width)
        {
            _spawners = new Transform[width];
        }
        
        public void CreateSpawner(int width,int id)
        {
            var spawner =  new GameObject("Spawner-" + width);
            #if UNITY_EDITOR
            var iconContent = EditorGUIUtility.IconContent("sv_label_1");
            EditorGUIUtility.SetIconForObject(spawner, (Texture2D) iconContent.image);
            #endif
            spawner.transform.position = _gridView.GetTileSpawnerPosition(id);
            _spawners[width] = spawner.transform;
        }
        

        public Vector3 GetSpawnTilePosition(int width)
        {
            return _spawners[width].position;
        }
    }
}
