using System;

namespace Assets.Scripts.Core.MapGen
{
    public class GeneratorWFC
    {
        private int mapSize;
        private int chunkSize;
        private TileSettings[] tiles;
        private MatrixCell[,] matrix;
        private int chunkTrys;
        public GeneratorWFC(TileSettings[] tiles, int mapSize, int chunkSize)
        {
            this.mapSize = mapSize;
            this.tiles = tiles;
            this.chunkSize = chunkSize;
        }
        public MatrixCell[,] Generate()
        {
            var chunkGen = new ChunkGenerator(tiles, chunkSize);
            matrix = new MatrixCell[mapSize * chunkSize, mapSize * chunkSize];
            for (int x = 0; x < mapSize; x++)
                for (int y = 0; y < mapSize; y++)
                {
                    chunkTrys = 100;
                    GenerateChunk(chunkGen, x, y);
                }

            return matrix;
        }
        private void GenerateChunk(ChunkGenerator chunkGen, int x, int y)
        {
            var leftNeib = GetLeftNeibs(x, y);
            var topNeib = GetTopNeibs(x, y);
            try
            {
                var chunk = chunkGen.GenerateChunk(leftNeib, topNeib);
                SaveToMatrix(chunk, x, y);
            }
            catch (Exception ex)
            {
                if (chunkTrys-- <= 0)
                    throw ex;
                GenerateChunk(chunkGen, x, y);
            }
        }

        private MatrixCell[] GetLeftNeibs(int x, int y)
        {
            int yIndex = y * chunkSize - 1;
            if (yIndex < 0)
                return null;
            var result = new MatrixCell[chunkSize];
            for (int i = 0; i < chunkSize; i++)
            {
                int xIndex = x * chunkSize + i;
                result[i] = matrix[xIndex, yIndex];
            }
            return result;
        }
        private MatrixCell[] GetTopNeibs(int x, int y)
        {
            int xIndex = x * chunkSize - 1;
            if (xIndex < 0)
                return null;
            var result = new MatrixCell[chunkSize];
            for (int i = 0; i < chunkSize; i++)
            {
                int yIndex = y * chunkSize + i;
                result[i] = matrix[xIndex, yIndex];
            }
            return result;
        }

        private void SaveToMatrix(MatrixCell[,] chunk, int x, int y)
        {
            for (int i = 0; i < chunkSize; i++)
                for (int j = 0; j < chunkSize; j++)
                {
                    matrix[i + x * chunkSize, j + y * chunkSize] = chunk[i, j];
                }
        }
    }
}