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

        [Test]
        [TestCaseSource("OnePossibleAnswerCases")]
        [TestCaseSource("ThirdRemainingColumnWithOneOptionAnswerCases")]
        [TestCaseSource("ThirdRemainingColumnUseOppositeDimensionAnswerCases")]
        [TestCaseSource("OnePossibleAnswerForColumn")]
        [TestCaseSource("OnePossibleAnswerForRow")]
        public void SolveCell(int?[,] puzzle, int expectedX, int expectedY, int cellValue)
        {
            //Arrange
            var model = new BoardViewModel();
            model.NewPuzzle(puzzle);

            //Act
            model.SolveNextCell();

            //Assert
            Assert.AreEqual(cellValue, model.Cells[expectedX][expectedY].Number);
        }

        [Test]
        public void SolveNotPossible()
        {
            //Arrange
            var model = new BoardViewModel();
            model.NewPuzzle(new int?[9, 9] {
                {_, _, _, _, _, _, _, _, _},
                {_, _, _, _, _, _, _, _, _},
                {_, _, _, _, _, _, _, _, _},
                {_, _, _, _, _, _, _, _, _},
                {_, _, _, _, _, _, _, _, _},
                {_, _, _, _, _, _, _, _, _},
                {_, _, _, _, _, _, _, _, _},
                {_, _, _, _, _, _, _, _, _},
                {_, _, _, _, _, _, _, _, _}
            });

            //Act & Assert
            Assert.Throws<IAmStuckException>(() => model.SolvePuzzle());

        }

        #region Test Case Sources
        private static object[] OnePossibleAnswerForRow =
        {
            new object[] { new int?[9,9] {
                {1, 2, 3, 4, _, 6, _, 8, 9},
                {_, _, _, _, _, _, _, _, _},
                {_, _, _, _, _, _, _, _, _},
                {_, _, _, _, _, _, _, _, _},
                {_, _, _, _, _, _, _, _, _},
                {_, _, _, _, _, _, _, _, _},
                {_, _, _, _, 7, _, _, _, _},
                {_, _, _, _, _, _, _, _, _},
                {_, _, _, _, _, _, _, _, _}
            }, 0, 6, 7 }
        };
        private static object[] OnePossibleAnswerForColumn =
        {
            new object[] { new int?[9,9] {
                {1, _, _, _, _, _, _, _, _},
                {_, _, _, _, 5, _, _, _, _},
                {3, _, _, _, _, _, _, _, _},
                {4, _, _, _, _, _, _, _, _},
                {_, _, _, _, _, _, _, _, _},
                {6, _, _, _, _, _, _, _, _},
                {7, _, _, _, _, _, _, _, _},
                {8, _, _, _, _, _, _, _, _},
                {9, _, _, _, _, _, _, _, _}
            }, 4, 0, 5 }
        };
        static object[] ThirdRemainingColumnUseOppositeDimensionAnswerCases = 
        {
            new object[] { new int?[9,9] {
                {1, _, _, _, _, _, _, _, _},
                {_, _, _, _, _, _, _, _, _},
                {_, _, _, _, _, _, _, _, _},
                {_, _, 1, _, _, _, _, _, _},
                {_, _, _, _, _, _, _, _, _},
                {_, _, _, _, _, _, _, _, _},
                {_, _, _, 1, _, _, _, _, _},
                {_, _, _, _, _, _, _, _, _},
                {_, _, _, _, _, _, 1, _, _}
            }, 7, 1, 1 },
            new object[] { new int?[9,9] {
                {_, _, _, 9, _, _, _, _, _},
                {_, _, _, _, _, _, _, _, _},
                {_, _, _, _, _, _, _, _, _},
                {_, _, _, _, _, _, _, _, 9},
                {9, _, _, _, _, _, _, _, _},
                {_, _, _, _, _, _, _, _, _},
                {_, _, _, _, _, _, _, _, _},
                {_, _, _, _, _, _, _, _, _},
                {_, _, _, _, _, 9, _, _, _}
            }, 5, 4, 9 }
        };
        static object[] ThirdRemainingColumnWithOneOptionAnswerCases = 
        {
            new object[] { new int?[9,9] {
                {1, _, _, _, _, _, _, _, _},
                {_, _, _, _, _, _, _, _, _},
                {_, _, _, _, _, _, _, _, _},
                {_, 1, _, _, _, _, _, _, _},
                {_, _, _, _, _, _, _, _, _},
                {_, _, _, _, _, _, _, _, _},
                {_, _, _, _, _, _, _, _, _},
                {_, _, 2, _, _, _, _, _, _},
                {_, _, 3, _, _, _, _, _, _}
            }, 6, 2, 1 },
            new object[] { new int?[9,9] {
                {_, _, _, _, _, _, _, _, 1},
                {_, _, _, _, _, 1, _, _, _},
                {2, _, 3, _, _, _, _, _, _},
                {_, _, _, _, _, _, _, _, _},
                {_, _, _, _, _, _, _, _, _},
                {_, _, _, _, _, _, _, _, _},
                {_, _, _, _, _, _, _, _, _},
                {_, _, _, _, _, _, _, _, _},
                {_, _, _, _, _, _, _, _, _}
            }, 2, 1, 1 }
        };
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
                {_, 2, _, _, _, _, _, _, _},
                {_, 3, _, _, _, _, _, _, _},
                {_, 4, _, _, _, _, _, _, _},
                {_, 5, _, _, _, _, _, _, _},
                {_, 6, _, _, _, _, _, _, _},
                {_, 7, _, _, _, _, _, _, _},
                {_, 8, _, _, _, _, _, _, _},
                {_, 9, _, _, _, _, _, _, _}
            }, 0, 1, 1 },
            new object[] { new int?[9,9] {
                {1, 2, 3, _, _, _, _, _, _},
                {4, _, 6, _, _, _, _, _, _},
                {7, 8, 9, _, _, _, _, _, _},
                {_, _, _, _, _, _, _, _, _},
                {_, _, _, _, _, _, _, _, _},
                {_, _, _, _, _, _, _, _, _},
                {_, _, _, _, _, _, _, _, _},
                {_, _, _, _, _, _, _, _, _},
                {_, _, _, _, _, _, _, _, _}
            }, 1, 1, 5 },
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
        #endregion

    }
}
