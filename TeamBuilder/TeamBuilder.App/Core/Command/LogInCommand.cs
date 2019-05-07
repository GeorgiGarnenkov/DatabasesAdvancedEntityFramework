﻿using System;
using System.Linq;
using TeamBuilder.App.Utilities;
using TeamBuilder.Data;
using TeamBuilder.Models;

namespace TeamBuilder.App.Core.Command
{
    public class LogInCommand : ICommand
    {
        public string Execute(string[] commandArgs)
        {
            Check.CheckLength(2, commandArgs);

            string username = commandArgs[0];
            string password = commandArgs[1];

            if (AuthenticationManager.IsAuthenticated())
            {
                throw new InvalidOperationException(Constants.ErrorMessages.LogoutFirst);
            }

            var user = this.GetUserByCredentials(username, password);

            if (user == null)
            {
                throw new ArgumentException(Constants.ErrorMessages.UserOrPasswordIsInvalid);
            }

            AuthenticationManager.Login(user);

            return string.Format(Constants.SuccessMessages.SuccessfulLogin, username);
        }

        private User GetUserByCredentials(string username, string password)
        {
            using (var context = new TeamBuilderContext())
            {
                var user = context.Users.FirstOrDefault(u => u.Username == username);

                if (user == null || user.Password != password || user.IsDeleted)
                {
                    return null;
                }

                return user;
            }
        }
    }
}