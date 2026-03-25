using BlogCore.DAL.Models;
using Bogus;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.Tests.Integration
{
    public static class DataGenerator
    {
        public static Faker<Post> GetPostFaker() =>
            new Faker<Post>()
            //.RuleFor(p => p.Id, f => f.UniqueIndex)
            .RuleFor(p => p.Author, f => f.Name.FullName())
            .RuleFor(p => p.Content, f => f.Lorem.Paragraphs(1));

        public static Faker<Comment> GetCommentFaker(int postId) =>
            new Faker<Comment>()
            //.RuleFor(c => c.Id, f => f.UniqueIndex)
            .RuleFor(c => c.PostId, _ => postId)
            .RuleFor(c => c.Content, f => f.Lorem.Sentence());
    }
}
