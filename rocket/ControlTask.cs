using System;

namespace func_rocket;

public class ControlTask
{
    public static Turn ControlRocket(Rocket rocket, Vector target)
    {
        // Calculate the vector from rocket to target
        var directionToTarget = target - rocket.Location;
        
        // Calculate the desired angle considering velocity and gravity
        var distanceToTarget = directionToTarget.Length;
        var velocityAngle = rocket.Velocity.Length > 1e-3 ? rocket.Velocity.Angle : directionToTarget.Angle;
        var gravityCompensation = Math.PI / 3 * Math.Min(1, directionToTarget.Length / 100);
        
        // Adjust target angle considering velocity and gravity
        var angleToTarget = directionToTarget.Angle + velocityAngle * 0.3;
        angleToTarget -= gravityCompensation * Math.Sign(rocket.Velocity.Y + 1);
        
        // Normalize rocket's direction to be between -π and π
        var normalizedDirection = rocket.Direction;
        while (normalizedDirection > Math.PI) normalizedDirection -= 2 * Math.PI;
        while (normalizedDirection < -Math.PI) normalizedDirection += 2 * Math.PI;
        
        // Calculate the angle difference
        var angleDiff = angleToTarget - normalizedDirection;
        
        // Normalize the difference to be between -π and π
        while (angleDiff > Math.PI) angleDiff -= 2 * Math.PI;
        while (angleDiff < -Math.PI) angleDiff += 2 * Math.PI;
        
        // Determine turn direction based on the shortest path
        if (angleDiff > 0.1) return Turn.Right;
        if (angleDiff < -0.1) return Turn.Left;
        return Turn.None;
    }
}