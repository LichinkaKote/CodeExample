using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Core.MapGen
{
    public class ChunkGenerator
    {
        private int size;
        private TileSettings[] tiles;
        private MatrixCell[,] chunkMatrix;
        public ChunkGenerator(TileSettings[] tiles, int size)
        {
            this.size = size;
            this.tiles = tiles;
        }

        public MatrixCell[,] GenerateChunk(MatrixCell[] leftNeibs, MatrixCell[] topNeibs)
        {
            InitChunkMatrix();
            UpdateNeighbours();

            var cellsToPropagate = PropagateFromOuterNebs(leftNeibs, topNeibs);
            bool allCellsHaveVariants = CheckForEmptyVariants(cellsToPropagate);

            if (!allCellsHaveVariants)
            {
                throw new Exception("Imposible to generate chunk due to neibors");
            }

            Propagate(cellsToPropagate);

            return chunkMatrix;
        }

        private bool CheckForEmptyVariants(List<MatrixCell> cellsToPropagate)
        {
            return cellsToPropagate.All(x => x.PossibleVariantsCount > 0);
        }

        private void Propagate(List<MatrixCell> cellsToPropagate)
        {
            int iterator = size * size + 10;
            while (iterator-- > 0)
            {
                if (cellsToPropagate.Count == 0)
                {
                    var startCell = GetRandomCell();
                    if (startCell == null)
                    {
                        //Debug.Log("Finish");
                        break;
                    }

                    startCell.SetRandomValue();
                    var startneibs = startCell.Propagate();
                    cellsToPropagate.AddRange(startneibs);
                }

                int count = size * size + 50;
                while (cellsToPropagate.Count > 0)
                {
                    if (count < 0)
                    {
                        throw new Exception("Break");
                    }
                    var lowestEntCell = ExtractLowestEntropyCell(cellsToPropagate);

                    var neibs = lowestEntCell.Propagate();
                    cellsToPropagate.AddRange(neibs);
                    count--;
                }
            }
            if (iterator <= 0)
                throw new Exception("Iterations overflow");
        }

        private List<MatrixCell> PropagateFromOuterNebs(MatrixCell[] leftNeibs, MatrixCell[] topNeibs)
        {
            var result = new List<MatrixCell>();

            if (leftNeibs != null)
            {
                for (int i = 0; i < size; i++)
                {
                    var allowedRight = leftNeibs[i].Value.ConnectionRules[1];
                    chunkMatrix[i, 0].UpdateAllowedConditions(allowedRight);
                    result.Add(chunkMatrix[i, 0]);
                }
            }

            if (topNeibs != null)
            {
                for (int i = 0; i < size; i++)
                {
                    var allowedDown = topNeibs[i].Value.ConnectionRules[2];
                    chunkMatrix[0, i].UpdateAllowedConditions(allowedDown);
                    result.Add(chunkMatrix[0, i]);
                }
            }

            return result;
        }

        private MatrixCell GetRandomCell()
        {
            var listCells = new List<MatrixCell>();
            for (int x = 0; x < size; x++)
                for (int y = 0; y < size; y++)
                {
                    var cell = chunkMatrix[x, y];
                    if (cell.PossibleVariantsCount > 1)
                        listCells.Add(cell);
                }

            if (listCells.Count == 0)
                return null;
            var r = new System.Random();
            var rndIndex = r.Next(0, listCells.Count);
            return listCells[rndIndex];
        }

        private void InitChunkMatrix()
        {
            chunkMatrix = new MatrixCell[size, size];

            for (int x = 0; x < size; x++)
                for (int y = 0; y < size; y++)
                {
                    chunkMatrix[x, y] = new MatrixCell(tiles, new Vector2Int(y, x));
                }
        }
        private void UpdateNeighbours()
        {
            for (int x = 0; x < size; x++)
                for (int y = 0; y < size; y++)
                {
                    var cell = chunkMatrix[x, y];
                    cell.SetNeighbours(GetNeibPositions(x, y));
                }
        }

        private Dictionary<byte, MatrixCell> GetNeibPositions(int x, int y)
        {
            var result = new Dictionary<byte, MatrixCell>();

            if (x - 1 >= 0)
                result.Add(0, chunkMatrix[x - 1, y]);

            if (y + 1 < size)
                result.Add(1, chunkMatrix[x, y + 1]);

            if (x + 1 < size)
                result.Add(2, chunkMatrix[x + 1, y]);

            if (y - 1 >= 0)
                result.Add(3, chunkMatrix[x, y - 1]);

            return result;
        }


        private MatrixCell ExtractLowestEntropyCell(List<MatrixCell> cellsToPropagate)
        {
            int compare = cellsToPropagate[0].PossibleVariantsCount;
            var r = new System.Random();
            int resultIndex = r.Next(0, cellsToPropagate.Count);

            for (int i = 0; i < cellsToPropagate.Count; i++)
            {
                if (cellsToPropagate[i].PossibleVariantsCount < compare)
                {
                    compare = cellsToPropagate[i].PossibleVariantsCount;
                    resultIndex = i;
                }
            }
            var result = cellsToPropagate[resultIndex];
            cellsToPropagate.RemoveAt(resultIndex);
            return result;
        }
    }
}