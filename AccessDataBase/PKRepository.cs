using Model.Model.Entities;
using ProjectManager.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccessDataBase
{
    public class PKRepository : Repository, IRepository<PK>
    {
        public void Create(PK entity)
        {
            var connection = new OleDbConnection(ConnectionString);
            connection.Open();

            var insertQuery = "INSERT INTO tab_PK (X, Y, H, X1, Y1, H1, Az, X2, Y2, H2, PKType, MD, ReceiverloopSize, Offset, Profile," +
                "ControlPK, Remarks, fReserved, Lon, Lat) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";
            using (var cmd = new OleDbCommand(insertQuery, connection))
            {
                cmd.Parameters.AddWithValue("X", entity.X);
                cmd.Parameters.AddWithValue("Y", entity.Y);
                cmd.Parameters.AddWithValue("H", entity.H);
                cmd.Parameters.AddWithValue("X1", entity.X1);
                cmd.Parameters.AddWithValue("Y1", entity.Y1);
                cmd.Parameters.AddWithValue("H1", entity.H1);
                cmd.Parameters.AddWithValue("Az", entity.Az);
                cmd.Parameters.AddWithValue("X2", entity.X2);
                cmd.Parameters.AddWithValue("Y2", entity.Y2);
                cmd.Parameters.AddWithValue("H2", entity.H2);
                cmd.Parameters.AddWithValue("PKType", entity.PKType);
                cmd.Parameters.AddWithValue("MD", entity.MDID);
                cmd.Parameters.AddWithValue("ReceiverloopSize", entity.ReceiverLoopSize);
                cmd.Parameters.AddWithValue("Offset", entity.Offset);
                cmd.Parameters.AddWithValue("Profile", entity.Profile);
                cmd.Parameters.AddWithValue("ControlPK", entity.ControlPK);
                cmd.Parameters.AddWithValue("Remarks", entity.Remarks);
                cmd.Parameters.AddWithValue("fReserved", entity.FReserved);
                cmd.Parameters.AddWithValue("Lon", entity.Lon);
                cmd.Parameters.AddWithValue("Lat", entity.Lat);

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

        public void CreateKeepID(PK entity)
        {
            var connection = new OleDbConnection(ConnectionString);
            connection.Open();

            var insertQuery = "INSERT INTO tab_PK (PK, X, Y, H, X1, Y1, H1, Az, X2, Y2, H2, PKType, MD, ReceiverloopSize, Offset, Profile," +
                "ControlPK, Remarks, fReserved, Lon, Lat) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";
            using (var cmd = new OleDbCommand(insertQuery, connection))
            {
                cmd.Parameters.AddWithValue("PK", entity.ID);
                cmd.Parameters.AddWithValue("X", entity.X);
                cmd.Parameters.AddWithValue("Y", entity.Y);
                cmd.Parameters.AddWithValue("H", entity.H);
                cmd.Parameters.AddWithValue("X1", entity.X1);
                cmd.Parameters.AddWithValue("Y1", entity.Y1);
                cmd.Parameters.AddWithValue("H1", entity.H1);
                cmd.Parameters.AddWithValue("Az", entity.Az);
                cmd.Parameters.AddWithValue("X2", entity.X2);
                cmd.Parameters.AddWithValue("Y2", entity.Y2);
                cmd.Parameters.AddWithValue("H2", entity.H2);
                cmd.Parameters.AddWithValue("PKType", entity.PKType);
                cmd.Parameters.AddWithValue("MD", entity.MDID);
                cmd.Parameters.AddWithValue("ReceiverloopSize", entity.ReceiverLoopSize);
                cmd.Parameters.AddWithValue("Offset", entity.Offset);
                cmd.Parameters.AddWithValue("Profile", entity.Profile);
                cmd.Parameters.AddWithValue("ControlPK", entity.ControlPK);
                cmd.Parameters.AddWithValue("Remarks", entity.Remarks);
                cmd.Parameters.AddWithValue("fReserved", entity.FReserved);
                cmd.Parameters.AddWithValue("Lon", entity.Lon);
                cmd.Parameters.AddWithValue("Lat", entity.Lat);

                cmd.ExecuteNonQuery();
            }
            connection.Close();
        }

        public void Delete(int ID)
        {
            var connection = new OleDbConnection(ConnectionString);
            connection.Open();

            var query = "DELETE FROM tab_PK WHERE PK = ?";
            using (var cmd = new OleDbCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("PK", ID);
                cmd.ExecuteNonQuery();
            }

            connection.Close();
        }

        public PK Read(int ID)
        {
            var connection = new OleDbConnection(ConnectionString);
            connection.Open();

            var query = "SELECT PK, X, Y, H, X1, Y1, H1, Az, X2, Y2, H2, PKType, MD, ReceiverloopSize, Offset, Profile," +
                "ControlPK, Remarks, fReserved, Lon, Lat FROM tab_PK WHERE PK = ?";
            using (var cmd = new OleDbCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("PK", ID);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var PK = Convert.ToInt32(reader["PK"]);
                        var X = Convert.ToInt32(reader["X"]);
                        var Y = Convert.ToInt32(reader["Y"]);
                        var H = Convert.ToInt32(reader["H"]);
                        var X1 = Convert.ToInt32(reader["X1"]);
                        var Y1 = Convert.ToInt32(reader["Y1"]);
                        var H1 = Convert.ToInt32(reader["H1"]);
                        var Az = Convert.ToInt32(reader["Az"]);
                        var X2 = Convert.ToInt32(reader["X2"]);
                        var Y2 = Convert.ToInt32(reader["Y2"]);
                        var H2 = Convert.ToInt32(reader["H2"]);
                        var PKType = reader["PKType"].ToString();
                        var MDID = Convert.ToInt32(reader["MD"]);
                        var ReceiverLoopSize = Convert.ToInt32(reader["ReceiverloopSize"]);
                        var Offset = Convert.ToInt32(reader["Offset"]);
                        var Profile = reader["Profile"].ToString();
                        var ControlPK = bool.Parse(reader["ControlPK"].ToString());
                        var Remarks = reader["Remarks"].ToString();
                        var FReserved = reader["FReserved"].ToString();
                        var Lon = 0;
                        var Lat = 0;
                        try
                        {
                            Lon = Convert.ToInt32(reader["Lon"]);
                        }
                        catch { };
                        try
                        {
                            Lat = Convert.ToInt32(reader["Lat"]);
                        }
                        catch { };

                        return new PK()
                        {
                            ID = PK,
                            X = X,
                            Y = Y,
                            H = H,
                            X1 = X1,
                            Y1 = Y1,
                            H1 = H1,
                            Az = Az,
                            X2 = X2,
                            Y2 = Y2,
                            H2 = H2,
                            PKType = PKType,
                            MDID = MDID,
                            ReceiverLoopSize = ReceiverLoopSize,
                            Offset = Offset,
                            Profile = Profile,
                            ControlPK = ControlPK,
                            Remarks = Remarks,
                            FReserved = FReserved,
                            Lon = Lon,
                            Lat = Lat,
                        };
                    }
                }
            }

            connection.Close();

            return null;
        }

        public IEnumerable<PK> ReadAll()
        {
            var PKs = new List<PK>();

            var connection = new OleDbConnection(ConnectionString);
            connection.Open();

            var query = "SELECT PK, X, Y, H, X1, Y1, H1, Az, X2, Y2, H2, PKType, MD, ReceiverLoopSize, Offset, Profile," +
                "ControlPK, Remarks, fReserved, Lon, Lat FROM tab_PK";
            using (var cmd = new OleDbCommand(query, connection))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while(reader.Read())
                    {

                        var ID = Convert.ToInt32(reader["PK"]);
                        var X = Convert.ToInt32(reader["X"]);
                        var Y = Convert.ToInt32(reader["Y"]);
                        var H = Convert.ToInt32(reader["H"]);
                        var X1 = Convert.ToInt32(reader["X1"]);
                        var Y1 = Convert.ToInt32(reader["Y1"]);
                        var H1 = Convert.ToInt32(reader["H1"]);
                        var Az = Convert.ToInt32(reader["Az"]);
                        var X2 = Convert.ToInt32(reader["X2"]);
                        var Y2 = Convert.ToInt32(reader["Y2"]);
                        var H2 = Convert.ToInt32(reader["H2"]);
                        var PKType = reader["PKType"].ToString();
                        var MDID = Convert.ToInt32(reader["MD"]);
                        var ReceiverLoopSize = Convert.ToInt32(reader["ReceiverloopSize"]);
                        var Offset = Convert.ToInt32(reader["Offset"]);
                        var Profile = reader["Profile"].ToString();
                        var ControlPK = bool.Parse(reader["ControlPK"].ToString());
                        var Remarks = reader["Remarks"].ToString();
                        var FReserved = reader["FReserved"].ToString();
                        var Lon = 0;
                        var Lat = 0;
                        try
                        {
                            Lon = Convert.ToInt32(reader["Lon"]);
                        }
                        catch { };
                        try
                        {
                            Lat = Convert.ToInt32(reader["Lat"]);
                        }
                        catch {};

                        PKs.Add(new PK()
                        {
                            ID = ID,
                            X = X,
                            Y = Y,
                            H = H,
                            X1 = X1,
                            Y1 = Y1,
                            H1 = H1,
                            Az = Az,
                            X2 = X2,
                            Y2 = Y2,
                            H2 = H2,
                            PKType = PKType,
                            MDID = MDID,
                            ReceiverLoopSize = ReceiverLoopSize,
                            Offset = Offset,
                            Profile = Profile,
                            ControlPK = ControlPK,
                            Remarks = Remarks,
                            FReserved = FReserved,
                            Lon = Lon,
                            Lat = Lat,
                        });
                    }
                }
            }

            connection.Close();

            return PKs;
        }

        public void Update(PK entity)
        {
            var connection = new OleDbConnection(ConnectionString);
            connection.Open();

            var query = "UPDATE tab_PK SET X = ?, Y = ?, H = ?, X1 = ?, Y1 = ?, H1 = ?, Az = ?, X2 = ?, Y2 = ?, H2 = ?, PKType = ?, " +
                "MD = ?, ReceiverLoopSize = ?, Offset = ?, Profile = ?, ControlPK = ?, Remarks = ?, fReserved = ?, Lon = ?, Lat = ? " +
                "WHERE PK = ?";
            using (var cmd = new OleDbCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("X", entity.X);
                cmd.Parameters.AddWithValue("Y", entity.Y);
                cmd.Parameters.AddWithValue("H", entity.H);
                cmd.Parameters.AddWithValue("X1", entity.X1);
                cmd.Parameters.AddWithValue("Y1", entity.Y1);
                cmd.Parameters.AddWithValue("H1", entity.H1);
                cmd.Parameters.AddWithValue("Az", entity.Az);
                cmd.Parameters.AddWithValue("X2", entity.X2);
                cmd.Parameters.AddWithValue("Y2", entity.Y2);
                cmd.Parameters.AddWithValue("H2", entity.H2);
                cmd.Parameters.AddWithValue("PKType", entity.PKType);
                cmd.Parameters.AddWithValue("MD", entity.MDID);
                cmd.Parameters.AddWithValue("ReceiverloopSize", entity.ReceiverLoopSize);
                cmd.Parameters.AddWithValue("Offset", entity.Offset);
                cmd.Parameters.AddWithValue("Profile", entity.Profile);
                cmd.Parameters.AddWithValue("ControlPK", entity.ControlPK);
                cmd.Parameters.AddWithValue("Remarks", entity.Remarks);
                cmd.Parameters.AddWithValue("fReserved", entity.FReserved);
                cmd.Parameters.AddWithValue("Lon", entity.Lon);
                cmd.Parameters.AddWithValue("Lat", entity.Lat);
                cmd.Parameters.AddWithValue("PK", entity.ID);

                cmd.ExecuteNonQuery();
            }

            connection.Close();
        }
    }
}
