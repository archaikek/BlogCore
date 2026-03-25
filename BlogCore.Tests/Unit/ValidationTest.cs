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
	public class ValidationTest : IntegrationTestBase
	{
		[TestMethod]
		[ExpectedException(typeof(Microsoft.EntityFrameworkCore.DbUpdateException))]
		public async Task AddPost_NullAuthor_ThrowsDbUpdateException()
		{
			// Arrange:
			var badPost = DataGenerator.GetPostFaker().Generate();
			badPost.Author = null!;

			// Act:
			_repo.AddPost(badPost);	

			// Assert: 
		}

		[TestMethod]
		[ExpectedException(typeof(Microsoft.EntityFrameworkCore.DbUpdateException))]
		public async Task AddComment_NullContent_ThrowsDbUpdateException()
		{
			// Arrange:
			var badPost = DataGenerator.GetPostFaker().Generate();
			badPost.Content = null!;

			// Act:
			_repo.AddPost(badPost);

			// Assert: 
		}
	}
}
