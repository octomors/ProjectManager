using Model.Model.Entities;
using ProjectManager.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using static System.Collections.Specialized.BitVector32;

namespace AccessDataBase
{
    public class ProfModRepository : Repository, IRepository<ProfMod>
    {
        public void Create(ProfMod entity)
        {
            var connection = new OleDbConnection(ConnectionString);
            connection.Open();

            var insertQuery = "INSERT INTO prof_Mod (PK, MD, TqqFileName, TxtFileName, PODFileName, AvrFileName, [Current], Angle, Station, " +
                "PKType, Remarks, Sync) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";
            using (var cmd = new OleDbCommand(insertQuery, connection))
            {
                cmd.Parameters.AddWithValue("PK", entity.PKID);
                cmd.Parameters.AddWithValue("MD", entity.MDID);
                cmd.Parameters.AddWithValue("TqqFileName", entity.TqqFileName);
                cmd.Parameters.AddWithValue("TxtFileName", entity.TxtFileName);
                cmd.Parameters.AddWithValue("PODFileName", entity.PodFileName);
                cmd.Parameters.AddWithValue("AvrFileName", entity.AvrFileName);
                cmd.Parameters.AddWithValue("Current", entity.Current);
                cmd.Parameters.AddWithValue("Angle", entity.Angle);
                cmd.Parameters.AddWithValue("Station", entity.Station);
                cmd.Parameters.AddWithValue("PKType", entity.PKType);
                cmd.Parameters.AddWithValue("Remarks", entity.Remarks);
                cmd.Parameters.AddWithValue("Sync", entity.Sync);

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

        public void CreateKeepID(ProfMod entity)
        {
            var connection = new OleDbConnection(ConnectionString);
            connection.Open();

            var insertQuery = "INSERT INTO prof_Mod ([RecNo], [PK], [MD], [TqqFileName], [TxtFileName], [PODFileName], " +
                "[AvrFileName], [Current], [Angle], [Station], [PKType], [Remarks], [Sync]) " +
                "VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";
            using (var cmd = new OleDbCommand(insertQuery, connection))
            {
                cmd.Parameters.AddWithValue("RecNo", entity.ID);
                cmd.Parameters.AddWithValue("PK", entity.PKID);
                cmd.Parameters.AddWithValue("MD", entity.MDID);
                cmd.Parameters.AddWithValue("TqqFileName", entity.TqqFileName);
                cmd.Parameters.AddWithValue("TxtFileName", entity.TxtFileName);
                cmd.Parameters.AddWithValue("PODFileName", entity.PodFileName);
                cmd.Parameters.AddWithValue("AvrFileName", entity.AvrFileName);
                cmd.Parameters.AddWithValue("Current", entity.Current);
                cmd.Parameters.AddWithValue("Angle", entity.Angle);
                cmd.Parameters.AddWithValue("Station", entity.Station);
                cmd.Parameters.AddWithValue("PKType", entity.PKType);
                cmd.Parameters.AddWithValue("Remarks", entity.Remarks);
                cmd.Parameters.AddWithValue("Sync", entity.Sync);

                cmd.ExecuteNonQuery();
            }
            connection.Close();
        }

        public void Delete(int ID)
        {
            var connection = new OleDbConnection(ConnectionString);
            connection.Open();

            var query = "DELETE FROM prof_Mod WHERE RecNo = ?";
            using (var cmd = new OleDbCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("RecNo", ID);
                cmd.ExecuteNonQuery();
            }

            connection.Close();
        }

        public ProfMod Read(int ID)
        {
            var connection = new OleDbConnection(ConnectionString);
            connection.Open();

            var query = "SELECT RecNo, PK, MD, TqqFileName, TxtFileName, PODFileName, AvrFileName, [Current], Angle, Station, " +
                "PKType, Remarks, Sync FROM prof_Mod WHERE Rec_No = ?";
            using (var cmd = new OleDbCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("RecNo", ID);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new ProfMod()
                        {
                            ID = Convert.ToInt32(reader["RecNo"]),
                            PKID = Convert.ToInt32(reader["PK"]),
                            MDID = Convert.ToInt32(reader["MD"]),
                            TqqFileName = reader["TqqFileName"].ToString(),
                            TxtFileName = reader["TxtFileName"].ToString(),
                            PodFileName = reader["PODFileName"].ToString(),
                            AvrFileName = reader["AvrFileName"].ToString(),
                            Current = Convert.ToInt32(reader["Current"]),
                            Angle = Convert.ToInt32(reader["Angle"]),
                            Station = Convert.ToInt32(reader["Station"]),
                            PKType = reader["PKType"].ToString(),
                            Remarks = reader["Remarks"].ToString(),
                            Sync = bool.Parse(reader["Sync"].ToString()),
                        };
                    }
                }
            }

            connection.Close();

            return null;
        }

