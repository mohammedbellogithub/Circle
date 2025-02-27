﻿using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Circle.Shared.Dapper.Interfaces;

namespace Circle.Shared.Dapper.Repository
{
     /// <summary>
    /// Repository that uses the Dapper micro-ORM framework, see https://github.com/StackExchange/Dapper
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    public class DapperRepository<TEntity> : GenericRepositoryBase<TEntity>, IDapperRepository<TEntity>
      where TEntity : class
    {
        private IDbConnection connection;
        private DbCommand command;
        private DbProviderFactory factory;
        private DbConnection dbConnection; 
        private DbTransaction dbTransaction;
        private long totalItems = 0;
        private Task<object> totalItemsTask = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="DapperRepository{TEntity}"/> class
        /// </summary>
        /// <param name="connection">Database connection</param>
        /// <param name="lastRowIdCommand">SQL command tpo get Id of the last row inserted. Defaults to TSQL syntax: SELECT @@IDENTITY</param>
        /// <param name="tableName">Table name. Defaults to entity type name</param>
        /// <param name="limitOffsetPattern">Limit and offset pattern. Must contain paceholders {PageNumber} and {PageSize}. Defaults to TSQL syntax: OFFSET ({PageNumber} - 1) * {PageSize} ROWS FETCH NEXT {PageSize} ROWS ONLY</param>
        /// <param name="idProperty">Id property expression</param>
        public DapperRepository(
          IDbConnection connection,
          string lastRowIdCommand = "SELECT @@IDENTITY",
          string limitOffsetPattern = "OFFSET ({PageNumber} - 1) * {PageSize} ROWS FETCH NEXT {PageSize} ROWS ONLY",
          string tableName = null,
          Expression<Func<TEntity, object>> idProperty = null)
          : base(idProperty)
        {
            this.connection = connection;

            if (tableName != null)
            {
                TableName = tableName;
            }
            else
            {
                TableName = EntityTypeName;
            }

            LastRowIdCommand = lastRowIdCommand;
            LimitOffsetPattern = limitOffsetPattern;
        }

        /// <summary>
        /// Gets number of items per page (when paging is used)
        /// </summary>
        public virtual int PageSize { get; private set; } = 0;

        /// <summary>
        /// Gets page number (one based index)
        /// </summary>
        public virtual int PageNumber { get; private set; } = 1;


        public List<string> ExcludeProperty { get; set; } = new List<string>() { "Total" };

        /// <summary>
        /// Gets the total number of items available in this set. For example, if a user has 100 blog posts, the response may only contain 10 items, but the totalItems would be 100.
        /// </summary>
        public virtual long TotalItems
        {
            get
            {
                if (this.totalItemsTask != null)
                {
                    this.totalItemsTask.WaitSync();
                    this.totalItems = (long)this.totalItemsTask.Result;
                    this.totalItemsTask = null;
                }

                return this.totalItems;
            }
        }

        /// <summary>
        /// Gets the index of the first item. For consistency, startIndex should be 1-based. For example, the first item in the first set of items should have a startIndex of 1. If the user requests the next set of data, the startIndex may be 10.
        /// </summary>
        public virtual int StartIndex
        {
            get { return PageNumber < 2 ? 1 : ((PageNumber - 1) * PageSize) + 1; }
        }

        /// <summary>
        /// Gets the total number of pages in the result set.
        /// </summary>
        public virtual int TotalPages
        {
            get { return PageSize == 0 ? 1 : (int)(TotalItems / PageSize) + 1; }
        }

        /// <summary>
        /// Gets the kind of sort order
        /// </summary>
        public virtual SortOrder SortOrder { get; private set; } = SortOrder.Unspecified;

        /// <summary>
        /// Gets property name for the property to sort by.
        /// </summary>
        public virtual string SortPropertyName { get; private set; } = null;

        /// <summary>
        /// Gets database connection
        /// </summary>
        public virtual IDbConnection Connection
        {
            get
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                return connection;
            }

            private set
            {
                connection = value;
            }
        }

        /// <summary>
        /// Gets SQL command to get Id of last row inserted
        /// </summary>
        protected virtual string LastRowIdCommand { get; private set; }

        /// <summary>
        /// Gets limit and offset pattern. Must contain paceholders {limit} and {offset}. Defaults to TSQL syntax: OFFSET {offset} ROWS FETCH NEXT {limit} ROWS ONLY
        /// </summary>
        protected virtual string LimitOffsetPattern { get; private set; }

