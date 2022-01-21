using System;

namespace OmegaProjectGame
{
    
    
    /// <summary>
    /// class that responsible of giving a tile value randomly according to the game rules
    /// </summary>
    public class TileValueGenerator
    {
        private static Random randomizer = new Random();
        
        
        /// <summary>
        /// function that responsible of giving a tile value randomly according to the game rules
        /// </summary>
        /// <returns>the tile value</returns>
        public static int GetTileNumber()
        {
            //generate a random number
            int tilePercent = randomizer.Next(1, 101);
            
            if (tilePercent <= 10)
            {
                //there is 10 percent chance that a tile will spawn as a four 
                return 2;
            }

            if (tilePercent <= 100)
            {
                //there is 90 percent chance that a tile will spawn as a two 
                return 1;
            }

            return -1;
        }
    }
}