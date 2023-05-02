/*
 * Given a Sudoku data structure with size NxN, N > 0 and vN == integer, write a method to validate if it has been filled out correctly.
 * The data structure is a multi-dimensional Array, ie:

    [
      [7,8,4,  1,5,9,  3,2,6],
      [5,3,9,  6,7,2,  8,4,1],
      [6,1,2,  4,3,8,  7,5,9],

      [9,2,8,  7,1,5,  4,6,3],
      [3,5,7,  8,4,6,  1,9,2],
      [4,6,1,  9,2,3,  5,8,7],

      [8,7,6,  3,9,4,  2,1,5],
      [2,4,3,  5,6,1,  9,7,8],
      [1,9,5,  2,8,7,  6,3,4]
    ]


 * Rules for validation

 * Data structure dimension: NxN where N > 0 and vN == integer
 * Rows may only contain integers: 1..N (N included)
 * Columns may only contain integers: 1..N (N included)
 * 'Little squares' (3x3 in example above) may also only contain integers: 1..N (N included)

 * taken from http://www.codewars.com/kata/540afbe2dc9f615d5e000425/train/javascript
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuTest
{
    class SudokuPuzzleValidator
    {
        static void Main(string[] args)
        {
            int[][] goodSudoku1 = {
                new int[] {7,8,4,  1,5,9,  3,2,6},
                new int[] {5,3,9,  6,7,2,  8,4,1},
                new int[] {6,1,2,  4,3,8,  7,5,9},

                new int[] {9,2,8,  7,1,5,  4,6,3},
                new int[] {3,5,7,  8,4,6,  1,9,2},
                new int[] {4,6,1,  9,2,3,  5,8,7},

                new int[] {8,7,6,  3,9,4,  2,1,5},
                new int[] {2,4,3,  5,6,1,  9,7,8},
                new int[] {1,9,5,  2,8,7,  6,3,4}
            };


            int[][] goodSudoku2 = {
                new int[] {1,4, 2,3},
                new int[] {3,2, 4,1},

                new int[] {4,1, 3,2},
                new int[] {2,3, 1,4}
            };

            int[][] badSudoku1 =  {
                new int[] {1,2,3, 4,5,6, 7,8,9},
                new int[] {1,2,3, 4,5,6, 7,8,9},
                new int[] {1,2,3, 4,5,6, 7,8,9},

                new int[] {1,2,3, 4,5,6, 7,8,9},
                new int[] {1,2,3, 4,5,6, 7,8,9},
                new int[] {1,2,3, 4,5,6, 7,8,9},

                new int[] {1,2,3, 4,5,6, 7,8,9},
                new int[] {1,2,3, 4,5,6, 7,8,9},
                new int[] {1,2,3, 4,5,6, 7,8,9}
            };

            int[][] badSudoku2 = {
                new int[] {1,2,3,4,5},
                new int[] {1,2,3,4},
                new int[] {1,2,3,4},
                new int[] {1}
            };

            Debug.Assert(ValidateSudoku(goodSudoku1), "This is supposed to validate! It's a good sudoku!");
            Debug.Assert(ValidateSudoku(goodSudoku2), "This is supposed to validate! It's a good sudoku!");
            Debug.Assert(!ValidateSudoku(badSudoku1), "This isn't supposed to validate! It's a bad sudoku!");
            Debug.Assert(!ValidateSudoku(badSudoku2), "This isn't supposed to validate! It's a bad sudoku!");
        }

        static bool ValidateSudoku(int[][] puzzle)
        {
            // Structure validation
            if (puzzle.Length <= 0 || !double.IsInteger(Math.Sqrt(puzzle.Length)) || puzzle.Any(x => x.Length != puzzle.Length))
            {
                Console.WriteLine("Invalid structure!");
                return false;
            }

            return DataValidation(puzzle);

            bool DataValidation(int[][] puzzle)
            {
                int level = (int)Math.Sqrt(puzzle.Length);
                const int L1_COUNT = 3; // Sudoku has 3 units to validate; rows, columns, big quares. So that makes our "first dimension" of our multidimensional array
                int L2_COUNT = puzzle.Length; // We can find √N items of each unit, so we have √N arrays of rows, √N arrays of columns and √N arrays of big squares.
                int L3_COUNT = puzzle.Length; // Each row, column or big square, has √N little squares which hold the numeric value

                bool[,,] validationUnits = new bool[L1_COUNT, L2_COUNT, L3_COUNT];
                int sudokuRowIndex = 0, sudokuColumnIndex = 1, sudokuBigSquareIndex = 2;

                for (int currentRowIndex = 0; currentRowIndex < puzzle.Length; currentRowIndex++)
                {
                    for (int currentColumnIndex = 0; currentColumnIndex < puzzle.Length; currentColumnIndex++)
                    {
                        int currentSquareIndex = (currentColumnIndex / level) + ((currentRowIndex / level) * level);
                        int value = puzzle[currentRowIndex][currentColumnIndex] - 1;

                        if (value < 0 || value >= puzzle.Length) // invalid value
                        {
                            Console.WriteLine($"Value should be between 1 and {puzzle.Length}!");
                            return false;
                        }

                        if (validationUnits[sudokuRowIndex, currentRowIndex, value] || validationUnits[sudokuColumnIndex, currentColumnIndex, value] || validationUnits[sudokuBigSquareIndex, currentSquareIndex, value])  // The value has been found before on a row, column or big square
                        {
                            Console.WriteLine("Invalid data!");
                            return false;
                        }

                        // The value hasn't been found before, so we check it on each validation unit
                        validationUnits[sudokuRowIndex, currentRowIndex, value] = true;
                        validationUnits[sudokuColumnIndex, currentColumnIndex, value] = true;
                        validationUnits[sudokuBigSquareIndex, currentSquareIndex, value] = true;
                    }
                }

                Console.WriteLine("Valid sudoku!");
                return true;
            }
        }
    }
}