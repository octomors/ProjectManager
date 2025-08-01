using Model.Model.Entities;
using ProjectManager.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.OleDb;

namespace AccessDataBase
{
    public class MDRepository : Repository, IRepository<MD>
    {
        public void Create(MD entity)
        {
            var connection = new OleDbConnection(ConnectionString);
            connection.Open();

            var insertQuery = "INSERT INTO tab_MD (Ax, Ay, Bx, By, Cx, Cy, Dx, Dy, Qx, Qy, H, DType, ControlMD, TransmiterLoopSize," +
                "TransmitterLoopSize2, Remarks, TransmitterNumberCoins) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";
            using (var cmd = new OleDbCommand(insertQuery, connection))
            {
                cmd.Parameters.AddWithValue("Ax", entity.Ax);
                cmd.Parameters.AddWithValue("Ay", entity.Ay);
                cmd.Parameters.AddWithValue("Bx", entity.Bx);
                cmd.Parameters.AddWithValue("By", entity.By);
                cmd.Parameters.AddWithValue("Cx", entity.Cx);
                cmd.Parameters.AddWithValue("Cy", entity.Cy);
                cmd.Parameters.AddWithValue("Dx", entity.Dx);
                cmd.Parameters.AddWithValue("Dy", entity.Dy);
                cmd.Parameters.AddWithValue("Qx", entity.Qx);
                cmd.Parameters.AddWithValue("Qy", entity.Qy);
                cmd.Parameters.AddWithValue("H", entity.H);
                cmd.Parameters.AddWithValue("DType", entity.DType);
                cmd.Parameters.AddWithValue("ControlMD", entity.ControlMD);
                cmd.Parameters.AddWithValue("TransmiterLoopSize", entity.TransmitterLoopSize);
                cmd.Parameters.AddWithValue("TransmitterLoopSize2", entity.TransmitterLoopSize2);
                cmd.Parameters.AddWithValue("Remarks", entity.Remarks);
                cmd.Parameters.AddWithValue("TransmitterNumberCoins", entity.TransmitterNumberCoins);

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

        public void CreateKeepID(MD entity)
        {
            var connection = new OleDbConnection(ConnectionString);
            connection.Open();

            var insertQuery = "INSERT INTO tab_MD ([MD], [Ax], [Ay], [Bx], [By], [Cx], [Cy], [Dx], [Dy], [Qx], [Qy], [H], " +
                "[DType], [ControlMD], [TransmitterLoopSize], [TransmitterLoopSize2], [Remarks], [TransmitterNumberCoins]) " +
                "VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";
            using (var cmd = new OleDbCommand(insertQuery, connection))
            {
                cmd.Parameters.AddWithValue("MD", entity.ID);
                cmd.Parameters.AddWithValue("Ax", entity.Ax);
                cmd.Parameters.AddWithValue("Ay", entity.Ay);
                cmd.Parameters.AddWithValue("Bx", entity.Bx);
                cmd.Parameters.AddWithValue("By", entity.By);
                cmd.Parameters.AddWithValue("Cx", entity.Cx);
                cmd.Parameters.AddWithValue("Cy", entity.Cy);
                cmd.Parameters.AddWithValue("Dx", entity.Dx);
                cmd.Parameters.AddWithValue("Dy", entity.Dy);
                cmd.Parameters.AddWithValue("Qx", entity.Qx);
                cmd.Parameters.AddWithValue("Qy", entity.Qy);
                cmd.Parameters.AddWithValue("H", entity.H);
                cmd.Parameters.AddWithValue("DType", entity.DType);
                cmd.Parameters.AddWithValue("ControlMD", entity.ControlMD);
                cmd.Parameters.AddWithValue("TransmitterLoopSize", entity.TransmitterLoopSize);
                cmd.Parameters.AddWithValue("TransmitterLoopSize2", entity.TransmitterLoopSize2);
                cmd.Parameters.AddWithValue("Remarks", entity.Remarks);
                cmd.Parameters.AddWithValue("TransmitterNumberCoins", entity.TransmitterNumberCoins);

                cmd.ExecuteNonQuery();
            }
            
            connection.Close();
        }

        public void Delete(int ID)
        {
            var connection = new OleDbConnection(ConnectionString);
            connection.Open();

            var query = "DELETE FROM tab_MD WHERE MD = ?";
            using (var cmd = new OleDbCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("MD", ID);
                cmd.ExecuteNonQuery();
            }

            connection.Close();
        }

        public MD Read(int ID)
        {
            var connection = new OleDbConnection(ConnectionString);
            connection.Open();

            var query = "SELECT MD, Ax, , Bx, By, Cx, Cy, Dx, Dy, Qx, Qy, H, DType, ControlMD, TransmiterLoopSize, TransmitterLoopSize2," +
                "Remarks, TransmitterNumberCoins FROM tab_MD WHERE MD = ?";
            using (var cmd = new OleDbCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("MD", ID);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new MD()
                        {
                            ID = Convert.ToInt32(reader["MD"]),
                            Ax = Convert.ToInt32(reader["Ax"]),
                            Ay = Convert.ToInt32(reader["Ay"]),
                            Bx = Convert.ToInt32(reader["Bx"]),
                            By = Convert.ToInt32(reader["By"]),
                            Cx = Convert.ToInt32(reader["Cx"]),
                            Cy = Convert.ToInt32(reader["Cy"]),
                            Dx = Convert.ToInt32(reader["Dx"]),
                            Dy = Convert.ToInt32(reader["Dy"]),
                            Qx = Convert.ToInt32(reader["Qx"]),
                            Qy = Convert.ToInt32(reader["Qy"]),
                            H = Convert.ToInt32(reader["H"]),
                            DType = reader["DType"].ToString(),
                            ControlMD = bool.Parse(reader["ControlMD"].ToString()),
                            TransmitterLoopSize = Convert.ToInt32(reader["TransmitterLoopSize"]),
                            TransmitterLoopSize2 = Convert.ToInt32(reader["TransmitterLoopSize2"]),
                            Remarks = reader["Remarks"].ToString(),
                            TransmitterNumberCoins = Convert.ToInt32(reader["TransmitterNumberCoins"]),
                        };
                    }
                }
            }

            connection.Close();

            return null;
        }

