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
    public class ProfPKRepository : Repository, IRepository<ProfPK>
    {
        public void Create(ProfPK entity)
        {
            var connection = new OleDbConnection(ConnectionString);
            connection.Open();

            var insertQuery = "INSERT INTO prof_PK (PK, MD, Station, ControlPK, NumberRecord, NumberChannel, NumberADC, NumberLoop, NameOutFile, " +
                "Date, TimeStart, TimeFinish, Operator, Rejim, Gain_Channel, DeltaT, NumberOfMeasuredPoints, NumberOfImpulses, TimeOfCharge, " +
                "ReceiverNumberCoins, Current, Shift, Comment, Period, CurrentADCQuant, ReceiverLoopSize, LoopShunt, MDShunt, SensRole, SensCoeff) " +
                "VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?,)";
            using (var cmd = new OleDbCommand(insertQuery, connection))
            {
                cmd.Parameters.AddWithValue("PK", entity.PKID);
                cmd.Parameters.AddWithValue("MD", entity.MDID);
                cmd.Parameters.AddWithValue("Station", entity.Station);
                cmd.Parameters.AddWithValue("ControlPK", entity.ControlPK);
                cmd.Parameters.AddWithValue("NumberRecord", entity.NumberRecord);
                cmd.Parameters.AddWithValue("NumberChannel", entity.NumberChannel);
                cmd.Parameters.AddWithValue("NumberADC", entity.NumberADC);
                cmd.Parameters.AddWithValue("NumberLoop", entity.NumberLoop);
                cmd.Parameters.AddWithValue("NameOutFile", entity.NameOutFile);
                cmd.Parameters.AddWithValue("Date", entity.Date);
                cmd.Parameters.AddWithValue("TimeStart", entity.TimeStart);
                cmd.Parameters.AddWithValue("TimeFinish", entity.TimeFinish);
                cmd.Parameters.AddWithValue("Operator", entity.Operator);
                cmd.Parameters.AddWithValue("Rejim", entity.Rejim);
                cmd.Parameters.AddWithValue("Gain_Channel", entity.GainChannel);
                cmd.Parameters.AddWithValue("DeltaT", entity.DeltaT);
                cmd.Parameters.AddWithValue("NumberOfMeasuredPoints", entity.NumberOfMeasuredPoints);
                cmd.Parameters.AddWithValue("NumberOfImpulses", entity.NumberOfImpulses);
                cmd.Parameters.AddWithValue("TimeOfCharge", entity.TimeOfCharge);
                cmd.Parameters.AddWithValue("ReceiverNumberCoins", entity.ReceiverNumberCoins);
                cmd.Parameters.AddWithValue("Current", entity.Current);
                cmd.Parameters.AddWithValue("Shift", entity.Shift);
                cmd.Parameters.AddWithValue("Comment", entity.Comment);
                cmd.Parameters.AddWithValue("Period", entity.Period);
                cmd.Parameters.AddWithValue("CurrentADCQuant", entity.CurrentADCQuant);
                cmd.Parameters.AddWithValue("ReceiverLoopSize", entity.ReceiverLoopSize);
                cmd.Parameters.AddWithValue("LoopShunt", entity.LoopShunt);
                cmd.Parameters.AddWithValue("MDShunt", entity.MDShunt);
                cmd.Parameters.AddWithValue("SensRole", entity.SensRole);
                cmd.Parameters.AddWithValue("SensCoeff", entity.SensCoeff);

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

        public void CreateKeepID(ProfPK entity)
        {
            var connection = new OleDbConnection(ConnectionString);
            connection.Open();

            var insertQuery = 
                "INSERT INTO prof_PK ([PK], [MD], [Station], [ControlPK], [NumberRecord], [NumberChannel], " +
                "[NumberADC], [NumberLoop], [NameOutFile], [Date], [TimeStart], [TimeFinish], [Operator], [Rejim], " +
                "[Gain_Channel], [DeltaT], [NumberOfMeasuredPoints], [NumberOfImpulses], [TimeOfCharge], " +
                "[ReceiverNumberCoins], [Current], [Shift], [Comment], [Period], [CurrentADCQuant], [ReceiverLoopSize], " +
                "[LoopShunt], [MDShunt], [SensRole], [SensCoeff]) " +
                "VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";
            using (var cmd = new OleDbCommand(insertQuery, connection))
            {
                cmd.Parameters.AddWithValue("PK", entity.PKID);
                cmd.Parameters.AddWithValue("MD", entity.MDID);
                cmd.Parameters.AddWithValue("Station", entity.Station);
                cmd.Parameters.AddWithValue("ControlPK", entity.ControlPK);
                cmd.Parameters.AddWithValue("NumberRecord", entity.NumberRecord);
                cmd.Parameters.AddWithValue("NumberChannel", entity.NumberChannel);
                cmd.Parameters.AddWithValue("NumberADC", entity.NumberADC);
                cmd.Parameters.AddWithValue("NumberLoop", entity.NumberLoop);
                cmd.Parameters.AddWithValue("NameOutFile", entity.NameOutFile);
                cmd.Parameters.AddWithValue("Date", entity.Date);
                cmd.Parameters.AddWithValue("TimeStart", entity.TimeStart);
                cmd.Parameters.AddWithValue("TimeFinish", entity.TimeFinish);
                cmd.Parameters.AddWithValue("Operator", entity.Operator);
                cmd.Parameters.AddWithValue("Rejim", entity.Rejim);
                cmd.Parameters.AddWithValue("Gain_Channel", entity.GainChannel);
                cmd.Parameters.AddWithValue("DeltaT", entity.DeltaT);
                cmd.Parameters.AddWithValue("NumberOfMeasuredPoints", entity.NumberOfMeasuredPoints);
                cmd.Parameters.AddWithValue("NumberOfImpulses", entity.NumberOfImpulses);
                cmd.Parameters.AddWithValue("TimeOfCharge", entity.TimeOfCharge);
                cmd.Parameters.AddWithValue("ReceiverNumberCoins", entity.ReceiverNumberCoins);
                cmd.Parameters.AddWithValue("Current", entity.Current);
                cmd.Parameters.AddWithValue("Shift", entity.Shift);
                cmd.Parameters.AddWithValue("Comment", entity.Comment);
                cmd.Parameters.AddWithValue("Period", entity.Period);
                cmd.Parameters.AddWithValue("CurrentADCQuant", entity.CurrentADCQuant);
                cmd.Parameters.AddWithValue("ReceiverLoopSize", entity.ReceiverLoopSize);
                cmd.Parameters.AddWithValue("LoopShunt", entity.LoopShunt);
                cmd.Parameters.AddWithValue("MDShunt", entity.MDShunt);
                cmd.Parameters.AddWithValue("SensRole", entity.SensRole);
                cmd.Parameters.AddWithValue("SensCoeff", entity.SensCoeff);

                cmd.ExecuteNonQuery();
            }

            connection.Close();
        }

        public void Delete(int ID)
        {
            var connection = new OleDbConnection(ConnectionString);
            connection.Open();

            var query = "DELETE FROM prof_PK WHERE RecNo = ?";
            using (var cmd = new OleDbCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("RecNo", ID);
                cmd.ExecuteNonQuery();
            }

            connection.Close();
        }

        public ProfPK Read(int ID)
        {
            var connection = new OleDbConnection(ConnectionString);
            connection.Open();

            var query = "SELECT RecNo, PK, MD, Station, ControlPK, NumberRecord, NumberChannel, NumberADC, NumberLoop, NameOutFile, " +
                "[Date], TimeStart, TimeFinish, Operator, Rejim, Gain_Channel, DeltaT, NumberOfMeasuredPoints, NumberOfImpulses, TimeOfCharge, " +
                "ReceiverNumberCoins, [Current], [Shift], Comment, Period, CurrentADCQuant, ReceiverLoopSize, LoopShunt, MDShunt, SensRole, SensCoeff " +
                "FROM prof_PK WHERE RecNo = ?";
            using (var cmd = new OleDbCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("RecNo", ID);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new ProfPK()
                        {
                            ID = Convert.ToInt32(reader["RecNo"]),
                            PKID = Convert.ToInt32(reader["PK"]),
                            MDID = Convert.ToInt32(reader["MD"]),
                            Station = Convert.ToInt32(reader["Station"]),
                            ControlPK = bool.Parse(reader["DType"].ToString()),
                            NumberRecord = Convert.ToInt32(reader["NumberRecord"]),
                            NumberChannel = Convert.ToInt32(reader["NumberChannel"]),
                            NumberADC = Convert.ToInt32(reader["NumberADC"]),
                            NumberLoop = Convert.ToInt32(reader["NumberLoop"]),
                            NameOutFile = reader["NameOutFile"].ToString(),
                            Date = Convert.ToDateTime(reader["Date"]),
                            TimeStart = Convert.ToDateTime(reader["TimeStart"]),
                            TimeFinish = Convert.ToDateTime(reader["TimeFinish"]),
                            Operator = reader["Operator"].ToString(),
                            Rejim = reader["Rejim"].ToString(),
                            GainChannel = Convert.ToInt32(reader["Gain_Channel"]),
                            DeltaT = Convert.ToInt32(reader["DeltaT"]),
                            NumberOfMeasuredPoints = Convert.ToInt32(reader["NumberOfMeasuredPoints"]),
                            NumberOfImpulses = Convert.ToInt32(reader["NumberOfImpulses"]),
                            TimeOfCharge = Convert.ToInt32(reader["TimeOfCharge"]),
                            ReceiverNumberCoins = Convert.ToInt32(reader["ReceiverNumberCoins"]),
                            Current = Convert.ToInt32(reader["Current"]),
                            Shift = Convert.ToInt32(reader["Shift"]),
                            Comment = reader["Comment"].ToString(),
                            Period = Convert.ToInt32(reader["Period"]),
                            CurrentADCQuant = Convert.ToInt32(reader["CurrendADCQuant"]),
                            ReceiverLoopSize = Convert.ToInt32(reader["ReceiverLoopSize"]),
                            LoopShunt = Convert.ToInt32(reader["LoopShunt"]),
                            SensRole = Convert.ToInt32(reader["SensRole"]),
                            MDShunt = Convert.ToInt32(reader["MDShunt"])
                        };
                    }
                }
            }

            connection.Close();

            return null;
        }

        public IEnumerable<ProfPK> ReadAll()
        {
            var profPKs = new List<ProfPK>();

            var connection = new OleDbConnection(ConnectionString);
            connection.Open();

            var query = "SELECT RecNo, PK, MD, Station, ControlPK, NumberRecord, NumberChannel, NumberADC, NumberLoop, NameOutFile, " +
                "[Date], TimeStart, TimeFinish, Operator, Rejim, Gain_Channel, DeltaT, NumberOfMeasuredPoints, NumberOfImpulses, TimeOfCharge, " +
                "ReceiverNumberCoins, [Current], [Shift], Comment, Period, CurrentADCQuant, ReceiverLoopSize, LoopShunt, MDShunt, SensRole, SensCoeff " +
                "FROM prof_PK";
            using (var cmd = new OleDbCommand(query, connection))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        profPKs.Add(new ProfPK()
                        {

                            ID = Convert.ToInt32(reader["RecNo"]),
                            PKID = Convert.ToInt32(reader["PK"]),
                            MDID = Convert.ToInt32(reader["MD"]),
                            Station = Convert.ToInt32(reader["Station"]),
                            ControlPK = bool.Parse(reader["ControlPK"].ToString()),
                            NumberRecord = Convert.ToInt32(reader["NumberRecord"]),
                            NumberChannel = Convert.ToInt32(reader["NumberChannel"]),
                            NumberADC = Convert.ToInt32(reader["NumberADC"]),
                            NumberLoop = Convert.ToInt32(reader["NumberLoop"]),
                            NameOutFile = reader["NameOutFile"].ToString(),
                            Date = Convert.ToDateTime(reader["Date"]),
                            TimeStart = Convert.ToDateTime(reader["TimeStart"]),
                            TimeFinish = Convert.ToDateTime(reader["TimeFinish"]),
                            Operator = reader["Operator"].ToString(),
                            Rejim = reader["Rejim"].ToString(),
                            GainChannel = Convert.ToInt32(reader["Gain_Channel"]),
                            DeltaT = Convert.ToInt32(reader["DeltaT"]),
                            NumberOfMeasuredPoints = Convert.ToInt32(reader["NumberOfMeasuredPoints"]),
                            NumberOfImpulses = Convert.ToInt32(reader["NumberOfImpulses"]),
                            TimeOfCharge = Convert.ToInt32(reader["TimeOfCharge"]),
                            ReceiverNumberCoins = Convert.ToInt32(reader["ReceiverNumberCoins"]),
                            Current = Convert.ToInt32(reader["Current"]),
                            Shift = Convert.ToInt32(reader["Shift"]),
                            Comment = reader["Comment"].ToString(),
                            Period = Convert.ToInt32(reader["Period"]),
                            CurrentADCQuant = Convert.ToInt32(reader["CurrentADCQuant"]),
                            ReceiverLoopSize = Convert.ToInt32(reader["ReceiverLoopSize"]),
                            LoopShunt = Convert.ToInt32(reader["LoopShunt"]),
                            SensRole = reader["SensRole"] == DBNull.Value ? 0 : Convert.ToInt32(reader["SensRole"]),
                            MDShunt = reader["SensRole"] == DBNull.Value ? 0 : Convert.ToInt32(reader["MDShunt"])
                        });
                    }
                }
            }

            connection.Close();

            return profPKs;
        }

        public void Update(ProfPK entity)
        {
            throw new NotImplementedException();
        }
    }
}
