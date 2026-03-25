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
    public class CommentTest : IntegrationTestBase
    {
        [TestMethod]
        public async Task AddComment_ValidData_IncreasesCountForPost()
        {
            // Arrange:
            var newPost = DataGenerator.GetPostFaker().Generate();
            _ctx.Posts.Add(newPost);
            await _ctx.SaveChangesAsync();

            var newComment = DataGenerator.GetCommentFaker(newPost.Id).Generate();
            _ctx.Comments.Add(newComment);
            await _ctx.SaveChangesAsync();

            // Act:
            var result = _repo.GetCommentsByPostId(newPost.Id).Count();

            // Assert:
            Assert.AreEqual(result, 1);
        }

        [TestMethod]
        public async Task GetCommentsByPostId_NonExistentPost_ReturnsEmpty()
        {
            // Arrange:

            // Act:
            var result = _repo.GetCommentsByPostId(2221);

            // Assert:
            Assert.IsTrue(result.IsNullOrEmpty());
        }

        [TestMethod]
        [ExpectedException(typeof(Microsoft.EntityFrameworkCore.DbUpdateException))]
        public async Task AddComment_OrphanComment_ThrowsException()
        {
            // Arrange:
            var badComment = DataGenerator.GetCommentFaker(2221).Generate();

            // Act:
            _repo.AddComment(badComment);

            // Assert:
        }

        [TestMethod]
        public async Task MultipleComments_DifferentPosts_ReturnsOnlyCorrectOnes()
        {
            // Arrange:
            var posts = DataGenerator.GetPostFaker().Generate(2);
            _ctx.Posts.AddRange(posts);
            await _ctx.SaveChangesAsync();

            var comments1 = DataGenerator.GetCommentFaker(posts[0].Id).Generate(5);
            var comments2 = DataGenerator.GetCommentFaker(posts[1].Id).Generate(2);
            _ctx.Comments.AddRange(comments1);	
            _ctx.Comments.AddRange(comments2);
            await _ctx.SaveChangesAsync();

            // Act:
            var result = _repo.GetCommentsByPostId(posts[0].Id);

            // Assert:
            Assert.IsFalse(result.Contains(comments2[0]) || result.Contains(comments2[1]));
        }
    }
}
