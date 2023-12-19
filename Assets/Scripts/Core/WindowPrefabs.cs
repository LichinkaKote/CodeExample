using Assets.Scripts.UI.Windows;
using UnityEngine;

namespace Assets.Scripts.Core
{
    [CreateAssetMenu(fileName = "WindowPrefabs", menuName = "ScriptableObjects/WindowPrefabs", order = 1)]
    public class WindowPrefabs : ScriptableObject
    {
        [SerializeField] private Window[] windows;

        public Window[] Windows { get => windows; }
    }
}