

using DbHelper.Models;
using PluralizeService.Core;
using System.Data;
using System;
using System.Data.SqlClient;
using System.Reflection;
using System.ComponentModel.DataAnnotations;

namespace DbHelper.Services;

public static class AdoNetService
{
    public static IEnumerable<T> GetAll<T>(this T entity) where T:BaseEntity 
    {
        var items= new List<T>();
        
        var tableName = GetTableName(entity);

        using (SqlConnection connection = new(Environment.GetEnvironmentVariable("ConnectionString")))
        {
            using (SqlCommand command = new(cmdText:$"SELECT * FROM {tableName}",connection:connection))
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var currentItem=Activator.CreateInstance(typeof(T)) as T;
                        //var instance = Activator.CreateInstance<T>();
                        foreach (var property in currentItem!.GetType().GetProperties())
                        {
                            property.SetValue(currentItem, reader[property.Name]);
                        }
                        items.Add(currentItem);
                    }
                }
            }
        }

        
        return items;
    }
    public static IEnumerable<T> GetAll<T>(this T entity, params string[] columnNames) where T : BaseEntity
    {
        var items = new List<T>();
        var tableName = GetTableName(entity);
        // windows environment yaz
        using (SqlConnection connection = new(Environment.GetEnvironmentVariable("ConnectionString")))
        {
            using (SqlCommand command = new(cmdText: $"SELECT {string.Join(",",columnNames)} FROM {tableName}", connection: connection))
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var currentItem=Activator.CreateInstance(typeof(T)) as T;
                        //var instance = Activator.CreateInstance<T>();
                        foreach (var property in currentItem!.GetType().GetProperties())
                        {
                            try
                            {
                                property.SetValue(currentItem, reader[property.Name]);
                            }
                            catch { }
                        }
                        items.Add(currentItem);
                    }
                }
            }
        }


        return items;
    }
    public static bool Add<T>(this T entity) where T:BaseEntity
    {
        var tableName = GetTableName(entity);
        Dictionary<string,object> keyValuePairs = new();
        foreach (var item in entity.GetType().GetProperties())
        {
            var pInfo = item.GetCustomAttribute<KeyAttribute>();
            if (pInfo != null)
                continue;
            keyValuePairs.Add(item.Name, item.GetValue(entity));
        }
        using (SqlConnection connection = new(Environment.GetEnvironmentVariable("ConnectionString")))
        {
            

            string commandText = $"INSERT INTO {tableName} ({string.Join(",",keyValuePairs.Keys)}) VALUES(@{string.Join(", @",keyValuePairs.Keys)})";
            using (SqlCommand command = new(cmdText: commandText, connection: connection))
            {
                foreach (var keyValue in keyValuePairs)
                {
                    command.Parameters.AddWithValue($"@{keyValue.Value}", keyValue.Value);
                }

                connection.Open();
                return command.ExecuteNonQuery() > 0;


            }
        }
        return true;
    }
    private static string GetTableName(BaseEntity entity) =>  PluralizationProvider.Pluralize(entity.GetType().Name);
    
}
