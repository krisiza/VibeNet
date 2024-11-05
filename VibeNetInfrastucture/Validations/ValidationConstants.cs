namespace VibeNetInfrastucture.Validations
{
    public static class ValidationConstants
    {
        public static class User
        {
            public const int FirstNameMaxLength = 50;
            public const int FirstNameMinLength = 2;
            public const int LastNameMaxLength = 50;
            public const int LastNameMinLength = 2;
            public const int HomeTownMaxLength = 100;
            public const int HomeTownMinLength = 2;
        }

        public static class DateTimeFormat
        {
            public const string Format = "yyyy.MM.dd";
        }

        public static class Comment
        {
            public const int ContentMaxLength = 300;
        }

        public static class Post
        {
            public const int ContentMaxLength = 400;
        }
    }
}
