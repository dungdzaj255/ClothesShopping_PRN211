﻿using DAOLibrary.DataAccess;
using DAOLibrary.Repository.Interface;
using DTOLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAOLibrary.Repository.Object
{
    public class UserRepository : IUserRepository
    {
        public void DeleteUser(int userId) => UserDAO.Instance.Remove(userId);
        
        public User GetUser(string email) => UserDAO.Instance.GetUser(email);

        public User GetUserById(int id) => UserDAO.Instance.GetUserById(id);
        
        public IEnumerable<User> GetUserList() => UserDAO.Instance.GetUserList();
        
        public User Login(string email, string password)
        {
            return UserDAO.Instance.Login(email, password);
        }

        public void SetAccountStatus(int id, bool status) => UserDAO.Instance.SetAccountStatus(id, status);
        
        public void UpdateUser(User user) => UserDAO.Instance.Update(user);
        
    }
}
