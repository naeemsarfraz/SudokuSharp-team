using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SudokuSharp.ViewModels;

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
            var board = new int?[2][]
                {
                    
                    new int?[]{1, 2},
                    new int?[]{_, 3}
                };

            //Act
            var result = BoardViewModel.Transpose(board);

            //Assert
            Assert.AreEqual(null, result[0][0]);
            Assert.AreEqual(1, result[0][1]);
            Assert.AreEqual(3, result[1][0]);
            Assert.AreEqual(2, result[1][1]);
        }
    }
}
