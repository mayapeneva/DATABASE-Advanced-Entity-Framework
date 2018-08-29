namespace PhotoShare.Services
{
    using AutoMapper.QueryableExtensions;
    using Contracts;
    using Data;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class UserService : IUserService
    {
        private readonly PhotoShareContext context;

        public UserService(PhotoShareContext context)
        {
            this.context = context;
        }

        public TModel ById<TModel>(int id)
        {
            return this.By<TModel>(u => u.Id == id).SingleOrDefault();
        }

        public TModel ByUsername<TModel>(string username)
        {
            return this.By<TModel>(u => u.Username == username).SingleOrDefault();
        }

        public bool Exists(int id)
        {
            return this.ById<User>(id) != null;
        }

        public bool Exists(string name)
        {
            return this.ByUsername<User>(name) != null;
        }

        private IEnumerable<TModel> By<TModel>(Func<User, bool> predicate) =>
            this.context.Users
                .Where(predicate)
                .AsQueryable()
                .ProjectTo<TModel>();

        public User Register(string username, string password, string email)
        {
            var user = new User()
            {
                Username = username,
                Password = password,
                Email = email,
                IsDeleted = false
            };

            this.context.Users.Add(user);
            this.context.SaveChanges();

            return user;
        }

        public void Delete(string username)
        {
            var user = this.context.Users.FirstOrDefault(u => u.Username == username);
            user.IsDeleted = true;
            this.context.SaveChanges();
        }

        public void LogIn(string username)
        {
            var user = this.context.Users.FirstOrDefault(u => u.Username == username);
            user.IsLogged = true;
            this.context.SaveChanges();
        }

        public string LogOut()
        {
            var user = this.context.Users.FirstOrDefault(u => u.IsLogged == true);
            user.IsLogged = false;
            this.context.SaveChanges();

            return user.Username;
        }

        public bool AnyUserLoggedIn()
        {
            return this.context.Users.Any(u => u.IsLogged == true);
        }

        public int GetLoggedInUserId()
        {
            return this.context.Users.FirstOrDefault(u => u.IsLogged).Id;
        }

        public Friendship AddFriend(int userId, int friendId)
        {
            var friendship = new Friendship()
            {
                UserId = userId,
                FriendId = friendId
            };

            this.context.Friendships.Add(friendship);
            this.context.Users.Find(userId).FriendsAdded.Add(friendship);
            this.context.SaveChanges();

            return friendship;
        }

        public Friendship AcceptFriend(int userId, int friendId)
        {
            var friendship = new Friendship()
            {
                UserId = userId,
                FriendId = friendId
            };

            this.context.Friendships.Add(friendship);
            this.context.Users.Find(userId).FriendsAdded.Add(friendship);
            this.context.SaveChanges();

            return friendship;
        }

        public void ChangePassword(int userId, string password)
        {
            this.context.Users.Find(userId).Password = password;

            this.context.SaveChanges();
        }

        public void SetBornTown(int userId, int townId)
        {
            this.context.Users.Find(userId).BornTownId = townId;

            this.context.SaveChanges();
        }

        public void SetCurrentTown(int userId, int townId)
        {
            this.context.Users.Find(userId).CurrentTownId = townId;

            this.context.SaveChanges();
        }
    }
}