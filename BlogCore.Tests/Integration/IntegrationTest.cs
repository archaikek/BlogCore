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
    public class IntegrationTest : IntegrationTestBase
    {
        [TestMethod]
        public async Task DeletePost_CascadeDeleteComments()
        {
            // Arrange:
            var newPost = DataGenerator.GetPostFaker().Generate();
            _ctx.Posts.Add(newPost);
            await _ctx.SaveChangesAsync();

            var newComment = DataGenerator.GetCommentFaker(newPost.Id).Generate();
            _ctx.Comments.Add(newComment);
            await _ctx.SaveChangesAsync();

            // Act:
            _repo.DeletePost(newPost);

            // Assert:
            Assert.IsFalse(_ctx.Comments.Contains(newComment));
        }
    }
}
