using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

namespace SudokuSharp.ViewModels
{
    public class BoardViewModel
    {
        public BoardViewModel()
        {
            Cells = new CellViewModel[9][];
            for (var x = 0; x < 9; x++)
            {
                Cells[x] = new CellViewModel[9];
                for (var y = 0; y < 9; y++)
                {
                    Cells[x][y] = new CellViewModel() { Number = 2 };
                }
            }
        }

        public CellViewModel[][] Cells { get; private set; }

        public void SetCell(int x, int y, int value)
        {
            if (!(InRange(x, 0, 8) && InRange(y, 0, 8) && InRange(value, 1, 9)))
                throw new ArgumentException(string.Format("Error Setting cell[{0}][{1}] to {2}.", x, y, value));

            Cells[x][y].Number = value;
        }

        public void Clear()
        {
            for (var x = 0; x < 9; x++)
            {
                for (var y = 0; y < 9; y++)
                {
                    Cells[x][y].Number = null;
                }
            }
        }

        private static bool InRange(int value, int lowBound, int highBound)
        {
            return (value >= lowBound && value <= highBound);
        }

        public void NewPuzzle(int?[,] puzzleData)
        {
            var xLength = puzzleData.GetLength(0);
            var yLength = puzzleData.GetLength(1);
            if (xLength != 9 || yLength != 9) throw new ArgumentException("Puzzle data needs to be 9x9 array");

            for (var x = 0; x < 9; x++)
            {
                for (var y = 0; y < 9; y++)
                {
                    Cells[x][y].Number = puzzleData[x, y];
                }
            }
        }

        private void analyse()
        {
            for (int x = 0; x < Cells.Length; x++)
            {
                for (int y = 0; y < Cells[x].Length; y++)
                {
                    var rootCell = GetRootCellIndex(x, y);

                    if (Cells[x][y].Number.HasValue)
                    {
                        Cells[x][y].PossibleValues.Clear();
                        continue;
                    }

                    var available = AvailableNumbers(new Point(x, y));

                    Debug.WriteLine("cell[{0}][{1}] has {2} numbers available {{{3}}}",
                        x, y, available.Count, String.Join(",", available.ToArray()));

                    Cells[x][y].PossibleValues = available;
                }
            }
        }

        private void solveWhereOnlyOneNumberIsAvailable()
        {
            forEachRow((row) =>
                forEachColumnInRow(row, (x, y, cell) =>
                {
                    if (Cells[x][y].Number == null && cell.PossibleValues.Count == 1)
                        SetCell(x, y, cell.PossibleValues.First());
                }));
        }

        private void forEachColumnInRow(int x, Action<int, int, CellViewModel> onEachColumnInRow)
        {
            for (int y = 0; y < Cells[x].Length; y++)
            {
                onEachColumnInRow(x, y, Cells[x][y]);
            }
        }

        private void forEachRow(Action<int> onEachRow)
        {
            for (int x = 0; x < Cells.Length; x++)
            {
                onEachRow(x);
            }
        }

        public void Solve()
        {
            analyse();
            solveWhereOnlyOneNumberIsAvailable();

            for (int x = 0; x < Cells.Length; x++)
            {
                for (int y = 0; y < Cells[x].Length; y++)
                {
                    var rootCell = GetRootCellIndex(x, y);

                    if (Cells[x][y].Number.HasValue)
                        continue;

                    List<int> existingNumbersInTriplet = new List<int>();
                    int numberFilled = 0;
                    for (int i = 0; i < 3; i++)
                    {
                        if (Cells[i + (rootCell.X)][y].Number.HasValue)
                        {
                            existingNumbersInTriplet.Add(Cells[i + (rootCell.X)][y].Number.Value);
                            numberFilled++;
                        }
                    }

                    if (numberFilled == 2)
                    {
                        List<int> columnIndexes = new List<int>();
                        for (int i = rootCell.Y; i < rootCell.Y + 3; i++)
                        {
                            if (y == i)
                                continue;

                            columnIndexes.Add(i);
                        }

                        List<int> inuse1 = GetColumnExcludingBlock(columnIndexes[0], rootCell).Where(c => c.Number.HasValue).Select(c => c.Number.Value).ToList();
                        List<int> inuse2 = GetColumnExcludingBlock(columnIndexes[1], rootCell).Where(c => c.Number.HasValue).Select(c => c.Number.Value).ToList();

                        List<int> cellValues = inuse1.Intersect(inuse2).ToList();

                        foreach (var cellValue in cellValues)
                        {
                            if (!existingNumbersInTriplet.Contains(cellValue))
                            {
                                SetCell(x, y, cellValue);
                            }
                        }
                    }

                }
            }
        }

        private List<int> AvailableNumbers(Point testCell)
        {
            List<int> fullNumbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            List<CellViewModel> block = GetBlock(GetRootCellIndex(testCell.X, testCell.Y));

            var row = GetRow(testCell.X);

            var column = GetColumn(testCell.Y);

            List<int> numbersInUse = new List<int>();

            numbersInUse.AddRange(block.Where(cvm => cvm.Number.HasValue).Select(cvm => cvm.Number.Value));
            numbersInUse.AddRange(row.Where(cvm => cvm.Number.HasValue).Select(cvm => cvm.Number.Value));
            numbersInUse.AddRange(column.Where(cvm => cvm.Number.HasValue).Select(cvm => cvm.Number.Value));

            List<int> availibleNumbers = fullNumbers.Except(numbersInUse).ToList();
            return availibleNumbers;
        }

        public List<CellViewModel> GetBlock(Point offset)
        {
            var cellViewModels = new List<CellViewModel>();


            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    cellViewModels.Add(Cells[x + offset.X][y + offset.Y]);
                }
            }



            return cellViewModels;
        }

        public List<CellViewModel> GetRow(int rowNumber)
        {
            var cellViewModels = new List<CellViewModel>();

            for (int y = 0; y < 9; y++)
            {
                cellViewModels.Add(Cells[rowNumber][y]);
            }

            return cellViewModels;
        }

        public List<CellViewModel> GetColumn(int columnNumber)
        {
            var cellViewModels = new List<CellViewModel>();

            for (int x = 0; x < 9; x++)
            {
                cellViewModels.Add(Cells[x][columnNumber]);
            }

            return cellViewModels;
        }

        public List<CellViewModel> GetColumnExcludingBlock(int columnNumber, Point blockRoot)
        {
            var cellViewModels = new List<CellViewModel>();

            for (int x = 0; x < 9; x++)
            {
                if (x == blockRoot.X)
                {
                    x = x + 3;
                }
                if (x > 8)
                {
                    return cellViewModels;
                }
                cellViewModels.Add(Cells[x][columnNumber]);
            }

            return cellViewModels;
        }

        public Point GetRootCellIndex(int x, int y)
        {
            return new Point(x - x % 3, y - y % 3);
        }
    }
}