        /// <summary>
        /// Gets table name
        /// </summary>
        protected virtual string TableName { get; private set; }

        /// <summary>
        /// Clear paging
        /// </summary>
        /// <returns>Current instance</returns>
        IPageableRepository<TEntity> IPageableRepository<TEntity>.ClearPaging()
        {
            return ClearPaging();
        }

        /// <summary>
        /// Clear paging
        /// </summary>
        /// <returns>Current instance</returns>
        public IDapperRepository<TEntity> ClearPaging()
        {
            PageSize = 0;
            PageNumber = 1;
            return this;
        }

        /// <summary>
        /// Clear sorting
        /// </summary>
        /// <returns>Current instance</returns>
        ISortableRepository<TEntity> ISortableRepository<TEntity>.ClearSorting()
        {
            return ClearSorting();
        }

        /// <summary>
        /// Clear sorting
        /// </summary>
        /// <returns>Current instance</returns>
        public IDapperRepository<TEntity> ClearSorting()
        {
            SortPropertyName = null;
            SortOrder = SortOrder.Unspecified;
            return this;
        }

        /// <summary>
        /// Execute a Stored Procedure script
        /// </summary>
        /// <param name="sql">Stored Procedure execute query</param>
        ///  <param name="parameters">List of parameters</param>
        /// <returns>Task<IEnumerable<DTO>></returns>
        public async Task<IEnumerable<DTO>> ExecuteStoredProcedure<DTO>(string sql, DynamicParameters parameters, IDbTransaction transaction = null)
        {
            int commandTimeout = 3000;
            if (Connection.State != ConnectionState.Open)
            {
                Connection.Open();
            }
            //IEnumerable<dynamic> results = Connection.Query(sql: sql, param: parameters, commandType: CommandType.StoredProcedure);
            //var result = await Connection.QueryAsync<DTO>(sql, parameters, commandType: CommandType.StoredProcedure);
            var result = await Connection.QueryAsync<DTO>(sql, parameters, transaction, commandType: CommandType.StoredProcedure, commandTimeout: commandTimeout);

            return result;
        }

        public async Task<IEnumerable<DTO>> Executebulk<DTO>(string sql, List<DynamicParameters> parameters, IDbTransaction transaction = null)
        {
            int commandTimeout = 3000;
            if (Connection.State != ConnectionState.Open)
            {
                Connection.Open();
            }
            //IEnumerable<dynamic> results = Connection.Query(sql: sql, param: parameters, commandType: CommandType.StoredProcedure);
            //var result = await Connection.QueryAsync<DTO>(sql, parameters, commandType: CommandType.StoredProcedure);
            var result = await Connection.QueryAsync<DTO>(sql, parameters, transaction, commandTimeout: commandTimeout);
            //commandType: CommandType.StoredProcedure,
            return result;
        }

        public async Task<IEnumerable<DTO>> ExecuteStoredqueryProcedure<DTO>(string sql, DynamicParameters parameters, IDbTransaction transaction = null)
        {

            if (Connection.State != ConnectionState.Open)
            {
                Connection.Open();
            }

            var result = await Connection.QueryAsync<DTO>(sql, parameters, transaction, commandType: CommandType.StoredProcedure);
            return result;
        }

        /// <summary>
        /// Create a new entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual void Create(TEntity entity, IDbTransaction transaction = null)
        {
            CreateAsync(entity, transaction).WaitSync();
        }

        /// <summary>
        /// Create a new entity
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>Task</returns>
        public virtual async Task CreateAsync(TEntity entity, IDbTransaction transaction = null)
        {
            var insertColumns = EntityColumns.Where(c => !ExcludeProperty.Contains(c));

            var insertQuery = $@"INSERT INTO {TableName} ({string.Join(",", insertColumns)})
                                    VALUES (@{string.Join(",@", insertColumns)});
                                    {LastRowIdCommand}";

            var result = await Connection.ExecuteAsync(insertQuery, entity, transaction);


            //IEnumerable<Guid?> result = await Connection.QueryAsync<Guid?>(insertQuery, entity);
            //EntityType.GetProperty(IdPropertyName)?
            //  .SetValue(entity, result.First());
        }

        public virtual async Task CreateRoleClaimAsync(TEntity entity, IDbTransaction transaction = null)
        {
            var insertColumns = EntityColumns.Where(c => !ExcludeProperty.Contains(c));

            var insertQuery = $@"INSERT INTO RoleClaims ({string.Join(",", insertColumns)})
                                    VALUES (@{string.Join(",@", insertColumns)});";

            IEnumerable<Guid?> result = await Connection.QueryAsync<Guid?>(insertQuery, entity, transaction);
            //EntityType.GetProperty(IdPropertyName)?
            //  .SetValue(entity, result.First());
        }

