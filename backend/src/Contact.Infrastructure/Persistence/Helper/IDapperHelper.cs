﻿using System.Data;
using Microsoft.Data.SqlClient;

namespace Contact.Infrastructure.Persistence.Helper;

public interface IDapperHelper : IDisposable
{
    SqlConnection GetConnection();
    Task<T> Get<T>(string sp, Object parms, CommandType commandType = CommandType.Text, IDbTransaction? transaction = null);
    Task<IEnumerable<T>> GetAll<T>(string sp, Object parms, CommandType commandType = CommandType.Text);
    Task<int> Execute(string sp, Object parms, CommandType commandType = CommandType.Text, IDbTransaction? transaction = null);
    Task<T> Insert<T>(string sp, Object parms, CommandType commandType = CommandType.Text, IDbTransaction? transaction = null);
    Task<T> Update<T>(string sp, Object parms, CommandType commandType = CommandType.Text, IDbTransaction? transaction = null);
}
