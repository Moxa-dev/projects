using System;
using System.Collections.Generic;

namespace func_rocket;

public class LevelsTask
{
    static readonly Physics standardPhysics = new();

    public static IEnumerable<Level> CreateLevels()
    {
        var rocket = new Rocket(new Vector(200, 500), Vector.Zero, -0.5 * Math.PI);
        var target = new Vector(600, 200);

        yield return CreateZeroLevel(rocket, target);
        yield return CreateHeavyLevel(rocket, target);
        yield return CreateUpLevel(rocket);
        yield return CreateWhiteHoleLevel(rocket, target);
        yield return CreateBlackHoleLevel(rocket, target);
        yield return CreateBlackAndWhiteLevel(rocket, target);
    }

    private static Level CreateZeroLevel(Rocket rocket, Vector target)
    {
        return new Level("Zero", rocket, target, (size, v) => Vector.Zero, standardPhysics);
    }

    private static Level CreateHeavyLevel(Rocket rocket, Vector target)
    {
        return new Level("Heavy", rocket, target, (size, v) => new Vector(0, 0.9), standardPhysics);
    }

    private static Level CreateUpLevel(Rocket rocket)
    {
        var upTarget = new Vector(700, 500);
        return new Level(
            "Up", 
            rocket, 
            upTarget,
            (size, v) => new Vector(
                0, 
                -300.0 / (size.Y - v.Y + 300.0)
            ),
            standardPhysics
        );
    }

    private static Level CreateWhiteHoleLevel(Rocket rocket, Vector target)
    {
        return new Level(
            "WhiteHole", 
            rocket, 
            target,
            (size, v) => CalculateHoleGravity(
                v, 
                target, 
                -140
            ),
            standardPhysics
        );
    }

    private static Level CreateBlackHoleLevel(Rocket rocket, Vector target)
    {
        return new Level(
            "BlackHole", 
            rocket, 
            target,
            (size, v) => CalculateHoleGravity(
                v, 
                GetAnomalyPosition(rocket, target), 
                300
            ),
            standardPhysics
        );
    }

    private static Level CreateBlackAndWhiteLevel(Rocket rocket, Vector target)
    {
        return new Level(
            "BlackAndWhite", 
            rocket, 
            target,
            (size, v) =>
            {
                var whiteForce = CalculateHoleGravity(
                    v, 
                    target, 
                    -140
                );
                var blackForce = CalculateHoleGravity(
                    v, 
                    GetAnomalyPosition(rocket, target), 
                    300
                );
                return (whiteForce + blackForce) * 0.5;
            },
            standardPhysics
        );
    }

    private static Vector CalculateHoleGravity(Vector position, Vector center, double forceMultiplier)
    {
        var diff = center - position;
        var distance = diff.Length;
        if (distance == 0) return Vector.Zero;
        return diff.Normalize() * forceMultiplier * distance / (distance * distance + 1);
    }

    private static Vector GetAnomalyPosition(Rocket rocket, Vector target)
    {
        return (rocket.Location + target) * 0.5;
    }
}