
var TargetX = new[] { 230, 283 };
var TargetY = new[] { -107, -57 };

static bool Targets(int velocityX, int velocityY, int[] targetX, int[] targetY, out List<(int x, int y)> positions)
{
    var posX = 0;
    var posY = 0;
    positions = new() { (posX, posY) };

    while ((posX < targetX[1] && posY > targetY[1]) && !(posX >= targetX[0] && posX <= targetX[1] // TODO: refactor
                                                                           && posY >= targetX[0] && posY <= targetY[1]))
    {
        posX += velocityX;
        posY += velocityY;
        positions.Add((posX, posY));

        if (velocityX > 0)
        {
            velocityX--;
        }

        velocityY--;
    }

    return posX >= targetX[0] && posX <= targetX[1]
                              && posY >= targetY[0] && posY <= targetY[1];
}

int? highestY = null;

for (var velocityY = TargetY[0] * -1; highestY == null && velocityY >= TargetY[0]; velocityY--)
{
    for (var velocityX = 1; highestY == null && velocityX < TargetX[0]; velocityX++)
    {
        if (Targets(velocityX, velocityY, TargetX, TargetY, out var positions))
        {
            highestY = positions.Max(p => p.y);
        }
    }
}

Console.WriteLine($"Part1 result: {highestY}");
