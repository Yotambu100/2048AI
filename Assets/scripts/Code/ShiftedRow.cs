
namespace OmegaProjectGame
{
    /// <summary>
    /// The class that hold information about every possible row.
    /// The class hold:
    /// newRow - the row after shifting the original row to the left(the original row is the index)
    /// numberOfEmptyTile - the number of empty tiles in the original row
    /// numberOfMergeableTile - the number mergeable tiles in the original row
    /// score - the score of the original row
    /// isWon - flag that indicate that if the new reached the game is over
    /// </summary>
    public class ShiftedRow
    {
        private ushort newRow;
        private ushort numberOfEmptyTile;
        private ushort numberOfMergeableTile;
        private uint score;
        private bool isWon;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="numberOfEmptyTile">the number of empty tiles in the original row</param>
        /// <param name="newRow">the row after shifting the original row to the left(the original row is the index)</param>
        /// <param name="numberOfMergeableTile">the number mergeable tiles in the original row</param>
        /// <param name="score">the score of the original row</param>
        /// <param name="isWon">flag that indicate that if the new reached the game is over</param>
        public ShiftedRow(ushort numberOfEmptyTile, ushort newRow,ushort numberOfMergeableTile,uint score,bool isWon)
        {
            this.numberOfEmptyTile = numberOfEmptyTile;
            this.newRow = newRow;
            this.numberOfMergeableTile = numberOfMergeableTile;
            this.score = score;
            this.isWon = isWon;
        }
        public ushort NewRow { get => newRow; set => newRow = value; }
        public ushort NumberOfEmptyTile{ get => numberOfEmptyTile; set => numberOfEmptyTile = value; }
        public ushort NumberOfMergeableTile { get => numberOfMergeableTile; set => numberOfMergeableTile = value; }
        public bool IsWon{ get => isWon; set => isWon = value; }
        public uint Score { get => score; set => score = value; }

        
    }
}