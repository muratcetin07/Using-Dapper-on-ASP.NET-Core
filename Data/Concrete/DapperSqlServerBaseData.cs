using Dapper;
using Data.Abstract;
using Data.Core;
using Microsoft.Extensions.Configuration;
using Model.Core;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Utilities;

namespace Data.Concrete
{
    public class DapperSqlServerBaseData<T> : IData<T> where T : BaseModel
    {
        protected string connectionString = "";

        private readonly IConfiguration _config;

        public DapperSqlServerBaseData(IConfiguration config)
        {
            _config = config;
            connectionString = config.GetConnectionString("DefaultConnection");
        }

        #region IData Implementation

        /// <summary>
        /// Insert new Entity
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public DataResult<T> Insert(T t)
        {

            var columns = GetColumns();
            var stringOfColumns = string.Join(", ", columns);
            var stringOfParameters = string.Join(", ", columns.Select(e => "@" + e));
            var query = $"insert into {typeof(T).Name} ({stringOfColumns}) values ({stringOfParameters})";

            using (var connection = new SqlConnection(connectionString))
            {
                using (var tran = connection.BeginTransaction())
                {
                    try
                    {
                        connection.Open();
                        connection.Execute(query, t);
                        tran.Commit();
                    }
                    catch (Exception e)
                    {
                        tran.Rollback();
                        return new DataResult<T>(t)
                        {
                            Code = ResponseCode.BadRequest,
                            Message = e.Message + e.InnerException == null ? "" : "(" + e.InnerException + ")"
                        };
                    }
                }

            }

            return new DataResult<T>(t) { Code = ResponseCode.OK };
        }

        /// <summary>
        /// bulk insert
        /// </summary>
        /// <param name="ts"></param>
        /// <returns></returns>
        public DataResult InsertBulk(List<T> ts)
        {

            var columns = GetColumns();
            var stringOfColumns = string.Join(", ", columns);
            var stringOfParameters = string.Join(", ", columns.Select(e => "@" + e));
            var query = $"insert into {typeof(T).Name} ({stringOfColumns}) values ({stringOfParameters})";

            using (var connection = new SqlConnection(connectionString))
            {
                using (var tran = connection.BeginTransaction())
                {
                    try
                    {
                        connection.Open();

                        foreach (var item in ts)
                        {
                            connection.Execute(query, item);
                        }

                        tran.Commit();

                        return new DataResult(true, "");
                    }
                    catch (Exception e)
                    {
                        tran.Rollback();
                        return new DataResult(false, e.Message + e.InnerException == null ? "" : "(" + e.InnerException + ")");
                    }
                }

            }


        }

        /// <summary>
        /// Update Entity
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public DataResult<T> Update(T t)
        {
            var columns = GetColumns();
            var stringOfColumns = string.Join(", ", columns.Select(e => $"{e} = @{e}"));
            var query = $"update {typeof(T).Name} set {stringOfColumns} where Id = @Id";

            using (var connection = new SqlConnection(connectionString))
            {
                using (var tran = connection.BeginTransaction())
                {
                    try
                    {
                        connection.Open();
                        connection.Execute(query, t);
                        tran.Commit();
                    }
                    catch (Exception e)
                    {
                        tran.Rollback();
                        return new DataResult<T>(t)
                        {
                            Code = ResponseCode.BadRequest,
                            Message = e.Message + e.InnerException == null ? "" : "(" + e.InnerException + ")"
                        };
                    }
                }

            }

            return new DataResult<T>(t) { Code = ResponseCode.OK };
        }

        /// <summary>
        /// Delete Entity
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public DataResult<bool> Delete(T t)
        {

            var query = $"delete from {typeof(T).Name} where Id = @Id";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                connection.Execute(query, t);
            }

            return new DataResult<bool>(true) { Code = ResponseCode.OK };
        }

        /// <summary>
        /// Delete Entity by Key
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataResult<bool> DeleteByKey(long id)
        {
            var query = $"delete from {typeof(T).Name} where Id = @Id";


            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                connection.Query(query, new { Id = id });
            }

            return new DataResult<bool>(true) { Code = ResponseCode.OK };
        }

        /// <summary>
        /// Get Entity By Key
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T GetByKey(long id)
        {
            var query = $"select * from {typeof(T).Name} where Id=@Id";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return connection.Query<T>(query, new { Id = id }).FirstOrDefault();
            }
        }

        /// <summary>
        /// Get All Entities
        /// </summary>
        /// <returns></returns>
        public List<T> GetAll()
        {
            var query = $"select * from {typeof(T).Name}";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return connection.Query<T>(query).ToList();
            }
        }

        /// <summary>
        /// Get Count of Entities
        /// </summary>
        /// <returns></returns>
        public int GetCount()
        {
            var query = $"select count(*) from {typeof(T).Name}";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return connection.ExecuteScalar<int>(query);
            }

        }

        #endregion

        private IEnumerable<string> GetColumns()
        {
            return typeof(T)
                    .GetProperties()
                    .Where(e => e.Name != "Id" && !e.PropertyType.GetTypeInfo().IsGenericType)
                    .Select(e => e.Name);
        }

    }
}
