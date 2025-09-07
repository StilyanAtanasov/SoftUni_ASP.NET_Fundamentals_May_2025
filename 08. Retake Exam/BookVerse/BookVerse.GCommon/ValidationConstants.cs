namespace BookVerse.GCommon;

public static class ValidationConstants
{
    public static class Book
    {
        public const byte TitleMinLength = 3;
        public const byte TitleMaxLength = 80;

        public const byte DescriptionMinLength = 10;
        public const byte DescriptionMaxLength = 250;

        public const string DateFormat = "dd-MM-yyyy";

        public const bool IsDeletedDefaultValue = false;
    }

    public static class Genre
    {
        public const byte NameMinLength = 3;
        public const byte NameMaxLength = 20;
    }
}