using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testcontainers.MsSql;
using BlogCore.DAL.Data;
using BlogCore.DAL.Repositories;
using Respawn;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Bogus.Extensions.UnitedKingdom;
using Respawn.Graph;
using Microsoft.EntityFrameworkCore.Storage;

namespace BlogCore.Tests.Integration
{
	[TestClass]
	public abstract class IntegrationTestBase
	{
		protected static readonly MsSqlContainer _dbContainer =
			new MsSqlBuilder("mcr.microsoft.com/mssql/server:2022-latest")
			.WithPassword("StrongPassword123!")
			.Build();
		protected BlogContext _ctx = null!;
		protected BlogRepository _repo = null!;
		protected Respawner _respawner = null!;
		//protected IDbContextTransaction _transaction = null!;

		[AssemblyInitialize]
		public static async Task AssemblyInit(TestContext ctx)
		{
			await _dbContainer.StartAsync();

			var connectionString = _dbContainer.GetConnectionString();
			var options = new DbContextOptionsBuilder<BlogContext>().UseSqlServer(connectionString).Options;
			
			using var _ctx = new BlogContext(options);
			await _ctx.Database.EnsureCreatedAsync();
		}

		[TestInitialize]
		public async Task Setup()
		{
			var connectionString = _dbContainer.GetConnectionString();

			// 2. Inicjalizacja Respawn przy użyciu AKTYWNEGO połączenia
			using (var connection = new SqlConnection(connectionString))
			{
				await connection.OpenAsync();
				_respawner ??= await Respawner.CreateAsync(connection,
					new RespawnerOptions
					{
						TablesToIgnore = new Table[]
						{
							new Table("__EFMigrationsHistory")
						}
					});
			}
			// 3. Pierwszy reset bazy
			await ResetDatabaseAsync();

			// 1. Konfiguracja EF Core
			var options = new DbContextOptionsBuilder<BlogContext>().UseSqlServer(connectionString).Options;
			_ctx = new BlogContext(options);
			_repo = new BlogRepository(_ctx);

			//_transaction = await _ctx.Database.BeginTransactionAsync();
		}
		protected async Task ResetDatabaseAsync()
		{
			if (_respawner is not null)
			{
				var connectionString = _dbContainer.GetConnectionString();
				using (var connection = new SqlConnection(connectionString))
				{
					await connection.OpenAsync();
					// Resetowanie danych przy użyciu obiektu połączenia
					await _respawner.ResetAsync(connection);
				}
			}
		}

		[TestCleanup]
		public async Task Cleanup()
		{
			//await _transaction.RollbackAsync();
			await _ctx.DisposeAsync();
		}

		[AssemblyCleanup]
		public static async Task AssemblyCleanup()
		{
			await _dbContainer.StopAsync();
		}
	}
}
