# Sudoku Sharp
A code exercise to create a Sudoku Solver using .NET

A Sudoku puzzle is grid a grid of 9 by 9 grid filled with the numbers 1 to 9. Each number should be used exactly once in each row, column and 3 by 3 group. To start a Sudoku puzzle has numbers in some boxes such that there is exactly one solution to the puzzle.

Depending on the starting numbers a sudoku puzzle can be graded from easy to difficult, or tofu to sumo.

# This exercise
This .NET project contains a simple WPF App that displays a Sudoku. There are two buttons; new and solve. You need to implement the solve function and you may want to make new display more than one Sudoku puzzle.

# Developing the Solution
Team: @naeemsarfraz @foz1284

The solution utilises the following strategies to solve the Sudoku puzzle:
* For every empty cell calculate the possible numbers
* Complete any cell where there is only one possible number
* Complete any 3 x 3 group (I call it a block in code) where there is only one possible answer
* Complete any rows where there is only one possible answer
* Rotate the grid and complete any rows where there is only one possible answer and rotate the grid back to it's original orientation. This in effect checks each column to see we can complete them.
* Each time we complete a cell all the possible values are recalculated

# Possible next steps

* [x] Implement a check method that checks to see if a Sudoku solution is correct.
* [x] Implement a solve method that takes a starter sudoku and solves it.
* [ ] Hover over cell to see possible numbers for that cell
* [ ] Add Solve Cell button to solve only one cell, keep clicking it to complete the grid
* [ ] Allow user to input numbers
* [ ] Generate new random puzzles
* [ ] Generate puzzles of differing levels of difficulty
