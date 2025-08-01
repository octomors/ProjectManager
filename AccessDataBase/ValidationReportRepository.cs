using Model.Model.Entities;
using Model.Model.Interfaces;
using ProjectManager.Model.Entities;
using System;
using System.Collections.Generic;
using System.Data.OleDb;

namespace AccessDataBase
{
    public class ValidationReportRepository : Repository, IValidationReportRepository
    {
        /// <summary>
        /// Создает новую запись в бд, присваивает сущности вычисленный ID
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public void Create(ValidationReport entity)
        {
            var connection = new OleDbConnection(ConnectionString);
            connection.Open();

            var insertQuery = "INSERT INTO ValidationReports (ProjectID, CheckDateTime, Comments, EmployeeName) VALUES (?, ?, ?, ?)";
            using (var cmd = new OleDbCommand(insertQuery, connection))
            {
                cmd.Parameters.AddWithValue("ProjectID", entity.ProjectID);
                cmd.Parameters.AddWithValue("CheckDateTime", entity.CheckDateTime.ToString());
                cmd.Parameters.AddWithValue("Comments", entity.Comments);
                cmd.Parameters.AddWithValue("EmployeeName", entity.EmployeeName);

                cmd.ExecuteNonQuery();
            }

            var identityQuery = "SELECT @@IDENTITY";
            using (var cmd = new OleDbCommand(identityQuery, connection))
            {
                object result = cmd.ExecuteScalar();
                if (result != null && int.TryParse(result.ToString(), out int assignedId))
                {
                    entity.ID = assignedId;
                }
            }

            connection.Close();
        }

        public void CreateKeepID(ValidationReport entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int ID)
        {
            var connection = new OleDbConnection(ConnectionString);
            connection.Open();

            var query = "DELETE FROM ValidationReports WHERE ID = ?";
            using (var cmd = new OleDbCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("ID", ID);
                cmd.ExecuteNonQuery();
            }

            connection.Close();
        }

        public ValidationReport Read(int ID)
        {
            var connection = new OleDbConnection(ConnectionString);
            connection.Open();

            var query = "SELECT ID, ProjectID, CheckDateTime, Comments, EmployeeName FROM ValidationReports WHERE ID = ?";
            using (var cmd = new OleDbCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("ID", ID);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new ValidationReport(
                            Convert.ToInt32(reader["ID"]),
                            Convert.ToInt32(reader["ProjectID"]),
                            Convert.ToDateTime(reader["CheckDateTime"]),
                            reader["Comments"].ToString(),
                            reader["EmployeeName"].ToString()
                            );
                    }
                }
            }

            connection.Close();

            return null;
        }

        public IEnumerable<ValidationReport> ReadAll()
        {
            var validationReports = new List<ValidationReport>();

            var connection = new OleDbConnection(ConnectionString);
            connection.Open();

            var query = "SELECT ID, ProjectID, CheckDateTime, Comments, EmployeeName FROM ValidationReports";
            using (var cmd = new OleDbCommand(query, connection))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        validationReports.Add(new ValidationReport(
                            Convert.ToInt32(reader["ID"]),
                            Convert.ToInt32(reader["ProjectID"]),
                            Convert.ToDateTime(reader["CheckDateTime"]),
                            reader["Comments"].ToString(),
                            reader["EmployeeName"].ToString()
                            ));
                    }
                }
            }

            connection.Close();

            return validationReports;
        }

        public IEnumerable<ValidationReport> ReadAllByProjectID(int id)
        {
            var validationReports = new List<ValidationReport>();

            var connection = new OleDbConnection(ConnectionString);
            connection.Open();

            var query = "SELECT ID, ProjectID, CheckDateTime, Comments, EmployeeName FROM ValidationReports WHERE ProjectID = ?";
            using (var cmd = new OleDbCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("ProjectID", id);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        validationReports.Add(new ValidationReport(
                            Convert.ToInt32(reader["ID"]),
                            Convert.ToInt32(reader["ProjectID"]),
                            Convert.ToDateTime(reader["CheckDateTime"]),
                            reader["Comments"].ToString(),
                            reader["EmployeeName"].ToString()
                            ));
                    }
                }
            }

            connection.Close();

            return validationReports;
        }

        public void Update(ValidationReport entity)
        {
            var connection = new OleDbConnection(ConnectionString);
            connection.Open();

            var query = "UPDATE ValidationReports SET ProjectID = ?, CheckDateTime = ?, Comments = ?, EmployeeName = ? WHERE ID = ?";
            using (var cmd = new OleDbCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("ProjectID", entity.ProjectID);
                cmd.Parameters.AddWithValue("CheckDateTime", entity.CheckDateTime.ToString());
                cmd.Parameters.AddWithValue("Comments", entity.Comments);
                cmd.Parameters.AddWithValue("EmployeeName", entity.EmployeeName);
                cmd.Parameters.AddWithValue("ID", entity.ID);

                cmd.ExecuteNonQuery();
            }

            connection.Close();
        }
    }
}
