namespace App.Utilities
{
    public static class Constants
    {
        public const string DateTimeFormat = "dd/MM/yyyy HH:mm";

        public static class ErrorMessages
        {
            // Common error messages.
            public const string InvalidArgumentsCount = "Invalid arguments count!";

            public const string LogoutFirst = "You should logout first!";
            public const string LoginFirst = "You should login first!";

            public const string TeamExists = "Team {0} exists!";
            public const string TeamOrUserNotExist = "Team or user does not exist!";

            public const string InviteIsAlreadySent = "Invite is already sent!";
            public const string NotPartOfTeam = "User {0} is not a member in {1}!";

            public const string UserNotValid = "User details not valid!";
            public const string EventNotValid = "Event details not valid!";
            public const string TeamNotValid = "Team details not valid!";

            public const string TeamNotFound = "Team {0} not found!";
            public const string UserNotFound = "User {0} not found!";
            public const string EventNotFound = "Event {0} not found!";
            public const string InviteNotFound = "Invite from {0} is not found!";

            public const string NotAllowed = "Not allowed!";
            public const string CommandNotValid = "Command {0} not supported!";
            public const string CommandNotAllowed = "Command not allowed. Use DisbandTeam instead.";
            public const string CannotAddSameTeamTwice = "Cannot add same team twice!";

            // User error messages.
            public const string PasswordDoesNotMatch = "Passwords do not match!";

            public const string GenderNotValid = "Gender should be either “Male” or “Female”!";
            public const string UsernameIsTaken = "Username {0} is already taken!";
            public const string UserOrPasswordIsInvalid = "Invalid username or password!";

            public const string InvalidDateFormat = "Please insert the dates in format: [dd/MM/yyyy HH:mm]!";
            public const string StartDateAfterEndDate = "Start date should be before end date.";
        }

        public static class SuccessMessages
        {
            public const string Register = "User {0} was registered successfully!";
            public const string Reregister = "User {0} was registered again successfully!";
            public const string Login = "User {0} successfully logged in!";
            public const string Logout = "User {0} successfully logged out!";
            public const string Delete = "User {0} was deleted successfully!";
            public const string Event = "Event {0} was created successfully!";
            public const string Team = "Team {0} successfully created!";
            public const string Invite = "Team {0} invited {1}!";
            public const string Accept = "User {0} joined team {1}!";
            public const string Decline = "Invite from {0} declined.";
            public const string Kick = "User {0} was kicked from {1}!";
            public const string Disband = "{0} has disbanded!";
            public const string AddTeamTo = "Team {0} added for {1}!";
        }
    }
}