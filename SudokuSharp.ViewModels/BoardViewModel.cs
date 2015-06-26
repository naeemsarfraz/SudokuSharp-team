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
                    Cells[x][y] = new CellViewModel() {Number = 2};
                }
            }
        }

        public  CellViewModel[][] Cells { get; private set;}

        public void SetCell(int x, int y, int value)
        {
            if( !(InRange(x, 0, 8) && InRange(y, 0, 8) && InRange(value, 1, 9)) )
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
            if(xLength != 9 || yLength != 9) throw new ArgumentException("Puzzle data needs to be 9x9 array");

            for (var x = 0; x < 9; x++)
            {
                for (var y = 0; y < 9; y++)
                {
                    Cells[x][y].Number = puzzleData[x,y];
                }
            }
        }

        public void Solve()
        {
                for (int x = 0; x < 9; x++)
                {
                    for (int y = 0; y < 9; y++)
                    {
                        if (!Cells[x][y].Number.HasValue)
                        {
                            var availibleNumbers = AvailibleNumbers(new Point(x, y));
                            Trace.WriteLine(string.Format("x{0}y{1}{2}", x, y, availibleNumbers.Count));
                            if (availibleNumbers.Count == 1)
                            {
                                Cells[x][y].Number = availibleNumbers.First();
                            }

                            List<int> existingNumbersInTriplet = new List<int>();
                            int numberFilled = 0;
                            for (int i = 0; i < 3; i++)
                            {
                                if (Cells[i + (x - x%3)][y].Number.HasValue)
                                {
                                    existingNumbersInTriplet.Add(Cells[i + (x - x%3)][y].Number.Value);
                                         numberFilled++;
                                }
                               

                            }

                            if (numberFilled == 2)
                            {
                                int root = y - (y%3);
                                List<int> columnIndexes = new List<int>();
                                for (int i = root; i < root+3; i++)
                                {
                                    if (y == i)
                                        continue;
                                    
                                    columnIndexes.Add(i);
                                }

                                List<int> inuse1 = GetColumnExcludingBlock(columnIndexes[0], GetBlockRootIndex(new Point(x,y))).Where(c => c.Number.HasValue).Select(c => c.Number.Value).ToList();
                                List<int> inuse2 = GetColumnExcludingBlock(columnIndexes[1], GetBlockRootIndex(new Point(x,y))).Where(c => c.Number.HasValue).Select(c => c.Number.Value).ToList();

                                List<int> cellValues = inuse1.Intersect(inuse2).ToList();

                                foreach (var cellValue in cellValues)
                                {
                                    if (!existingNumbersInTriplet.Contains(cellValue))
                                    {
                                        Cells[x][y].Number = cellValue;
                                    }
                                }
                            }
                        }

                    }
                }







            //foreach (var VARIABLE in Cells)
            //{

            //}

            //DataContext
        }

        private List<int> AvailibleNumbers(Point testCell)
        {
            List<int> fullNumbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            List<CellViewModel> block = GetBlock(GetBlockRootIndex(testCell));

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

        public Point GetBlockRootIndex(Point point)
        {
            return new Point(point.X-point.X%3,point.Y - point.Y%3);
        }
    }
}