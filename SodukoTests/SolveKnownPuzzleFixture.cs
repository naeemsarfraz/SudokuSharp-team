using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using NUnit.Framework;
using SudokuSharp.ViewModels;
using SudokuSharp.WinApp;

namespace SodukoTests
{
    [TestFixture]
    public class SolveKnownPuzzleFixture
    {
        static readonly int? _ = null;

        [Test, TestCaseSource("OnePossibleAnswerCases")]
        public void SolveWhereOnePossibleAnswer(int?[,] puzzle, int expectedX, int expectedY, int cellValue)
        {
            //Arrange
            var model = new BoardViewModel();
            model.NewPuzzle(puzzle);

            //Act
            model.Solve();

            //Assert
            Assert.AreEqual(cellValue, model.Cells[expectedX][expectedY].Number);
        }

        static object[] OnePossibleAnswerCases = 
        {
            new object[] { new int?[9,9] {
                {_, 2, 3, 4, 5, 6, 7, 8, 9},
                {_, _, _, _, _, _, _, _, _},
                {_, _, _, _, _, _, _, _, _},
                {_, _, _, _, _, _, _, _, _},
                {_, _, _, _, _, _, _, _, _},
                {_, _, _, _, _, _, _, _, _},
                {_, _, _, _, _, _, _, _, _},
                {_, _, _, _, _, _, _, _, _},
                {_, _, _, _, _, _, _, _, _}
            }, 0, 0, 1 },
            new object[] { new int?[9,9] {
                {_, _, _, _, _, _, _, _, _},
                {2, _, _, _, _, _, _, _, _},
                {3, _, _, _, _, _, _, _, _},
                {4, _, _, _, _, _, _, _, _},
                {5, _, _, _, _, _, _, _, _},
                {6, _, _, _, _, _, _, _, _},
                {7, _, _, _, _, _, _, _, _},
                {8, _, _, _, _, _, _, _, _},
                {9, _, _, _, _, _, _, _, _}
            }, 0, 0, 1 },
            new object[] { new int?[9,9] {
                {_, 2, 3, _, _, _, _, _, _},
                {4, 5, 6, _, _, _, _, _, _},
                {7, 8, 9, _, _, _, _, _, _},
                {_, _, _, _, _, _, _, _, _},
                {_, _, _, _, _, _, _, _, _},
                {_, _, _, _, _, _, _, _, _},
                {_, _, _, _, _, _, _, _, _},
                {_, _, _, _, _, _, _, _, _},
                {_, _, _, _, _, _, _, _, _}
            }, 0, 0, 1 },
            new object[] { new int?[9,9] {
                {_, _, _, _, _, _, _, _, _},
                {_, _, _, _, _, _, _, _, _},
                {_, _, _, _, 7, _, _, _, _},
                {_, _, _, 1, _, 3, _, _, _},
                {_, _, 9, _, _, _, 8, _, _},
                {_, _, _, 4, 5, 6, _, _, _},
                {_, _, _, _, _, _, _, _, _},
                {_, _, _, _, _, _, _, _, _},
                {_, _, _, _, _, _, _, _, _}
            }, 4, 4, 2 }
            /*new object[] { new int?[9,9] {
                {_, _, _, _, _, _, _, _, _},
                {_, _, _, _, _, _, _, _, _},
                {_, _, _, _, _, _, _, _, _},
                {_, _, _, _, _, _, _, _, _},
                {_, _, _, _, _, _, _, _, _},
                {_, _, _, _, _, _, _, _, _},
                {_, _, _, _, _, _, _, _, _},
                {_, _, _, _, _, _, _, _, _},
                {_, _, _, _, _, _, _, _, _}
            }, 0, 0, 1 }*/
        };

    }
}
