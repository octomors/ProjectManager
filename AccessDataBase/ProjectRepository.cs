using ProjectManager.Model.Interfaces;
using ProjectManager.Model.Entities;
using System.Collections.Generic;
using System;
using System.Data.OleDb;
using Model.Model.Entities;

namespace AccessDataBase
{
    public class ProjectRepository : Repository, IRepository<Project>
    {
        /// <summary>
        /// Создает новую запись в бд, присваивает сущности вычисленный ID
        /// </summary>
        /// <param name="entity"></param>
        public void Create(Project entity)
        {
            var connection = new OleDbConnection(ConnectionString);
            connection.Open();

            var insertQuery = "INSERT INTO Projects (Proj_Short, MethodNo, FDB_Dir, DB_Dir, DB_File, Proj_Alias) VALUES (?, ?, ?, ?, ?, ?)";
            using (var cmd = new OleDbCommand(insertQuery, connection))
            {
                cmd.Parameters.AddWithValue("Proj_Short", entity.Name);
                cmd.Parameters.AddWithValue("MethodNo", entity.MethodID);
                cmd.Parameters.AddWithValue("FDB_Dir", entity.Directory);
                cmd.Parameters.AddWithValue("DB_Dir", entity.DBFolder);
                cmd.Parameters.AddWithValue("DB_File", entity.DBFile);
                cmd.Parameters.AddWithValue("Proj_Alias", entity.ProjectAlias);

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

            var tabTEMInsertQuery = "INSERT INTO tabTEM ([Proj_No], [FDB_DirGIS], [FDB_DirTXT], [FDB_DirAVR], " +
                "[FDB_DirCurves], [FDB_DirRB], [FDB_DirTMP], [FDB_DirPOD]) VALUES (?, ?, ?, ?, ?, ?, ?, ?)";
            using (var cmd = new OleDbCommand(tabTEMInsertQuery, connection))
            {
                cmd.Parameters.AddWithValue("Proj_No", entity.ID);
                cmd.Parameters.AddWithValue("FDB_DirGIS", "\\GIS");
                cmd.Parameters.AddWithValue("FDB_DirTXT", "\\TXT");
                cmd.Parameters.AddWithValue("FDB_DirAVR", "\\AVR");
                cmd.Parameters.AddWithValue("FDB_DirCurves", "\\Curves");
                cmd.Parameters.AddWithValue("FDB_DirRB", "\\RB");
                cmd.Parameters.AddWithValue("FDB_DirTMP", "\\TMP");
                cmd.Parameters.AddWithValue("FDB_DirPOD", "\\POD");

                cmd.ExecuteNonQuery();
            }

            connection.Close();
        }

        public void CreateKeepID(Project entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int ID)
        {
            return;
            //var connection = new OleDbConnection(ConnectionString);
            //connection.Open();

            //var query = "DELETE FROM Projects WHERE Proj_No = ?";
            //using (var cmd = new OleDbCommand(query, connection))
            //{
            //    cmd.Parameters.AddWithValue("Proj_No", ID);
            //    cmd.ExecuteNonQuery();
            //}

            //connection.Close();
        }

        public Project Read(int ID)
        {
            var connection = new OleDbConnection(ConnectionString);
            connection.Open();

            var query = "SELECT Proj_No, Proj_Short, MethodNo, FDB_Dir, DB_Dir, DB_File, Proj_Alias FROM Projects WHERE Proj_No = ?";
            using (var cmd = new OleDbCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("Proj_No", ID);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Project(
                            Convert.ToInt32(reader["Proj_No"]),
                            reader["Proj_Short"].ToString(),
                            Convert.ToInt32(reader["MethodNo"]),
                            reader["FDB_Dir"].ToString(),
                            reader["DB_Dir"].ToString(),
                            reader["DB_File"].ToString(),
                            reader["Proj_Alias"].ToString()
                            );
                    }
                }
            }

            connection.Close();

            return null;
        }

        public IEnumerable<Project> ReadAll()
        {
            var projects = new List<Project>();

            var connection = new OleDbConnection(ConnectionString);
            connection.Open();

            var query = "SELECT Proj_No, Proj_Short, MethodNo, FDB_Dir, DB_Dir, DB_File, Proj_Alias FROM Projects ORDER BY Proj_No";
            using (var cmd = new OleDbCommand(query, connection))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        projects.Add(new Project(
                            Convert.ToInt32(reader["Proj_No"]),
                            reader["Proj_Short"].ToString(),
                            Convert.ToInt32(reader["MethodNo"]),
                            reader["FDB_Dir"].ToString(),
                            reader["DB_Dir"].ToString(),
                            reader["DB_File"].ToString(),
                            reader["Proj_Alias"].ToString()
                            ));
                    }
                }
            }

            connection.Close();

            return projects;
        }

        public void Update(Project entity)
        {
            var connection = new OleDbConnection(ConnectionString);
            connection.Open();

            var query = "UPDATE Projects SET Proj_Short = ?, MethodNo = ?, FDB_Dir = ?, DB_Dir = ?, DB_File = ?, Proj_Alias = ? WHERE Proj_No = ?";
            using (var cmd = new OleDbCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("Proj_Short", entity.Name);
                cmd.Parameters.AddWithValue("MethodNo", entity.MethodID);
                cmd.Parameters.AddWithValue("FDB_Dir", entity.Directory);
                cmd.Parameters.AddWithValue("DB_Dir", entity.DBFolder);
                cmd.Parameters.AddWithValue("DB_File", entity.DBFile);
                cmd.Parameters.AddWithValue("Proj_Alias", entity.ProjectAlias);
                cmd.Parameters.AddWithValue("Proj_No", entity.ID);

                cmd.ExecuteNonQuery();
            }

            connection.Close();
        }
    }
}
