
namespace OmegaProjectGame
{

    /// <summary>
    /// the class that hold the values that are in the TranspositionTable
    /// score - the score that the ai already found from the current board
    /// depth - the depth the ai calculated the original board score
    /// </summary>
    public class TranspositionValue
    {
        private double score;
        private ushort depth;
        // private ushort move;
      
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="score">the score that the ai already found from the current board</param>
        /// <param name="depth">the depth the ai calculated the original board score</param>
        public TranspositionValue(double score,ushort depth)
        {
            this.depth = depth;
            this.score = score;
        }
        public double Score { get => score; set => score = value; }
        public ushort Depth { get => depth; set => depth = value; }
    }
}