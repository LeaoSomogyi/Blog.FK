﻿using Microsoft.AspNetCore.TestHost;
using System;
using System.Linq;
using VM = Blog.FK.Web.ViewModels;

namespace Blog.FK.Test.Utils
{
    public static class TestHelper
    {
        /// <summary>
        /// Get a filled Blog.FK.Web.ViewModels.UserViewModel to Login
        /// </summary>
        /// <returns>Blog.FK.Web.ViewModels.UserViewModel</returns>
        public static VM.UserViewModel GetLoginUserViewModel()
        {
            return new VM.UserViewModel()
            {
                Email = "leaosomogyi@hotmail.com",
                Password = "12345678"
            };
        }

        /// <summary>
        /// Get a filled Blog.FK.Web.ViewModels.UserViewModel
        /// </summary>
        /// <returns>Blog.FK.Web.ViewModels.UserViewModel</returns>
        public static VM.UserViewModel GetUserViewModel()
        {
            return new VM.UserViewModel()
            {
                Name = "Test User",
                Email = "test@user.com",
                Password = "@abc123"
            };
        }

        /// <summary>
        /// Get a filled Blog.FK.Web.ViewModels.BlogPostViewModel
        /// </summary>
        /// <returns>Blog.FK.Web.ViewModels.BlogPostViewModel</returns>
        public static VM.BlogPostViewModel GetBlogPostViewModel()
        {
            return new VM.BlogPostViewModel()
            {
                Title = "Test Post",
                ShortDescription = "Some nice description",
                Content = "Some awesome content."
            };
        }

        /// <summary>
        /// Moq some action in a object
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="obj">Object</param>
        /// <param name="action">Action to be applied</param>
        /// <returns>Object with action applied</returns>
        public static T Moq<T>(this T obj, Action<T> action)
        {
            action(obj);

            return obj;
        }

        /// <summary>
        /// Generated random characters based on length informed
        /// </summary>
        /// <param name="value">Current string instance</param>
        /// <param name="length">Length of the string</param>
        /// <returns>Random string generated</returns>
        public static string GenerateRandomCharacters(this string value, int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            value = new string(Enumerable.Repeat(chars, length).Select(s => s[new Random().Next(s.Length)]).ToArray());

            return value;
        }

        /// <summary>
        /// Get service from Host ServiceCollection
        /// </summary>
        /// <typeparam name="TService">Service Required</typeparam>
        /// <param name="server">Current host server</param>
        /// <returns>The Service required</returns>
        public static TService GetService<TService>(this TestServer server) where TService : class
        {
            return server?.Host?.Services?.GetService(typeof(TService)) as TService;
        }
    }
}
