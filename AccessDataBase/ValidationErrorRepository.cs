using Model.Model.Entities;
using Model.Model.Interfaces;
using ProjectManager.Model.Entities;
using System;
using System.Collections.Generic;
using System.Data.OleDb;

namespace AccessDataBase
{
    public class ValidationErrorRepository : Repository, IValidationErrorRepository
    {
        /// <summary>
        /// Создает новую запись в бд, присваивает сущности вычисленный ID
        /// </summary>
        /// <param name="entity"></param>
        public void Create(ValidationError entity)
        {
            var connection = new OleDbConnection(ConnectionString);
            connection.Open();

            var insertQuery = "INSERT INTO ValidationErrors (ReportID, Message, ErrorTypeID) VALUES (?, ?, ?)";
            using(var cmd = new OleDbCommand(insertQuery, connection))
            {
                cmd.Parameters.AddWithValue("ReportID", entity.ReportID);
                cmd.Parameters.AddWithValue("Message", entity.Message);
                cmd.Parameters.AddWithValue("ErrorTypeID", entity.TypeID);

                cmd.ExecuteNonQuery();
            }

            var identityQuery = "SELECT @@IDENTITY";
            using(var cmd = new OleDbCommand(identityQuery, connection))
            {
                object result = cmd.ExecuteScalar();
                if(result != null && int.TryParse(result.ToString(), out int assignedId))
                {
                    entity.ID = assignedId;
                }
            }

            connection.Close();
        }

        public void CreateKeepID(ValidationError entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int ID)
        {
            var connection = new OleDbConnection(ConnectionString);
            connection.Open();

            var query = "DELETE FROM ValidationErrors WHERE ID = ?";
            using(var cmd = new OleDbCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("ID", ID);
                cmd.ExecuteNonQuery();
            }

            connection.Close();
        }

        public ValidationError Read(int ID)
        {
            var connection = new OleDbConnection(ConnectionString);
            connection.Open();

            var query = "SELECT ID, ReportID, Message, ErrorTypeID FROM ValidationErrors WHERE ID = ?";
            using(var cmd = new OleDbCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("ID", ID);

                using(var reader = cmd.ExecuteReader())
                {
                    if(reader.Read())
                    {
                        return new ValidationError(
                            Convert.ToInt32(reader["ID"]),
                            Convert.ToInt32(reader["ReportID"]),
                            reader["Message"].ToString(),
                            Convert.ToInt32(reader["ErrorTypeID"])
                            );
                    }
                }
            }

            connection.Close();

            return null;
        }

        public IEnumerable<ValidationError> ReadAll()
        {
            var ValidationErrors = new List<ValidationError>();

            var connection = new OleDbConnection(ConnectionString);
            connection.Open();

            var query = "SELECT ID, ReportID, ErrorTypeID Message FROM ValidationErrors";
            using (var cmd = new OleDbCommand(query, connection))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ValidationErrors.Add(new ValidationError(
                            Convert.ToInt32(reader["ID"]),
                            Convert.ToInt32(reader["ReportID"]),
                            reader["Message"].ToString(),
                            Convert.ToInt32(reader["ErrorTypeID"])
                            ));
                    }
                }
            }

            connection.Close();

            return ValidationErrors;
        }

        public IEnumerable<ValidationError> ReadAllByReportID(int id)
        {
            var ValidationErrors = new List<ValidationError>();

            var connection = new OleDbConnection(ConnectionString);
            connection.Open();

            var query = "SELECT ID, ReportID, Message, ErrorTypeID FROM ValidationErrors WHERE ReportID = ?";
            using (var cmd = new OleDbCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("ReportID", id);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ValidationErrors.Add(new ValidationError(
                            Convert.ToInt32(reader["ID"]),
                            Convert.ToInt32(reader["ReportID"]),
                            reader["Message"].ToString(),
                            Convert.ToInt32(reader["ErrorTypeID"])
                            ));
                    }
                }
            }

            connection.Close();

            return ValidationErrors;
        }

        public Dictionary<int, string> ReadAllErrorTypes()
        {
            var ValidationErrorTypes = new Dictionary<int, string>();

            var connection = new OleDbConnection(ConnectionString);
            connection.Open();

            var query = "SELECT ID, Title FROM ValidationErrorTypes";
            using (var cmd = new OleDbCommand(query, connection))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ValidationErrorTypes.Add(
                            Convert.ToInt32(reader["ID"]),
                            (reader["Title"]).ToString()
                            );
                    }
                }
            }

            connection.Close();

            return ValidationErrorTypes;
        }

        public void Update(ValidationError entity)
        {
            var connection = new OleDbConnection(ConnectionString);
            connection.Open();

            var query = "UPDATE ValidationErrors SET ReportID = ?, Message = ? WHERE ID = ?";
            using (var cmd = new OleDbCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("ReportID", entity.ReportID);
                cmd.Parameters.AddWithValue("Message", entity.Message);
                cmd.Parameters.AddWithValue("ID", entity.ID);

                cmd.ExecuteNonQuery();
            }

            connection.Close();
        }
    }
}