        public IEnumerable<MD> ReadAll()
        {
            var MDs = new List<MD>();

            var connection = new OleDbConnection(ConnectionString);
            connection.Open();

            var query = "SELECT MD, Ax, Ay, Bx, By, Cx, Cy, Dx, Dy, Qx, Qy, H, DType, ControlMD, TransmitterLoopSize, TransmitterLoopSize2," +
                "Remarks, TransmitterNumberCoins FROM tab_MD";
            using (var cmd = new OleDbCommand(query, connection))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        MDs.Add(new MD()
                        {
                            ID = Convert.ToInt32(reader["MD"]),
                            Ax = Convert.ToInt32(reader["Ax"]),
                            Ay = Convert.ToInt32(reader["Ay"]),
                            Bx = Convert.ToInt32(reader["Bx"]),
                            By = Convert.ToInt32(reader["By"]),
                            Cx = Convert.ToInt32(reader["Cx"]),
                            Cy = Convert.ToInt32(reader["Cy"]),
                            Dx = Convert.ToInt32(reader["Dx"]),
                            Dy = Convert.ToInt32(reader["Dy"]),
                            Qx = Convert.ToInt32(reader["Qx"]),
                            Qy = Convert.ToInt32(reader["Qy"]),
                            H = Convert.ToInt32(reader["H"]),
                            DType = reader["DType"].ToString(),
                            ControlMD = bool.Parse(reader["ControlMD"].ToString()),
                            TransmitterLoopSize = Convert.ToInt32(reader["TransmitterLoopSize"]),
                            TransmitterLoopSize2 = Convert.ToInt32(reader["TransmitterLoopSize2"]),
                            Remarks = reader["Remarks"].ToString(),
                            TransmitterNumberCoins = Convert.ToInt32(reader["TransmitterNumberCoins"]),
                        });
                    }
                }
            }

            connection.Close();

            return MDs;
        }

        public void Update(MD entity)
        {
            var connection = new OleDbConnection(ConnectionString);
            connection.Open();

            var query = 
                "UPDATE tab_MD " +
                "SET [Ax] = ?, [Ay] = ?, [Bx]= ?, [By] = ?, [Cx] = ?, [Cy] = ?, [Dx]= ?, [Dy] = ?, [Qx] = ?, [Qy] = ?, " +
                "H = ?, [DType] = ?, [ControlMD]= ?, [TransmitterLoopSize] = ?, [TransmitterLoopSize2] = ?, [Remarks] = ?, " +
                "[TransmitterNumberCoins] = ? WHERE [MD] = ?;";
            using (var cmd = new OleDbCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("Ax", entity.Ax);
                cmd.Parameters.AddWithValue("Ay", entity.Ay);
                cmd.Parameters.AddWithValue("Bx", entity.Bx);
                cmd.Parameters.AddWithValue("By", entity.By);
                cmd.Parameters.AddWithValue("Cx", entity.Cx);
                cmd.Parameters.AddWithValue("Cy", entity.Cy);
                cmd.Parameters.AddWithValue("Dx", entity.Dx);
                cmd.Parameters.AddWithValue("Dy", entity.Dy);
                cmd.Parameters.AddWithValue("Qx", entity.Qx);
                cmd.Parameters.AddWithValue("Qy", entity.Qy);
                cmd.Parameters.AddWithValue("H", entity.H);
                cmd.Parameters.AddWithValue("DType", entity.DType);
                cmd.Parameters.AddWithValue("ControlMD", entity.ControlMD);
                cmd.Parameters.AddWithValue("TransmiterLoopSize", entity.TransmitterLoopSize);
                cmd.Parameters.AddWithValue("TransmitterLoopSize2", entity.TransmitterLoopSize2);
                cmd.Parameters.AddWithValue("Remarks", entity.Remarks);
                cmd.Parameters.AddWithValue("TransmitterNumberCoins", entity.TransmitterNumberCoins);
                cmd.Parameters.AddWithValue("MD", entity.ID);

                cmd.ExecuteNonQuery();
            }

            connection.Close();
        }
    }
}
