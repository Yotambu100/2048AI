using OmegaProjectGame;
using System;
using System.IO;

namespace DefaultNamespace.GameScripts
{
    /// <summary>
    /// objact that responsible of writing the board at losing state in file
    /// </summary>
    public class FileResults
    {
        
        /// <summary>
        /// constructor
        /// </summary>
        public FileResults()
        {
        }

        /// <summary>
        /// function that gets a board and write it in a file
        /// </summary>
        /// <param name="board">the board to be written</param>
        public void WriteBoardResultOnFile(Board board)
        {
            
            //the string to be writtin in the file
            string[] lines=new String[20];
            int counter = 0;
            
            // for every tile
            for (int Row = 0; Row < Board.ColumnLength; Row++)
            {
                for (int Column = 0; Column < Board.ColumnLength; Column++)
                {
                    //add the tile to the string
                    lines[counter] = Math.Pow(2, board[Row, Column]).ToString()+" ";
                    counter++;
                }
        
                lines[counter] = "\n";
                counter++;
            }

            // the file path
            string docPath ="/Users/yotambuhnik/Documents/2048Result";
            
            
            // Write the string array to the file
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath, "Results.txt")))
            {
                foreach (string line in lines)
                    outputFile.Write(line);
                
                outputFile.WriteLine("-------------------");
                outputFile.WriteLine("");

            }
        }
    }
}