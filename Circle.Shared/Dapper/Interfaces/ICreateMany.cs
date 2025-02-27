﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Circle.Shared.Dapper.Interfaces
{
    /// <summary>
    /// Creates a list of new entities.
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    public interface ICreateMany<TEntity>
      where TEntity : class
    {
        /// <summary>
        /// Create a list of new entities
        /// </summary>
        /// <param name="entities">List of entities</param>
        void CreateMany(IEnumerable<TEntity> entities, IDbTransaction transaction = null);
    }
}
