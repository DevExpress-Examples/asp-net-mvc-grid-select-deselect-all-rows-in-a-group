using System.Data;
using System.Data.OleDb;
using System.Web.Configuration;

public class NorthwindDataProvider {
    public static DataTable GetProducts() {
        OleDbConnection connection = new OleDbConnection(WebConfigurationManager.ConnectionStrings["Northwind"].ConnectionString);
        OleDbCommand selectCommand = new OleDbCommand("SELECT * FROM Products ORDER BY ProductID", connection);
        OleDbDataAdapter da = new OleDbDataAdapter(selectCommand);
        DataSet ds = new DataSet();

        selectCommand.Connection = connection;
        da.Fill(ds, "Products");

        return ds.Tables["Products"];
    }
}
