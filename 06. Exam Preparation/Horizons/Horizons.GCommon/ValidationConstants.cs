namespace Horizons.GCommon
{
	public static class ValidationConstants
	{
		public static class Destination
		{
			public const byte NameMinLength = 3;
			public const byte NameMaxLength = 80;

			public const byte DescriptionMinLength = 10;
			public const byte DescriptionMaxLength = 250;

			public const string DateTimeFormat = "dd-MM-yyyy";
		}

		public static class Terrain
		{
			public const byte NameMinLength = 3;
			public const byte NameMaxLength = 20;
		}
	}
}
