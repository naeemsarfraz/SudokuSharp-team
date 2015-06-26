using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SudokuSharp.ViewModels;
using SudokuSharp.WinApp;

namespace SodukoTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestBlock()
        {
            BoardViewModel model = new BoardViewModel();
            model.NewPuzzle(SampleData.StarterPuzzles[0]);
            List<CellViewModel> cellViewModels = model.GetBlock(new Point(0,0));

            Assert.IsTrue(cellViewModels[0].Number == null);
            Assert.IsTrue(cellViewModels[1].Number == null);
            Assert.IsTrue(cellViewModels[2].Number == 2);
            
        }
        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
        public void TestBlockRoot()
        {
            BoardViewModel model = new BoardViewModel();
            model.NewPuzzle(SampleData.StarterPuzzles[0]);


            Assert.IsTrue(model.GetBlockRootIndex(new Point(0,0)) == new Point(0,0));
            Assert.IsTrue(model.GetBlockRootIndex(new Point(8, 8)) == new Point(6,6));
        }
    }
}
