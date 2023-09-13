using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Game.Model;
using Game.View;
using Zenject;

namespace Game.Controller
{
    public class LevelController : IInitializable
    {
        [Inject] private readonly GridModel _gridModel;
        [Inject] private readonly LevelView _levelView;
        
        private const string FilePath = "Assets/Resources/Levels/Level-1.txt";
        private List<string> _lineData = new List<string>();
        
        public void Initialize()
        {
            var streamReader = new StreamReader(FilePath);
            while (streamReader.Peek() >= 0)
            {
                _lineData.Add(streamReader.ReadLine());
            }

            _lineData.Reverse();
            _gridModel.SetData(_lineData.ToArray());
        }
    }
}