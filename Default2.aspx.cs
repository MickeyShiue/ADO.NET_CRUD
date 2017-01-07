using System;
using System.Data;
using System.Data.SqlClient;

using System.Web.Configuration;


public partial class Default2 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    public void SqlDataReader_Select()
    {
        using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["你的DB字串"].ConnectionString))
        {
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            SqlDataReader dr = null;
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT * FROM TEST WHERE ID=@ID";
            cmd.Parameters.AddWithValue("@ID", "只要是string類型都可以，變數，參數都好");
            dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                //自行發揮....把資料Load出來
            }
            cmd.Dispose();
        }
        //============================================================================
        //SqlDataReader 是一直跟資料庫保持連線，所以一個SqlDataReader
        //就占用一條SqlConnection，而這時候這條SqlConnection就會被占據
        //如果有其他的SqlDataReader要使用，必須先關閉SqlConnection
        //而SqlDataReader只能順向讀取，所以你若要做分頁功能就不能用SqlDataReader
        //然而相對效能上SqlDataReader會比DataTable 或者 DataSet好
        //============================================================================
    }

    public void DataTable_DataSet_Select()
    {
        using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["你的DB字串"].ConnectionString))
        {
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            SqlDataReader dr = null;
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();

            //這是一個查詢結果集，使用DataTable就可以了
            cmd.Connection = conn;
            cmd.CommandText = "SELECT * FROM TEST";
            DataTable dt = new DataTable();
            da.Fill(dt);


            //這是多個查詢結果集，必須要DataSet
            cmd.Connection = conn;
            cmd.CommandText = "SELECT * FROM TEST; SELECT * FROM TEST2";
            DataSet ds = new DataSet();
            da.Fill(ds);

            dt = ds.Tables[0]; //SELECT * FROM TEST
            dt = ds.Tables[1]; //SELECT * FROM TEST2
        }
        //============================================================================
        //以上是DataTable與DataSet的用法介紹，相信這個範例可以讓大家知道
        //什麼時候要用DataTable，什麼時候要用DataSet吧
        //那麼基本上如果沒有要做分頁就用DataReader
        //如果說你最後查詢的資料要做DataBind()的動作就要用使用DataTable或DataSet
        //DataTable跟DataSet是離線存取，也就是查完結果，就跟資料庫連結中斷
        //把這些查詢的資料放在記憶體當中，方便你使用
        //所以你的資料庫可一次最多能支援多少SqlConnection
        //跟一台WebServer的IIS執行緒一樣，都有最高上限的限制
        //最後該如何使用，就取決於各位吧
        //============================================================================
    }

    public void Insert_Delete_Update()
    {
        using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["你的DB字串"].ConnectionString))
        {
            if (conn.State == ConnectionState.Closed)
                conn.Open();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "INSERT INTO TEST(欄位A，欄位B)VALUES(@欄位A，@欄位B)";
            cmd.Parameters.AddWithValue("@欄位A", "請放要寫入欄位A的值");
            cmd.Parameters.AddWithValue("@欄位B", "請放要寫入欄位B的值");

            int Count = cmd.ExecuteNonQuery();
            //Count 若-1 則表示沒有資料寫入成功
            //Count 1~xxxx表示你寫入的比數
            //以上 新增 刪除 修改 都這樣寫即可
            //因為新珊修不須要回傳結果，只要知道有幾筆資料更動即可
            //ExecuteNonQuery() 就是執行你寫的SQL
        }
    }
}