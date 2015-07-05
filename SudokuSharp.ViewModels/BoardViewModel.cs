using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

namespace SudokuSharp.ViewModels
{
    public class BoardViewModel
    {
        public enum RotateDirection
        {
            Clockwise = 0,
            CounterClockwise
        }

        bool isDirty = false;
        private bool ifSolving = false;

        public BoardViewModel()
        {
            Cells = new CellViewModel[9][];
            for (var x = 0; x < 9; x++)
            {
                Cells[x] = new CellViewModel[9];
                for (var y = 0; y < 9; y++)
                {
                    Cells[x][y] = new CellViewModel() { Number = 2 };
                    Cells[x][y].PropertyChanged += BoardViewModel_PropertyChanged;
                }
            }
        }

        void BoardViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (!ifSolving)
                return;

            isDirty = true;
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
            ifSolving = false;

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

        public bool IsSolved()
        {
            bool isSolved = true;

            ForEachCell((x, y, cell) =>
            {
                if (cell.Number == null)
                    isSolved = false;
            });

            return isSolved;
        }

        public void SolvePuzzle()
        {
            ifSolving = true;

            while (!IsSolved())
            {
                isDirty = false;
                SolveNextCell();

                if (!isDirty)
                    throw new IAmStuckException();
            }
        }

        public void SolveNextCell()
        {
            ifSolving = true;

            Analyse();
            SolveWhereOnlyOneNumberIsAvailable();
            Analyse();
            SolveWhereOneOptionWithinBlock();
            Analyse();
        }

        private void SolveWhereOneOptionWithinBlock()
        {
            for (int i = 0; i < 9; i = i + 3)
            {
                for (int j = 0; j < 9; j = j + 3)
                {
                    var cellsInBlock = GetBlock(new Point(i, j));
                    Dictionary<int, int> count = new Dictionary<int, int>();
                    foreach (var i1 in Enumerable.Range(1, 9))
                    {
                        count.Add(i1, 0);
                    }

                    foreach (var cellViewModel in cellsInBlock)
                    {
                        foreach (var possibleValue in cellViewModel.PossibleValues)
                        {
                            count[possibleValue]++;
                        }
                    }

                    foreach (var i1 in count.Where(v => v.Value == 1))
                    {
                        foreach (var cellViewModel in cellsInBlock.Where(v => v.PossibleValues.Contains(i1.Key)))
                        {
                            cellViewModel.Number = i1.Key;
                        }
                    }
                }
            }
        }

        private void SolveWhereOnlyOneNumberIsAvailable()
        {
            ForEachCell((x, y, cell) =>
                {
                    if (Cells[x][y].Number == null && cell.PossibleValues.Count == 1)
                        SetCell(x, y, cell.PossibleValues.First());
                });
        }

        private void Analyse()
        {
            ForEachCell((x, y, cell) =>
            {
                if (cell.Number.HasValue)
                {
                    cell.PossibleValues.Clear();
                    return;
                }

                var available = AvailableNumbers(new Point(x, y));

                Debug.WriteLine("cell[{0}][{1}] has {2} numbers available {{{3}}}",
                    x, y, available.Count, String.Join(",", available.ToArray()));

                cell.PossibleValues = available;
            });
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

        public void Rotate(RotateDirection direction)
        {
            if (!Enum.IsDefined(typeof(RotateDirection), direction))
                throw new ArgumentOutOfRangeException("direction");

            int?[,] result = new int?[Cells.Length, Cells.Length];

            ForEachCell((x, y, cell) =>
            {
                if (direction == RotateDirection.Clockwise)
                    result[x, y] = Cells[Cells.Length - y - 1][x].Number;
                else if (direction == RotateDirection.CounterClockwise)
                    result[x, y] = Cells[y][Cells.Length - x - 1].Number;

            });

            NewPuzzle(result);
        }

        private void ForEachCell(Action<int, int, CellViewModel> onEachCell)
        {
            ForEachRow((row) =>
            {
                for (int y = 0; y < Cells[row].Length; y++)
                {
                    onEachCell(row, y, Cells[row][y]);
                }
            });
        }

        private void ForEachRow(Action<int> onEachRow)
        {
            for (int x = 0; x < Cells.Length; x++)
            {
                onEachRow(x);
            }
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