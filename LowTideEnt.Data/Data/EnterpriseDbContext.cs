using LowTideEnt.Domain.Entities.Admin;
using Npgsql;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Text.Json;

namespace LowTideEnt.Infrastructure.Data
{
    public class EnterpriseDbContext(DbContextOptions<EnterpriseDbContext> options) : DbContext(options)
    {

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var entityTypes = typeof(BaseEntity).Assembly.GetTypes()
                .Where(t => typeof(BaseEntity).IsAssignableFrom(t)
                    && t != typeof(BaseEntity)
                    && !t.IsAbstract
                    && !t.IsInterface);

            foreach (var type in entityTypes)
            {
                modelBuilder.Entity(type);
            }


            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (!typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                {
                    continue;
                }

                modelBuilder.Entity(entityType.ClrType)
                    .Property(nameof(BaseEntity.CreatedDate))
                    .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");
            }

            // Apply configurations not inheriting BaseEntity
            modelBuilder.Entity<UserToRoleEntity>(entity => entity.HasKey(e => new { e.UserId, e.RoleId }));
        }
        /// <summary>
        /// Calls a Psql function with params.
        /// </summary>
        /// <typeparam name="T">Model to translate return to</typeparam>
        /// <param name="functionName">Name of PSQL function</param>
        /// <param name="parameters">Dict{ string: Parameter Name, object: Parameter Value }</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>A <see cref="Task{T}"/> representing the asynchronous operation, containing the instance of <typeparamref name="T"/>.</returns>

        private static Type GetEntityType(Type t)
        {
            if (t.IsArray) return t.GetElementType()!;

            var enumerableInterface = t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>)
                ? t
                : t.GetInterfaces().FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>));

            return enumerableInterface?.GetGenericArguments()[0] ?? t;
        }
        public async Task<T?> ExecutePsqlFunctionAsync<T>(string functionName, Dictionary<string, object> parameters, CancellationToken cancellationToken, string? schema = null) where T : class
        {
            if (schema == null)
            {
                var entityType = GetEntityType(typeof(T));
                schema = entityType.GetCustomAttribute<TableAttribute>()?.Schema
                    ?? throw new InvalidOperationException($"{entityType.Name} has no [Table] schema.");
            }

            var connection = (NpgsqlConnection)Database.GetDbConnection();
            var wasOpen = connection.State == ConnectionState.Open;
            if (!wasOpen) await connection.OpenAsync(cancellationToken);
            try
            {
                var functionCall = $"""SELECT "{schema}"."{functionName}"({string.Join(", ", parameters.Select(p => $"@{p.Key}"))})""";
                await using var cmd = new NpgsqlCommand(functionCall, connection);
                foreach (var parameter in parameters)
                {
                    cmd.Parameters.AddWithValue(parameter.Key, parameter.Value);
                }

                var json = (string?)await cmd.ExecuteScalarAsync(cancellationToken);

                return JsonSerializer.Deserialize<T>(json ?? "{}");
            }
            finally
            {
                if (!wasOpen) await connection.CloseAsync();
            }
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries<BaseEntity>();

            foreach (var entry in entries)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedDate = DateTime.UtcNow;
                        break;
                    case EntityState.Modified:
                        entry.Entity.ModifiedDate = DateTime.UtcNow;
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
