using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using Game.Model;
using Game.View;
using Zenject;

namespace Game.Controller
{
    public class TileSpawnerController
    {
        [Inject] private ITileSpawnerView _tileSpawnerView;
        [Inject] private IGridModel _gridModel;
        [Inject] private ITileSpawnerModel _tileSpawnerModel;
        
        private const string FilePath = "Assets/Resources/Levels/Level-1-Probability.txt";
        private readonly List<string> _lineData = new List<string>();
        private readonly int[] _probabilityArray = new int[100];

        public void InitSpawner()
        {
            var gridWidth = _gridModel.Width;
            var gridHeight = _gridModel.Height;
            _tileSpawnerView.InitSpawnerArray(gridWidth);
            for (int i = 0; i < _gridModel.Width; i++)
            {
                var cellModel = _gridModel.GetCellModel(i, gridHeight-1);
                _tileSpawnerView.CreateSpawner(i, cellModel.Id);
            }
            ReadProbabilityData();
            InitProbabilityArray();
            // _tileSpawnerView.Log(_probabilityArray);
        }

        private void ReadProbabilityData()
        {
            var streamReader = new StreamReader(FilePath);
            while (streamReader.Peek() >= 0)
            {
                _lineData.Add(streamReader.ReadLine());
            }
            // _lineData.Reverse();
            _tileSpawnerModel.SetData(_lineData.ToArray());
        }

        private void InitProbabilityArray()
        {
            for (int i = 0; i < _tileSpawnerModel.RedProbability; i++)
            {
                _probabilityArray[i] = 1;
            }

            for (int i = _tileSpawnerModel.RedProbability; i <_tileSpawnerModel.RedProbability+ _tileSpawnerModel.GreenProbability; i++)
            {
                _probabilityArray[i] = 2;
            }

            for (int i = _tileSpawnerModel.RedProbability + _tileSpawnerModel.GreenProbability; i < _tileSpawnerModel.RedProbability+ _tileSpawnerModel.GreenProbability+_tileSpawnerModel.BlueProbability; i++)
            {
                _probabilityArray[i] = 3;
            }
            // ShuffleArray(probabilityArray);
        }
        
        // private void ShuffleArray<T>(T[] array)
        // {
        //     for (int i = 0; i < array.Length; i++)
        //     {
        //         int randomIndex =  RandomNumberGenerator.GetInt32(0, array.Length);
        //         (array[i], array[randomIndex]) = (array[randomIndex], array[i]);
        //     }
        // }

        public int GetSpawnTileType()
        {
            return _probabilityArray[RandomNumberGenerator.GetInt32(0, _probabilityArray.Length)];
        }
        
    }
}
