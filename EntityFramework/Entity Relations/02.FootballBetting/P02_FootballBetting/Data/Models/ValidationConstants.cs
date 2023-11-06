namespace P02_FootballBetting.Data.Models
{
    public class ValidationConstants
    {
        //Team
        public const int MaxTeamName = 100;
        public const int MaxUrlLength = 2048;
        public const int MaxInitialsLength = 3;

        //Color
        public const int MaxColorLength = 10;

        //Town
        public const int MaxTownNameLength = 85;

        //Country
        public const int MaxCountryNameLength = 56;
        public const int MaxPlayerNameLength = 100;

        //Position
        public const int MaxPositionNameLength = 25;

        //User
        public const int MaxUsernameLength = 150;
        public const int MaxPasswordLength = 64;
        public const int MaxEmailLength = 320;
    }
}
