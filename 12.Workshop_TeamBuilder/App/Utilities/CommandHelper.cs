namespace App.Utilities
{
    using Data;
    using Models;
    using System.Linq;

    public class CommandHelper
    {
        public static bool IsTeamExisting(string teamName)
        {
            using (var context = new TeamBuilderContext())
            {
                return context.Teams.Any(t => t.Name == teamName);
            }
        }

        public static bool IsUserExisting(string userName)
        {
            using (var context = new TeamBuilderContext())
            {
                return context.Users.Any(u => u.UserName == userName);
            }
        }

        public static bool IsUserDeleted(string userName)
        {
            using (var context = new TeamBuilderContext())
            {
                return context.Users.Any(u => u.UserName == userName && u.IsDeleted);
            }
        }

        public static bool IsInvitationExisting(string teamName, User user)
        {
            using (var context = new TeamBuilderContext())
            {
                return context.Invitations.Any(i => i.Team.Name == teamName && i.InvitedUserId == user.Id);
            }
        }

        public static bool IsInvitationExistingAndIsActive(string teamName, User user)
        {
            using (var context = new TeamBuilderContext())
            {
                return context.Invitations.Any(i => i.Team.Name == teamName && i.InvitedUserId == user.Id && i.IsActive);
            }
        }

        public static bool IsUserCreatorOfTeam(string teamName, User user)
        {
            using (var context = new TeamBuilderContext())
            {
                return context.Teams.Any(t => t.Name == teamName && t.CreatorId == user.Id);
            }
        }

        public static bool IsUserCreatorOfEvent(string eventName, User user)
        {
            using (var context = new TeamBuilderContext())
            {
                return context.Events.Any(e => e.Name == eventName && e.CreatorId == user.Id);
            }
        }

        public static bool IsTeamAddedToEvent(string eventName, string teamName)
        {
            using (var context = new TeamBuilderContext())
            {
                return context.Events.Any(e =>
                    e.Name == eventName && e.ParticipatingTeams.Any(t => t.Team.Name == teamName));
            }
        }

        public static bool IsMemberOfTeam(string teamName, string username)
        {
            using (var context = new TeamBuilderContext())
            {
                return context.Teams.Single(t => t.Name == teamName)
                    .Memebers.Any(ut => ut.User.UserName == username);
            }
        }

        public static bool IsEventExisting(string eventName)
        {
            using (var context = new TeamBuilderContext())
            {
                return context.Events.Any(e => e.Name == eventName);
            }
        }

        public static User GetUser(string userName)
        {
            using (var context = new TeamBuilderContext())
            {
                return context.Users.FirstOrDefault(u => u.UserName == userName);
            }
        }
    }
}