using Boggle;

namespace BoggleGame
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            IBogglePlayer player = new PeterTheGreat();
            //IBogglePlayer player = new MarvinTheMartian();
            player.Start();
        }
    }
}