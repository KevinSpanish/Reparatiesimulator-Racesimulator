using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControllerTest
{
    [TestFixture]
    public class Console_Visualization
    {
        #region graphics

        private static string[] _finishHorizontal =
            {
            "----",
            " 1|#",
            "2|# ",
            "----"
        };

        private static string[] _finishHorizontalInv =
            {
            "----",
            "#|1 ",
            " #|2",
            "----"
        };

        private static string[] _finishVertical =
            {
            "|##|",
            "|- |",
            "|1 |",
            "| 2|"
        };

        private static string[] _finishVerticalInv =
            {
            "|  |",
            "|2 |",
            "| 1|",
            "|##|"
        };

        private static string[] _startHorizontal =
            {
            "----",
            "  1|",
            " 2| ",
            "----"
        };

        private static string[] _startHorizontalInv =
            {
            "----",
            "|2  ",
            " |1 ",
            "----"
        };

        private static string[] _startVertical =
            {
            "|- |",
            "|1-|",
            "| 2|",
            "|  |"
        };

        private static string[] _startVerticalInv =
            {
            "|  |",
            "| 1|",
            "|2-|",
            "|- |"
        };

        private static string[] _straightHorizontal =
            {
            "----",
            "  1 ",
            " 2  ",
            "----"
        };

        private static string[] _straightHorizontalInv =
            {
            "----",
            "  2 ",
            " 1  ",
            "----"
        };

        private static string[] _straightVertical =
            {
            "|  |",
            "|1 |",
            "| 2|",
            "|  |"
        };

        private static string[] _straightVerticalInv =
            {
            "|  |",
            "|2 |",
            "| 1|",
            "|  |"
        };

        private static string[] _corner0 =
            {
            "|  |",
            "|1  ",
            "| 2 ",
            "----"
        };

        private static string[] _corner1 =
            {
            "----",
            "|1  ",
            "| 2 ",
            "|  |"
        };

        private static string[] _corner2 =
            {
            "----",
            "  1|",
            " 2 |",
            "|  |"
        };

        private static string[] _corner3 =
            {
            "|  |",
            " 2 |",
            "  1|",
            "---|"
        };

        #endregion

        [SetUp]
        public void SetUp()
        {
        }
    }
}
