public static class ItemHelper
{
    public static float GetScaleFromPackage(PackageSize size) => size switch
    {
        PackageSize.S => 0.4f,
        PackageSize.M => 0.7f,
        PackageSize.L => 1.0f,
        PackageSize.XL => 1.4f,
        PackageSize.XXL => 1.7f,
        _ => 1.0f
    };
}
