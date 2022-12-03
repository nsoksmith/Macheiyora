// ========== ========== ==========
//   Sparkle.cginc
//      Author : にしおかすみす！
//      Twitter : @nsokSMITHdayo
// ========== ========== ==========

float rand2dTo1d(float2 value, float2 dotDir = float2(12.9898, 78.233))
{
    float2 smallValue = sin(value);
    float random = dot(smallValue, dotDir);
    random = frac(sin(random) * 143758.5453);
    return random;
}

float2 rand2dTo2d(float2 value)
{
    return float2(
        rand2dTo1d(value, float2(12.989, 78.233)),
        rand2dTo1d(value, float2(39.346, 11.135))
    );
}

float rand1dTo1d(float3 value, float mutator = 0.546)
{
    float random = frac(sin(value + mutator) * 143758.5453);
    return random;
}

float3 rand1dTo3d(float value)
{
    return float3(
        rand1dTo1d(value, 3.9812),
        rand1dTo1d(value, 7.1536),
        rand1dTo1d(value, 5.7241)
    );
}

float3 voronoiNoise(float2 value)
{
    float2 baseCell = floor(value);
    float minDistToCell = 10;
    float2 toClosestCell;
    float2 closestCell;
    [unroll]
    for (int x1 = -1; x1 <= 1; x1++)
    {
        [unroll]
        for (int y1 = -1; y1 <= 1; y1++)
        {
            float2 cell = baseCell + float2(x1, y1);
            float2 cellPosition = cell + rand2dTo2d(cell);
            float2 toCell = cellPosition - value;
            float distToCell = length(toCell);
            if (distToCell < minDistToCell)
            {
                minDistToCell = distToCell;
                closestCell = cell;
                toClosestCell = toCell;
            }
        }
    }
    float minEdgeDistance = 10;
    [unroll]
    for (int x2 = -1; x2 <= 1; x2++)
    {
        [unroll]
        for (int y2 = -1; y2 <= 1; y2++)
        {
            float2 cell = baseCell + float2(x2, y2);
            float2 cellPosition = cell + rand2dTo2d(cell);
            float2 toCell = cellPosition - value;

            float2 diffToClosestCell = abs(closestCell - cell);
            bool isClosestCell = diffToClosestCell.x + diffToClosestCell.y < 0.1;
            if (!isClosestCell)
            {
                float2 toCenter = (toClosestCell + toCell) * 0.5;
                float2 cellDifference = normalize(toCell - toClosestCell);
                float edgeDistance = dot(toCenter, cellDifference);
                minEdgeDistance = min(minEdgeDistance, edgeDistance);
            }
        }
    }
    float random = rand2dTo1d(closestCell);
    return float3(minDistToCell, random, minEdgeDistance);
}

float3 HSVtoRGB(float3 hsv)
{
    float3 rgb = clamp(abs(fmod(hsv.x * 6 + float3(0, 4, 2), 6) - 3) - 1, 0, 1);
    return hsv.z * lerp(float3(1, 1, 1), rgb, hsv.y);
}