        /// <summary>
        /// Create a list of new entities
        /// </summary>
        /// <param name="entities">List of entities</param>
        public virtual void CreateMany(IEnumerable<TEntity> entities, IDbTransaction transaction = null)
        {
            CreateManyAsync(entities, transaction).WaitSync();
        }

        /// <summary>
        /// Create a list of new entities
        /// </summary>
        /// <param name="entities">List of entities</param>
        /// <returns>Task</returns>
        public virtual async Task CreateManyAsync(IEnumerable<TEntity> entities, IDbTransaction transaction = null)
        {
            try
            {
                var insertColumns = EntityColumns.Where(c => !ExcludeProperty.Contains(c));

                var insertCommand = $@"INSERT INTO {TableName} ({string.Join(",", insertColumns)}) 
                                    VALUES (@{string.Join(",@", insertColumns)})";
                //IEnumerable<Guid?> result = await Connection.QueryAsync<Guid?>(insertCommand, entities);

                await Connection.ExecuteAsync(insertCommand, entities.ToList(), transaction);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Delete an existing entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual void Delete(TEntity entity, IDbTransaction transaction = null)
        {
            DeleteAsync(entity, transaction).WaitSync();
        }

        /// <summary>
        /// Delete an existing entity
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>Task</returns>
        public virtual async Task DeleteAsync(TEntity entity, IDbTransaction transaction = null)
        {
            var deleteQCommand = $@"DELETE FROM {TableName}
                                        WHERE {IdPropertyName}=@{IdPropertyName}";

            await Connection.ExecuteAsync(deleteQCommand, entity, transaction);
        }

        /// <summary>
        /// Delete a list of existing entities
        /// </summary>
        /// <param name="entities">Entity list</param>
        public virtual void DeleteMany(IEnumerable<TEntity> entities, IDbTransaction transaction = null)
        {
            DeleteManyAsync(entities, transaction).WaitSync();
        }

        /// <summary>
        /// Delete a list of existing entities
        /// </summary>
        /// <param name="entities">Entity list</param>
        /// <returns>Task</returns>
        public virtual async Task DeleteManyAsync(IEnumerable<TEntity> entities, IDbTransaction transaction = null)
        {
            var deleteCommand = $@"DELETE FROM {TableName}
                                    WHERE {IdPropertyName} IN (@Id)";

            var ids = new List<object>();
            foreach (var entity in entities)
            {
                ids.Add(EntityType.GetProperty(IdPropertyName).GetValue(entity));
            }

            await Connection.ExecuteAsync(deleteCommand, ids.Select(i => new { Id = i }), transaction);
        }

        /// <summary>
        /// Get a list of entities
        /// </summary>
        /// <returns>Query result</returns>
        public virtual IEnumerable<TEntity> Find(IDbTransaction transaction = null)
        {
            var task = FindAsync(transaction);
            task.WaitSync();
            return task.Result;
        }

        /// <summary>
        /// Get a list of entities
        /// </summary>
        /// <returns>Query result</returns>
        public virtual async Task<IEnumerable<TEntity>> FindAsync(IDbTransaction transaction = null)
        {
            var r = await Connection.QueryAsync<TEntity>(GetQuery());
            if (PageSize == 0)
            {
                this.totalItems = r.LongCount();
            }
            else
            {
                totalItemsTask = Connection.ExecuteScalarAsync(GetCount(), transaction: transaction);
            }

            return r;
        }

        /// <summary>
        /// Filters a collection of entities using a predicate
        /// </summary>
        /// <param name="sql">SQL containing named parameter placeholders. For example: SELECT * FROM Customer WHERE Id = @Id</param>
        /// <param name="parameters">Named parameters</param>
        /// <param name="parameterPattern">Parameter Regex pattern, Defualts to @(\w+)</param>
        /// <returns>Filtered collection of entities</returns>
        public virtual IEnumerable<TEntity> Find(
          string sql,
          IDictionary<string, object> parameters = null,
          IDbTransaction transaction = null,
          string parameterPattern = @"@(\w+)")
        {
            var task = FindAsync(sql, parameters, transaction, parameterPattern);
            task.WaitSync();
            return task.Result;
        }

        /// <summary>
        /// Filters a collection of entities using a predicate
        /// </summary>
        /// <param name="sql">SQL containing named parameter placeholders. For example: SELECT * FROM Customer WHERE Id = @Id</param>
        /// <param name="parameters">Named parameters</param>
        /// <param name="parameterPattern">Parameter Regex pattern, Defualts to @(\w+)</param>
        /// <returns>Filtered collection of entities</returns>
        public virtual async Task<IEnumerable<TEntity>> FindAsync(
          string sql,
          IDictionary<string, object> parameters = null,
          IDbTransaction transaction = null,
          string parameterPattern = "@(\\w+)")
        {
            if (parameters == null)
            {
                parameters = new Dictionary<string, object>();
            }

            CheckParameters(sql, parameters, parameterPattern);
            var r = await Connection.QueryAsync<TEntity>(GetQuery(sql), ToObject(parameters), transaction);
            if (PageSize == 0)
            {
                this.totalItems = r.LongCount();
            }
            else
            {
                totalItemsTask = Connection.ExecuteScalarAsync(GetCount(), ToObject(parameters), transaction);
            }

            return r;
        }

        /// <summary>
        /// Gets an entity by id.
        /// </summary>
        /// <param name="id">Filter</param>
        /// <returns>Entity</returns>
        public virtual TEntity GetById(object id, IDbTransaction transaction = null)
        {
            var task = GetByIdAsync(id, transaction);
            task.WaitSync();
            return task.Result;
        }

        /// <summary>
        /// Gets an entity by id.
        /// </summary>
        /// <param name="id">Filter to find a single item</param>
        /// <returns>Entity</returns>
        public virtual async Task<TEntity> GetByIdAsync(object id, IDbTransaction transaction = null)
        {
            if (Connection.State != ConnectionState.Open)
            {
                Connection.Open();
            }

            var findQuery = $@"SELECT * FROM {TableName}
                                    WHERE {IdPropertyName}=@{IdPropertyName}";

            return await Connection.QueryFirstOrDefaultAsync<TEntity>(findQuery, new { Id = id }, transaction);
        }

        /// <summary>
        /// Use paging
        /// </summary>
        /// <param name="pageNumber">Page to get (one based index).</param>
        /// <param name="pageSize">Number of items per page.</param>
        /// <returns>Current instance</returns>
        IPageableRepository<TEntity> IPageableRepository<TEntity>.Page(int pageNumber, int pageSize)
        {
            return Page(pageNumber, pageSize);
        }

        /// <summary>
        /// Use paging
        /// </summary>
        /// <param name="pageNumber">Page to get (one based index).</param>
        /// <param name="pageSize">Number of items per page.</param>
        /// <returns>Current instance</returns>
        public IDapperRepository<TEntity> Page(int pageNumber, int pageSize)
        {
            PageSize = pageSize;
            PageNumber = pageNumber;
            return this;
        }

        /// <summary>
        /// Property to sort by (ascending)
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns>Current instance</returns>
        ISortableRepository<TEntity> ISortableRepository<TEntity>.SortBy(Expression<Func<TEntity, object>> property)
        {
            return SortBy(property);
        }

        /// <summary>
        /// Property to sort by (ascending)
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns>Current instance</returns>
        public IDapperRepository<TEntity> SortBy(Expression<Func<TEntity, object>> property)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            var name = GetPropertyName(property);
            SortBy(name);
            return this;
        }

        /// <summary>
        /// Sort ascending by a property
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>Current instance</returns>
        ISortableRepository<TEntity> ISortableRepository<TEntity>.SortBy(string propertyName)
        {
            return SortBy(propertyName);
        }

        /// <summary>
        /// Sort ascending by a property
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>Current instance</returns>
        public IDapperRepository<TEntity> SortBy(string propertyName)
        {
            if (propertyName == null)
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            ValidatePropertyName(propertyName, out propertyName);

            SortOrder = SortOrder.Ascending;
            SortPropertyName = propertyName;
            return this;
        }

        /// <summary>
        /// Property to sort by (descending)
        /// </summary>
        /// <param name="property">The property</param>
        /// <returns>Current instance</returns>
        ISortableRepository<TEntity> ISortableRepository<TEntity>.SortByDescending(Expression<Func<TEntity, object>> property)
        {
            return SortByDescending(property);
        }

        /// <summary>
        /// Property to sort by (descending)
        /// </summary>
        /// <param name="property">The property</param>
        /// <returns>Current instance</returns>
        public IDapperRepository<TEntity> SortByDescending(Expression<Func<TEntity, object>> property)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            var name = GetPropertyName(property);
            SortByDescending(name);
            return this;
        }

