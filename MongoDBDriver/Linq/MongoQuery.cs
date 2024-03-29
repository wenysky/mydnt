﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace MongoDB.Linq
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MongoQuery<T> : IOrderedQueryable<T>, IMongoQueryable
    {
        private readonly Expression _expression;
        private readonly MongoQueryProvider _provider;

        /// <summary>
        /// Gets the expression tree that is associated with the instance of <see cref="T:System.Linq.IQueryable"/>.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// The <see cref="T:System.Linq.Expressions.Expression"/> that is associated with this instance of <see cref="T:System.Linq.IQueryable"/>.
        /// </returns>
        Expression IQueryable.Expression
        {
            get { return _expression; }
        }

        /// <summary>
        /// Gets the type of the element(s) that are returned when the expression tree associated with this instance of <see cref="T:System.Linq.IQueryable"/> is executed.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// A <see cref="T:System.Type"/> that represents the type of the element(s) that are returned when the expression tree associated with this object is executed.
        /// </returns>
        Type IQueryable.ElementType
        {
            get { return typeof(T); }
        }

        /// <summary>
        /// Gets the name of the collection.
        /// </summary>
        /// <value>The name of the collection.</value>
        string IMongoQueryable.CollectionName
        {
            get { return _provider.CollectionName; }
        }

        /// <summary>
        /// Gets the database.
        /// </summary>
        /// <value>The database.</value>
        IMongoDatabase IMongoQueryable.Database
        {
            get { return _provider.Database; }
        }

        /// <summary>
        /// Gets the query provider that is associated with this data source.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// The <see cref="T:System.Linq.IQueryProvider"/> that is associated with this data source.
        /// </returns>
        IQueryProvider IQueryable.Provider
        {
            get { return this._provider; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoQuery&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="provider">The provider.</param>
        public MongoQuery(MongoQueryProvider provider)
        {
            if (provider == null)
                throw new ArgumentNullException("provider");

            this._expression = Expression.Constant(this);
            this._provider = provider;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoQuery&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="expression">The expression.</param>
        public MongoQuery(MongoQueryProvider provider, Expression expression)
        {
            if (provider == null)
                throw new ArgumentNullException("provider");
            if (expression == null)
                throw new ArgumentNullException("expression");

            if (!typeof(IQueryable<T>).IsAssignableFrom(expression.Type))
                throw new ArgumentOutOfRangeException("expression");
            this._provider = provider;
            this._expression = expression;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<T> GetEnumerator() {
            return ((IEnumerable<T>)this._provider.Execute(_expression)).GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator() {
            return ((IEnumerable)this._provider.Execute(_expression)).GetEnumerator();
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString() {
            return _provider.GetQueryObject(_expression).ToString();
        }

        /// <summary>
        /// Gets the query object.
        /// </summary>
        /// <returns></returns>
        MongoQueryObject IMongoQueryable.GetQueryObject()
        {
            return _provider.GetQueryObject(_expression);
        }
    }
}
