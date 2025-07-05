namespace RecipeSharingPlatform.GCommon;

public static class ValidationConstants
{
    public static class Recipe
    {
        public const byte TitleMinLength = 3;
        public const byte TitleMaxLength = 80;

        public const byte InstructionsMinLength = 10;
        public const byte InstructionsMaxLength = 250;

        public const string DateTimeFormat = "dd-MM-yyyy";
    }

    public static class Category
    {
        public const byte NameMinLength = 3;
        public const byte NameMaxLength = 20;
    }
}