        /// <summary>
        /// Sort descending by a property.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>Current instance</returns>
        ISortableRepository<TEntity> ISortableRepository<TEntity>.SortByDescending(string propertyName)
        {
            return SortByDescending(propertyName);
        }

        /// <summary>
        /// Sort descending by a property.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>Current instance</returns>
        public IDapperRepository<TEntity> SortByDescending(string propertyName)
        {
            if (propertyName == null)
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            ValidatePropertyName(propertyName, out propertyName);

            SortOrder = SortOrder.Descending;
            SortPropertyName = propertyName;
            return this;
        }

        /// <summary>
        /// Update an existing entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual void Update(TEntity entity, IDbTransaction transaction = null)
        {

            UpdateAsync(entity, transaction).WaitSync();

        }

        /// <summary>
        /// Update an existing entity
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>Task</returns>
        public virtual async Task UpdateAsync(TEntity entity, IDbTransaction transaction = null)
        {
            if (Connection.State != ConnectionState.Open)
            {
                Connection.Open();
            }

            var columns = EntityColumns.Where(s => s != IdPropertyName);
            var parameters = columns.Select(name => name + "=@" + name).ToList();

            var updateQuery = $@"UPDATE {TableName} 
                                    SET {string.Join(",", parameters)}
                                    WHERE {IdPropertyName}=@{IdPropertyName}";

            await Connection.ExecuteAsync(updateQuery, entity, transaction);
        }

