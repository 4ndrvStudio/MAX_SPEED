namespace TurnTheGameOn.ArcadeRacer
{
    public enum CameraType
    {
        ChaseCamera,
        HelmetCamera
    }

    public enum CarDriveType
    {
        FrontWheelDrive,
        RearWheelDrive,
        FourWheelDrive
    }

    public enum SpeedType
    {
        MPH,
        KPH
    }

    public enum MobileSteeringType
    {
        UIButtons,
        Tilt,
        UIJoystick,
        UISteeringWheel
    }

    public enum AvatarInputType
    {
        Player,
        AI,
        RCC,
        EVP
    }

    public enum UIType
    {
        None,
        Standalone,
        Mobile
    }

    public enum TargetSide
    {
        Left,
        Right,
        None
    }

    public enum SteeringTargets
    {
        Two,
        All
    }

    public enum SeriesType
    {
        Arcade,
        Championship
    }
}