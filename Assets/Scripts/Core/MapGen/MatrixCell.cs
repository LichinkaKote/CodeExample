using Assets.Scripts.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Core.MapGen
{
    public class MatrixCell : IMapTile
    {
        public int PossibleVariantsCount => posibleConditions.Count;
        public Vector2Int Position { get; private set; }
        public Dictionary<byte, MatrixCell> Neighbours { get; private set; }
        public TileSettings Value { get; private set; }

        public Sprite Sprite => Value.Sprite;
        public float Rotation => Value.Rotation * 90f;
        public bool Walkable => Value.Walkable;

        private List<TileSettings> posibleConditions = new List<TileSettings>();
        private readonly TileSettings[] allConditions;

        public MatrixCell(TileSettings[] allConditions, Vector2Int position)
        {
            this.allConditions = allConditions;
            Position = position;
            Reset();
        }
        public void Reset()
        {
            posibleConditions.Clear();
            for (int i = 0; i < allConditions.Length; i++)
                posibleConditions.Add(allConditions[i]);
        }
        public void SetNeighbours(Dictionary<byte, MatrixCell> neibs)
        {
            Neighbours = neibs;
        }
        public void SetRandomValue()
        {
            if (posibleConditions.Count == 0)
            {
                throw new Exception($"No posibleConditions for cell {Position}");
            }
            else
            {
                var index = GetRandomWeightedIndex(posibleConditions.Select(c => c.Weight).ToArray());
                Value = posibleConditions[index];
                posibleConditions.Clear();
                posibleConditions.Add(Value);
            }
        }
        public List<MatrixCell> Propagate()
        {
            var result = new List<MatrixCell>();
            foreach (var neib in Neighbours)
            {
                var posibleCondForNeib = new List<TileSettings>();
                for (int i = 0; i < posibleConditions.Count; i++)
                {
                    var dirConds = posibleConditions[i].ConnectionRules[neib.Key];
                    for (int j = 0; j < dirConds.Count; j++)
                    {
                        if (!posibleCondForNeib.Contains(dirConds[j]))
                            posibleCondForNeib.Add(dirConds[j]);
                    }
                }

                if (neib.Value.UpdateConditions(posibleCondForNeib))
                    result.Add(neib.Value);
            }
            return result;
        }

        private bool UpdateConditions(List<TileSettings> allowedConditions)
        {
            if (posibleConditions.Count == 0)
            {
                throw new Exception($"UpdateConditions for cell {Position}");
            }
            bool remove = true;
            for (int i = 0; i < posibleConditions.Count; i++)
            {
                remove = true;
                foreach (var condition in allowedConditions)
                {
                    if (condition == posibleConditions[i])
                    {
                        remove = false; break;
                    }
                }
                if (remove)
                {
                    posibleConditions.RemoveAt(i);
                    i--;
                }
            }
            if (posibleConditions.Count == 1)
            {
                Value = posibleConditions.First();
            }
            return remove;
        }
        public void UpdateAllowedConditions(List<TileSettings> allowedConditions)
        {
            UpdateConditions(allowedConditions);
        }

        private int GetRandomWeightedIndex(ushort[] weights)
        {
            int sum = 0;
            for (int i = 0; i < weights.Length; i++)
                sum += weights[i];
            var r = new System.Random();
            var random = r.Next(1, sum + 1);
            int progress = 0;

            for (int i = 0; i < weights.Length; i++)
            {
                progress += weights[i];
                if (random <= progress)
                    return i;
            }
            throw new Exception("GetRandomWeightedIndex error");
        }

    }
}