        ///// <summary>
        ///// Update an bulk existing entity
        ///// </summary>
        ///// <param name="sql">query</param>
        ///// <returns>Task</returns>
        //public virtual async Task<byte[]> UpdateBulkAsync(string query,List<object> list)
        //{
        //    if (Connection.State != ConnectionState.Open)
        //    {
        //        Connection.Open();
        //    }

        // return  await Connection.ExecuteScalarAsync<byte[]>(query, list);
        //}


        /// <summary>
        /// Update an existing entity
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>Task</returns>
        public virtual async Task UpdateWithConcurrencyAsync(TEntity entity, IDbTransaction transaction = null)
        {
            if (Connection.State != ConnectionState.Open)
            {
                Connection.Open();
            }

            var columns = EntityColumns.Where(s => s != IdPropertyName);
            var parameters = columns.Select(name => name + "=@" + name).ToList();

            var updateQuery = $@"UPDATE {TableName} 
                                    SET {string.Join(",", parameters)}
                                    OUTPUT inserted.RowVersion
                                    WHERE {IdPropertyName}=@{IdPropertyName}
                                    AND RowVersion = @RowVersion";

            var rowCount = await Connection.ExecuteScalarAsync<byte[]>(updateQuery, entity, transaction);

            if (rowCount == null)
            {
                throw new DBConcurrencyException("The entity you were trying to edit has changed. Reload the entity and try again.");
            }
        }

        /// <summary>
        /// Check SQL parameters
        /// </summary>
        /// <param name="sql">SQL containing named parameter placeholders. For example: SELECT * FROM Customer WHERE Id = @Id</param>
        /// <param name="parameters">Named parameters</param>
        /// <param name="parameterPattern">Parameter Regex pattern, Defualts to @(\w+)</param>
        protected virtual void CheckParameters(string sql, IDictionary<string, object> parameters, string parameterPattern)
        {
            var placeholders = Regex.Matches(sql, parameterPattern);
            for (int i = 0; i < placeholders.Count; i++)
            {
                var parameterName = Regex.Match(placeholders[i].Value, @"(\w+)").Value;
                if (!parameters.ContainsKey(parameterName))
                {
                    throw new ArgumentException($"Value must be specified for parameter \"{parameterName}\"");
                }
            }
        }

