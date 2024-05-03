using System;

public static class Utils {
    static readonly Random random = new Random();
    public static float RandomFloat(float scale) {
        return random.NextSingle() * scale;
    }

    public static float RandomOffset(float ratio) {
        // Ratio guide table:
        // 0.0  = 0 deg
        // 0.05 = 2,86 deg
        // 0.1  = 5,71 deg
        // 0.15 = 8,53 deg
        // 0.2  = 11,31 deg
        // 0.25 = 14 deg

        // 0.5  = 26,57 deg
        // 0.75 = 36,87 deg
        // 1.0  = 45 deg

        return (random.NextSingle() - random.NextSingle()) * ratio;
    }
}