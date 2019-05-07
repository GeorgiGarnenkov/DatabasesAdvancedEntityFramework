﻿using System;
using System.Linq;
using TeamBuilder.App.Utilities;
using TeamBuilder.Data;

namespace TeamBuilder.App.Core.Command
{
    public class KickMemberCommand : ICommand
    {
        public string Execute(string[] commandArgs)
        {
            Check.CheckLength(2, commandArgs);
            AuthenticationManager.Authorize();

            var user = AuthenticationManager.GetCurrentUser();
            string teamName = commandArgs[0];
            string username = commandArgs[1];

            if (!CommandHelper.IsUserCreatorOfTeam(teamName, user))
            {
                throw new InvalidOperationException(Constants.ErrorMessages.NotAllowed);
            }

            if (!CommandHelper.IsTeamExisting(teamName))
            {
                throw new ArgumentException(String.Format(Constants.ErrorMessages.TeamNotFound, teamName));
            }

            if (!CommandHelper.IsUserExisting(username))
            {
                throw new ArgumentException(String.Format(Constants.ErrorMessages.UserNotFound, username));
            }

            if (!CommandHelper.IsMemberOfTeam(teamName, username))
            {
                throw new ArgumentException(String.Format(Constants.ErrorMessages.NotPartOfTeam, username, teamName));
            }

            if (user.Username == username)
            {
                throw new InvalidOperationException(string.Format(Constants.ErrorMessages.CommandNotAllowed));
            }

            using (var context = new TeamBuilderContext())
            {
                var userToBeKicked = context.Users.SingleOrDefault(u => u.Username == username);
                var team = context.Teams.SingleOrDefault(t => t.Name == teamName);
                var userTeam = context.UserTeams.SingleOrDefault(ut => ut.Team == team && ut.User == userToBeKicked);

                context.UserTeams.Remove(userTeam);
                context.SaveChanges();
            }

            return string.Format(Constants.SuccessMessages.SuccessfullyKickedMember, username, teamName);
        }
    }
}