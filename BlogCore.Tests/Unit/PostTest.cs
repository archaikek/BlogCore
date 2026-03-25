using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlogCore.DAL.Models;
using BlogCore.Tests.Integration;
using Bogus;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Org.BouncyCastle.Asn1.X509;

namespace BlogCore.Tests.Unit
{
	[TestClass]
	public class PostTest : IntegrationTestBase
	{
		[TestMethod]
		public async Task GetAllPosts_EmptyDb_ReturnsZero()
		{
			// Arrange:

			// Act:
			var posts = _repo.GetAllPosts();

			// Assert:
			Assert.IsTrue(posts.IsNullOrEmpty() && posts is not null);
		}

		[TestMethod]
		public async Task AddPost_LongContent_SavesCorrectly()
		{
			// Arrange:
			var longPost = DataGenerator.GetPostFaker().RuleFor(p => p.Content, f => f.Lorem.Paragraphs(5)).Generate();
			_ctx.Add(longPost);
			await _ctx.SaveChangesAsync();

			// Act:
			var post = _repo.GetAllPosts().Where(p => p.Id == longPost.Id).FirstOrDefault();

			// Assert:
			Assert.AreEqual(longPost.Content, post.Content);
		}
		[TestMethod]
		public async Task AddPost_SpecialCharactersInAuthor_SavesCorrectly()
		{
			// Arrange:
			var newPost = DataGenerator.GetPostFaker().Generate();
			newPost.Author = "Zażółć Gęślą Jaźń 123!";
			_ctx.Add(newPost);
			await _ctx.SaveChangesAsync();

			// Act:
			var post = _repo.GetAllPosts().Where(p => p.Id == newPost.Id).FirstOrDefault()!;

			// Assert:
			Assert.AreEqual(newPost.Author, post.Author);
		}
	}
}
