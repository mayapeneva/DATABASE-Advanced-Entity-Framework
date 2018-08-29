using System;

using Instagraph.Data;

namespace Instagraph.DataProcessor
{
    using System.IO;
    using System.Linq;
    using System.Xml;
    using System.Xml.Serialization;
    using AutoMapper.QueryableExtensions;
    using ExportDtos;
    using Newtonsoft.Json;
    using Formatting = Newtonsoft.Json.Formatting;

    public class Serializer
    {
        public static string ExportUncommentedPosts(InstagraphContext context)
        {
            var posts = context.Posts
                .Where(p => p.Comments.Count == 0)
                .OrderBy(p => p.Id)
                .ProjectTo<UncommentedPostDto>()
                .ToArray();

            return JsonConvert.SerializeObject(posts, Formatting.Indented);
        }

        public static string ExportPopularUsers(InstagraphContext context)
        {
            var users = context.Users
                .Where(u => u.Posts.Any(p => p.Comments.Any(c =>
                    u.Followers.Any(uf => uf.FollowerId == c.UserId))))
                .OrderBy(u => u.Id)
                .ProjectTo<PopularUserDto>()
                .ToArray();

            return JsonConvert.SerializeObject(users, Formatting.Indented);
        }

        public static string ExportCommentsOnPosts(InstagraphContext context)
        {
            var users = context.Users
                .ProjectTo<UserDataDto>()
                .OrderByDescending(u => u.MostComments)
                .ThenBy(u => u.Username)
                .ToArray();

            var serializer = new XmlSerializer(typeof(UserDataDto[]), new XmlRootAttribute("users"));
            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, users, new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty }));
                return writer.ToString();
            }
        }
    }
}