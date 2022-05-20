using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharky;

public class DebugServiceCopy : DebugService
{


public HashSet<Point2D> allBlockedPaths = new HashSet<Point2D>();

    public DebugServiceCopy(SharkyOptions sharkyOptions) : base(sharkyOptions)
    {
        
    }
    public void drawBlockedPaths()
    {
        foreach (Point2D p in allBlockedPaths)
        {
            DrawSphere(new Point { X = p.X, Y = p.Y, Z = 12 }, 0.1f, new Color { R = 255, G = 0, B = 0 });
        }
    }

    public void DrawTextWorld(string text, uint size, float x, float y, float z = 12)
    {
        DrawRequest.Debug.Debug[0].Draw.Text.Add(new DebugText() { Text = text, Size = size, WorldPos = new Point() { X = x, Y = y, Z = z } });
    }


}