        public virtual async Task CreateRolesClaimAsync(TEntity entity, IDbTransaction transaction = null)
        {
            if (Connection.State != ConnectionState.Open)
            {
                Connection.Open();
            }

            var parameters = EntityColumns
                .Where(c => c != IdPropertyName)
                .Select(c => $"@{c}=@{c}").ToList();

            var createCommand = $"EXEC Create{EntityTypeName} {string.Join(",", parameters)}";

            IEnumerable<int> result = await Connection.QueryAsync<int>(createCommand, entity, transaction);

            EntityType.GetProperty(IdPropertyName)?
                .SetValue(entity, result.First());
        }
        /// <summary>
        /// Gets query string
        /// </summary>
        /// <param name="sql">SQL statement</param>
        /// <returns>Query string</returns>
        protected virtual string GetQuery(string sql = null)
        {
            if (sql == null)
            {
                sql = $"SELECT * FROM {TableName}";
            }

            var orderBy = string.Empty;
            if (SortOrder != SortOrder.Unspecified
              && !string.IsNullOrWhiteSpace(SortPropertyName))
            {
                var order = SortOrder == SortOrder.Descending ? " DESC" : string.Empty;
                orderBy = $"ORDER BY {SortPropertyName}{order}";
            }

            var offset = string.Empty;
            if (PageNumber > 1 || PageSize > 0)
            {
                offset = LimitOffsetPattern.Replace("{PageNumber}", $"{PageNumber}").Replace("{PageSize}", $"{PageSize}");
            }

            return $@"{sql}{orderBy}{offset}";
        }

        /// <summary>
        /// Gets query string
        /// </summary>
        /// <param name="sql">SQL statement</param>
        /// <returns>Query string</returns>
        protected virtual string GetCount(string sql = null)
        {
            if (sql == null)
            {
                sql = $"SELECT * FROM {TableName}";
            }

            sql = "SELECT COUNT(*) " + sql.Substring(sql.IndexOf("FROM"));

            return $@"{sql}";
        }

        public virtual IEnumerable<TEntity> GetLById(object id, IDbTransaction transaction = null)
        {
            var task = GetByLIdAsync(id, transaction);
            task.WaitSync();
            return task.Result;
        }

        public virtual async Task<IEnumerable<TEntity>> GetByLIdAsync(object id, IDbTransaction transaction = null)
        {
            if (Connection.State != ConnectionState.Open)
            {
                Connection.Open();
            }

            var findQuery = $@"SELECT * FROM {TableName}
                                    WHERE {IdPropertyName}=@{IdPropertyName}";

            return await Connection.QueryAsync<TEntity>(findQuery, new { Id = id }, transaction);

            //IEnumerable<TEntity> result = await Connection.QueryAsync<TEntity>(
            //    $"EXEC Get{EntityTypeName} @{IdPropertyName}",
            //    new { Id = id });

            //return result;
        }

        public void EstablishFactoryConnection()
        {
            dbConnection = factory.CreateConnection();
            if(dbConnection.State != ConnectionState.Open)
            {
                dbConnection.Open();
            }
        }

        private void PrepareCommand(bool blTransaction, CommandType cmdType, string cmdText)
        {

            if (dbConnection.State != ConnectionState.Open)
            {
                dbConnection.Open();
            }

            if (null == command)
                command = factory.CreateCommand();

            command.Connection = dbConnection;
            command.CommandText = cmdText;
            command.CommandType = cmdType;

            if (blTransaction)
                command.Transaction = dbTransaction;
        }
        private void PrepareCommand(bool blTransaction, CommandType cmdType, string cmdText, DynamicParameters parameters)
        {

            if (dbConnection.State != ConnectionState.Open)
            {
                dbConnection.Open();
            }

            if (null == command)
                command = factory.CreateCommand();

            command.Connection = dbConnection;
            command.CommandText = cmdText;
            command.CommandType = cmdType;

            if (blTransaction)
                command.Transaction = dbTransaction;
        }

        public DbDataReader ExecuteReader(CommandType cmdType, string cmdText)
        {
            try
            {
                EstablishFactoryConnection();
                PrepareCommand(false, cmdType, cmdText);
                DbDataReader drg = command.ExecuteReader(CommandBehavior.CloseConnection);
                command.Parameters.Clear();
                return drg;
            }
            catch(Exception ex)
            {
                if (dbConnection.State == ConnectionState.Open)
                {
                    dbConnection.Close();
                }
                throw ex;
            }
            finally
            {
                if (null != dbConnection)
                    dbConnection.Dispose(); 
            }
        }

        public DbDataReader ExecuteReader(CommandType cmdType, string cmdText, DynamicParameters parameters)
        {
            try
            {
                EstablishFactoryConnection();
                PrepareCommand(false, cmdType, cmdText, parameters);
                DbDataReader drg = command.ExecuteReader(CommandBehavior.CloseConnection);
                command.Parameters.Clear();
                return drg;
            }
            catch (Exception ex)
            {
                if (dbConnection.State == ConnectionState.Open)
                {
                    dbConnection.Close();
                }
                throw ex;
            }
            finally
            {
                if (null != dbConnection)
                    dbConnection.Dispose();
            }
        }



    }
}
