using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleTableExt;

namespace CodingTracker
{
    public class TableVisualizationEngine
    {
        public static void PrintInTableFormat(List<CodingSession> list)
        {
            //Header has no divider.
            ConsoleTableBuilder.From(list)
                .WithCharMapDefinition(CharMapDefinition.FramePipDefinition)
                .WithCharMapDefinition(
                    CharMapDefinition.FramePipDefinition,
                    new Dictionary<HeaderCharMapPositions, char> {
                    {HeaderCharMapPositions.TopLeft, '╒' },
                    {HeaderCharMapPositions.TopCenter, '═' },
                    { HeaderCharMapPositions.TopRight, '╕' },
                    { HeaderCharMapPositions.BottomLeft, '╞' },
                    { HeaderCharMapPositions.BottomCenter, '╤' },
                    { HeaderCharMapPositions.BottomRight, '╡' },
                    { HeaderCharMapPositions.BorderTop, '═' },
                    { HeaderCharMapPositions.BorderRight, '│' },
                    { HeaderCharMapPositions.BorderBottom, '═' },
                    { HeaderCharMapPositions.BorderLeft, '│' },
                    { HeaderCharMapPositions.Divider, ' ' },
                    })
                .ExportAndWriteLine();
        }

    }
}
