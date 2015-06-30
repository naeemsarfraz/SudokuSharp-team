using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SudokuSharp.ViewModels;
using SudokuSharp.WinApp;

namespace SodukoTests
{
    [TestFixture]
    public class BoardViewModelFixture
    {
        [TestCase(0, 0, 0, 0)]
        [TestCase(0, 2, 0, 0)]
        [TestCase(2, 2, 0, 0)]
        [TestCase(3, 3, 3, 3)]
        [TestCase(8, 8, 6, 6)]
        [TestCase(6, 2, 6, 0)]
        public void  WhatIsTheRootCellForAGivenPoint(int x, int y, int expectedX, int expectedY)
        {
            //Arrange
            var model = new BoardViewModel();

            //Act
            var result = model.GetRootCellIndex(x, y);

            //Assert
            Assert.AreEqual(expectedX, result.X);
            Assert.AreEqual(expectedY, result.Y);
        }

        [Test]
        public void RotateBoardClockwise()
        {
            //Arrange
            int? _ = null;
            var board = new int?[9,9]
                {
                    {_, _, 2, _, _, _, 8, _, _},
                    {1, _, _, 2, _, _, _, 4, _},
                    {3, _, 6, 8, _, _, 7, 2, _},
                    {_, _, 5, 3, _, _, _, _, 8},
                    {_, 2, _, _, _, _, _, 9, _},
                    {6, _, _, _, _, 1, 5, _, _},
                    {_, 5, 7, _, _, 3, 2, _, 1},
                    {_, 1, _, _, _, 7, _, _, 6},
                    {_, _, 3, _, _, _, 4, _, _}
                };
            var model = new BoardViewModel();
            model.NewPuzzle(board);

            //Act
            model.Rotate(BoardViewModel.RotateDirection.Clockwise);

            //Assert
            Assert.AreEqual(null, model.Cells[0][0].Number);
            Assert.AreEqual(5, model.Cells[1][2].Number);
            Assert.AreEqual(3, model.Cells[2][0].Number);

            Assert.AreEqual(7, model.Cells[6][6].Number);
            Assert.AreEqual(2, model.Cells[7][6].Number);
            Assert.AreEqual(4, model.Cells[7][7].Number);
        }

        [Test]
        public void RotateBoardCounterClockwise()
        {
            //Arrange
            int? _ = null;
            var board = new int?[9, 9]
                {
                    {_, _, 2, _, _, _, 8, _, _},
                    {1, _, _, 2, _, _, _, 4, _},
                    {3, _, 6, 8, _, _, 7, 2, _},
                    {_, _, 5, 3, _, _, _, _, 8},
                    {_, 2, _, _, _, _, _, 9, _},
                    {6, _, _, _, _, 1, 5, _, _},
                    {_, 5, 7, _, _, 3, 2, _, 1},
                    {_, 1, _, _, _, 7, _, _, 6},
                    {_, _, 3, _, _, _, 4, _, _}
                };
            var model = new BoardViewModel();
            model.NewPuzzle(board);

            //Act
            model.Rotate(BoardViewModel.RotateDirection.CounterClockwise);

            //Assert
            Assert.AreEqual(null, model.Cells[0][0].Number);
            Assert.AreEqual(8, model.Cells[0][3].Number);
            Assert.AreEqual(4, model.Cells[1][1].Number);

            Assert.AreEqual(7, model.Cells[6][6].Number);
            Assert.AreEqual(5, model.Cells[7][6].Number);
            Assert.AreEqual(3, model.Cells[6][8].Number);
        }

        
        [Test]
        public void TestBlock()
        {
            BoardViewModel model = new BoardViewModel();
            model.NewPuzzle(SampleData.StarterPuzzles[0]);
            List<CellViewModel> cellViewModels = model.GetBlock(new Point(0,0));

            Assert.IsTrue(cellViewModels[0].Number == null);
            Assert.IsTrue(cellViewModels[1].Number == null);
            Assert.IsTrue(cellViewModels[2].Number == 2);
            
        }

        [Test]
        public void Testrow()
        {
            BoardViewModel model = new BoardViewModel();
            model.NewPuzzle(SampleData.StarterPuzzles[0]);
            List<CellViewModel> cellViewModels = model.GetRow(0);

            Assert.IsTrue(cellViewModels[0].Number == null);
            Assert.IsTrue(cellViewModels[1].Number == null);
            Assert.IsTrue(cellViewModels[2].Number == 2);

            Assert.IsTrue(cellViewModels[3].Number == null);
            Assert.IsTrue(cellViewModels[4].Number == null);
            Assert.IsTrue(cellViewModels[5].Number == null);

            Assert.IsTrue(cellViewModels[6].Number == 8);
            Assert.IsTrue(cellViewModels[7].Number == null);
            Assert.IsTrue(cellViewModels[8].Number == null);

        }

        [Test]
        public void TestColumn()
        {
            BoardViewModel model = new BoardViewModel();
            model.NewPuzzle(SampleData.StarterPuzzles[0]);
            List<CellViewModel> cellViewModels = model.GetColumn(0);

            Assert.IsTrue(cellViewModels[0].Number == null);
            Assert.IsTrue(cellViewModels[1].Number == 1);
            Assert.IsTrue(cellViewModels[2].Number == 3);

            Assert.IsTrue(cellViewModels[3].Number == null);
            Assert.IsTrue(cellViewModels[4].Number == null);
            Assert.IsTrue(cellViewModels[5].Number == 6);

            Assert.IsTrue(cellViewModels[6].Number == null);
            Assert.IsTrue(cellViewModels[7].Number == null);
            Assert.IsTrue(cellViewModels[8].Number == null);

        }
    }
}
