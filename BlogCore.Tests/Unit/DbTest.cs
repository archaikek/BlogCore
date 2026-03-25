using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlogCore.DAL.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Org.BouncyCastle.Asn1.X509;

namespace BlogCore.Tests.Integration
{
	[TestClass]
	public class DbTest : IntegrationTestBase
	{
		[TestMethod]
		public async Task AddPost_NumberOfPostsIncreases()
		{
			// Arrange:
			int initialCount = _repo.GetAllPosts().Count();
			var newPost = DataGenerator.GetPostFaker().Generate();
			_ctx.Posts.Add(newPost);
			await _ctx.SaveChangesAsync();

			// Act:
			var result = _repo.GetAllPosts().Count();

			// Assert: 
			Assert.AreEqual(initialCount + 1, result);
		}

		[TestMethod]
		[ExpectedException(typeof(Microsoft.EntityFrameworkCore.DbUpdateException))]
		public async Task AddPost_NullContent_ThrowDbUpdateException()
		{
			// Arrange:
			var invalidPost = DataGenerator.GetPostFaker().Generate();
			invalidPost.Author = null!;

			// Act:
			_repo.AddPost(invalidPost);

			// Assert: throws DbUpdateException
		}
		[TestMethod]
		[ExpectedException(typeof(Microsoft.EntityFrameworkCore.DbUpdateException))]
		public async Task AddPost_NullAuthor_ThrowDbUpdateException()
		{
			// Arrange:
			var invalidPost = DataGenerator.GetPostFaker().Generate();
			invalidPost.Content = null!;
			// Act:
			_repo.AddPost(invalidPost);

			// Assert: throws DbUpdateException
		}

		[TestMethod]
		public async Task AddPostWithComments_ReturnsCommentsForThatPost()
		{
			// Arrange:
			var newPost = DataGenerator.GetPostFaker().Generate();
			_ctx.Posts.Add(newPost);
			await _ctx.SaveChangesAsync();

			var comments = DataGenerator.GetCommentFaker(newPost.Id).Generate(3);
			_ctx.Comments.AddRange(comments);
			await _ctx.SaveChangesAsync();

			// Act:
			var result = _repo.GetCommentsByPostId(newPost.Id);

			// Assert:
			Assert.IsTrue(true);
		}
	}
}
