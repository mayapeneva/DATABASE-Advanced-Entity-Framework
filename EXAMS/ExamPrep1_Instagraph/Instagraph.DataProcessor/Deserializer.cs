using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Linq;

using Newtonsoft.Json;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

using Instagraph.Data;
using Instagraph.Models;
using DataAnotations = System.ComponentModel.DataAnnotations;

namespace Instagraph.DataProcessor
{
    using System.IO;
    using System.Xml.Serialization;
    using Castle.Core.Internal;
    using ImportDtos;

    public class Deserializer
    {
        public static string ImportPictures(InstagraphContext context, string jsonString)
        {
            var pictures = new List<Picture>();
            var result = new StringBuilder();

            var objPictures = JsonConvert.DeserializeObject<Picture[]>(jsonString);
            foreach (var picture in objPictures)
            {
                var ifPictureExists = pictures.Any(p => p.Path == picture.Path);
                if (!IsValid(picture) || ifPictureExists || picture.Size <= 0)
                {
                    result.AppendLine(OutputMessages.Error);
                    continue;
                }

                pictures.Add(picture);
                result.AppendLine(string.Format(OutputMessages.Picture, picture.Path));
            }

            context.Pictures.AddRange(pictures);
            context.SaveChanges();

            return result.ToString().Trim();
        }

        public static string ImportUsers(InstagraphContext context, string jsonString)
        {
            var users = new List<User>();
            var result = new StringBuilder();

            var dtoUsers = JsonConvert.DeserializeObject<UserDto[]>(jsonString);
            foreach (var dtoUser in dtoUsers)
            {
                var ifuserExists = users.Any(u => u.Username == dtoUser.Username);
                if (!IsValid(dtoUser) || ifuserExists)
                {
                    result.AppendLine(OutputMessages.Error);
                    continue;
                }

                var picture = context.Pictures.FirstOrDefault(p => p.Path == dtoUser.ProfilePicture);
                if (picture == null)
                {
                    result.AppendLine(OutputMessages.Error);
                    continue;
                }

                dtoUser.ProfilePictureId = picture.Id;
                users.Add(Mapper.Map<User>(dtoUser));
                result.AppendLine(string.Format(OutputMessages.User, dtoUser.Username));
            }

            context.Users.AddRange(users);
            context.SaveChanges();

            return result.ToString().Trim();
        }

        public static string ImportFollowers(InstagraphContext context, string jsonString)
        {
            var followers = new List<UserFollower>();
            var result = new StringBuilder();

            var objFollowers = JsonConvert.DeserializeObject<UserFollowerDto[]>(jsonString);
            foreach (var objFollower in objFollowers)
            {
                var user = context.Users.FirstOrDefault(u => u.Username == objFollower.User);
                var follower = context.Users.FirstOrDefault(u => u.Username == objFollower.Follower);
                if (user == null || follower == null)
                {
                    result.AppendLine(OutputMessages.Error);
                    continue;
                }

                var ifAlreadyFollowed = followers.Any(f => f.UserId == user.Id && f.FollowerId == follower.Id);
                if (ifAlreadyFollowed)
                {
                    result.AppendLine(OutputMessages.Error);
                    continue;
                }

                var userFollower = new UserFollower()
                {
                    UserId = user.Id,
                    FollowerId = follower.Id
                };
                followers.Add(userFollower);
                result.AppendLine(string.Format(OutputMessages.UserFollower, objFollower.Follower, objFollower.User));
            }

            context.UsersFollowers.AddRange(followers);
            context.SaveChanges();

            return result.ToString().Trim();
        }

        public static string ImportPosts(InstagraphContext context, string xmlString)
        {
            var result = new StringBuilder();
            var posts = new List<Post>();

            var serializer = new XmlSerializer(typeof(PostDto[]), new XmlRootAttribute("posts"));
            var dtoPosts = (PostDto[])serializer.Deserialize(new StringReader(xmlString));

            foreach (var dtoPost in dtoPosts)
            {
                var user = context.Users.FirstOrDefault(u => u.Username == dtoPost.User);
                var picture = context.Pictures.FirstOrDefault(p => p.Path == dtoPost.Picture);
                if (!IsValid(dtoPost) || user == null || picture == null)
                {
                    result.AppendLine(OutputMessages.Error);
                    continue;
                }

                dtoPost.UserId = user.Id;
                dtoPost.PictureId = picture.Id;
                var post = Mapper.Map<Post>(dtoPost);
                posts.Add(post);
                result.AppendLine(string.Format(OutputMessages.Post, post.Caption));
            }

            context.Posts.AddRange(posts);
            context.SaveChanges();

            return result.ToString().Trim();
        }

        public static string ImportComments(InstagraphContext context, string xmlString)
        {
            var result = new StringBuilder();
            var comments = new List<Comment>();

            var serializer = new XmlSerializer(typeof(CommentDto[]), new XmlRootAttribute("comments"));
            var dtoComments = (CommentDto[])serializer.Deserialize(new StringReader(xmlString));

            foreach (var dtoComment in dtoComments)
            {
                var user = context.Users.FirstOrDefault(u => u.Username == dtoComment.User);
                var post = dtoComment.PostElement == null ? null : context.Posts.FirstOrDefault(p => p.Id == dtoComment.PostElement.PostId);
                if (!IsValid(dtoComment) || user == null || post == null)
                {
                    result.AppendLine(OutputMessages.Error);
                    continue;
                }

                dtoComment.UserId = user.Id;
                var comment = Mapper.Map<Comment>(dtoComment);
                comments.Add(comment);
                result.AppendLine(string.Format(OutputMessages.Comment, comment.Content));
            }

            context.Comments.AddRange(comments);
            context.SaveChanges();
            return result.ToString().Trim();
        }

        public static bool IsValid(object obj)
        {
            var validationContext = new DataAnotations.ValidationContext(obj);
            var validationResults = new List<DataAnotations.ValidationResult>();

            return DataAnotations.Validator.TryValidateObject(obj, validationContext, validationResults, true);
        }
    }
}