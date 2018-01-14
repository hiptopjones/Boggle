using System.Collections.Generic;

namespace Boggle
{
    public interface IBoggleRules
    {
        BoggleBoard GenerateBoard(int randomSeed = 0, int boardSize = BoggleRules.BoardSizeDefault);
        void CheckBoardSize(int boardSize);
        List<char[]> GetDiceSet(int boardSize = BoggleRules.BoardSizeDefault);
        HashSet<string> GetWordSet();
        int Score(int wordLength);
    }
}