        public IEnumerable<ProfMod> ReadAll()
        {
            var profMods = new List<ProfMod>();
            var connection = new OleDbConnection(ConnectionString);
            connection.Open();

            var query = "SELECT RecNo, PK, MD, TqqFileName, TxtFileName, PODFileName, AvrFileName, [Current], Angle, Station, " +
                "PKType, Remarks, Sync FROM prof_Mod";
            using (var cmd = new OleDbCommand(query, connection))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var ID = Convert.ToInt32(reader["RecNo"]);
                        var PKID = Convert.ToInt32(reader["PK"]);
                        var MDID = Convert.ToInt32(reader["MD"]);
                        var TqqFileName = reader["TqqFileName"].ToString();
                        var TxtFileName = reader["TxtFileName"].ToString();
                        var PodFileName = reader["PODFileName"].ToString();
                        var AvrFileName = reader["AvrFileName"].ToString();
                        var Current = Convert.ToInt32(reader["Current"]);
                        var Angle = Convert.ToInt32(reader["Angle"]);
                        var Station = Convert.ToInt32(reader["Station"]);
                        var PKType = reader["PKType"].ToString();
                        var Remarks = reader["Remarks"].ToString();
                        var Sync = bool.Parse(reader["Sync"].ToString());

                        profMods.Add(new ProfMod()
                        {
                            ID = ID,
                            PKID = PKID,
                            MDID = MDID,
                            TqqFileName = TqqFileName,
                            TxtFileName = TxtFileName,
                            PodFileName = PodFileName,
                            AvrFileName = AvrFileName,
                            Current = Current,
                            Angle = Angle,
                            Station = Station,
                            PKType = PKType,
                            Remarks = Remarks,
                            Sync = Sync,
                        });
                    }
                }
            }

            connection.Close();

            return profMods;
        }

        public void Update(ProfMod entity)
        {
            var connection = new OleDbConnection(ConnectionString);
            connection.Open();

            var query = "UPDATE prof_Mod SET PK = ?, MD = ?, TqqFileName = ?, TxtFileName = ?, PODFileName = ?, AvrFileName = ?," +
                "[Current] = ?, Angle = ?, Station = ?, PKType = ?, Remarks = ?, Sync = ? WHERE RecNo = ?";
            using (var cmd = new OleDbCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("PK", entity.PKID);
                cmd.Parameters.AddWithValue("MD", entity.MDID);
                cmd.Parameters.AddWithValue("TqqFileName", entity.TqqFileName);
                cmd.Parameters.AddWithValue("TxtFileName", entity.TxtFileName);
                cmd.Parameters.AddWithValue("PODFileName", entity.PodFileName);
                cmd.Parameters.AddWithValue("AvrFileName", entity.AvrFileName);
                cmd.Parameters.AddWithValue("Current", entity.Current);
                cmd.Parameters.AddWithValue("Angle", entity.Angle);
                cmd.Parameters.AddWithValue("Station", entity.Station);
                cmd.Parameters.AddWithValue("PKType", entity.PKType);
                cmd.Parameters.AddWithValue("Remarks", entity.Remarks);
                cmd.Parameters.AddWithValue("Sync", entity.Sync);
                cmd.Parameters.AddWithValue("RecNo", entity.ID);

                cmd.ExecuteNonQuery();
            }

            connection.Close();
        }
    }
}
