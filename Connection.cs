using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace AvaliacaoDotNetCsharp
{
     class Connection{

    private static string connString = "Server=localhost; Database=avaliacaoDesenvolvedorDotNet; Trusted_Connection=True;";
 
    private static SqlConnection conn = null;   
 
    public static SqlConnection connect(){
  
      conn = new SqlConnection(connString);
      try{
        conn.Open();
        return conn;
      }
      catch(SqlException sqle){
        conn = null;  
      }
      return null;
    }
 
    public static void disconnect(){
      if(conn != null){
        conn.Close();
      }
    }
  }
}
