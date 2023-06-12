using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
//Using que fueron remplazados por privacidad

namespace NombreNoReal
{
    public class GeneradorCodigoService : IGeneradorCodigoService
    {

        public async Task<IList<TipoCodigosDeBarra>> GetTypesBarCode()
        {
          
            List<TipoCodigosDeBarra> result = new List<TipoCodigosDeBarra>();

            try
            {

                SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["xxxxxxxxxx"].ConnectionString);
                conn.Open();
                using (conn)
                {
                    SqlCommand command = new SqlCommand("GetTypesBarCodes", conn);
                    command.CommandType = CommandType.StoredProcedure;
                   
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        result.Add(new TipoCodigosDeBarra()
                        {
                            TIPO = (string)reader[0],
                            tipoValueJs = (string)reader[1],
                            tipoGS1 = (string)reader[2],
                            maxValue = (string)reader[3],
                            minValue = (string)reader[4],
                            nameJS = (string)reader[5]

                        });



                    }
                    reader.Close();
                    conn.Close();
                }
            }
            catch (Exception x)
            {
             
                throw new Exception(x.Message);
            }
            return result;
        }

        public async Task<int> SaveLogDownloadSymbols(string userId, string tipo)
        {
            try
            {

                SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["xxxxxxxxxxx"].ConnectionString);
                conn.Open();
                using (conn)
                {
                    SqlCommand command = new SqlCommand("spLogSymbologyDownloads", conn);
                    command.CommandType = CommandType.StoredProcedure;

            
                    command.Parameters.Add("@UserId", SqlDbType.NVarChar).Value = userId;
                    command.Parameters.Add("@Symbology", SqlDbType.Char).Value = tipo;
                    command.ExecuteNonQuery();


                    conn.Close();
                }
           
                return 1;
            }
            catch (Exception x)
            {
               
                throw new Exception(x.Message);
            }
        }
    }
}
