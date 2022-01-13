using MIS_Summary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MIS_Summary.Controllers
{
    public class SummaryMethods
    {
        public static List<View_Summary> SelectSummaryListSQL(string DocumentNo, string Mat_Code, string Mat_Name, string ApplyStartDate, string ApplyEndDate)
        {
            C6Entities db = new C6Entities();
            string sql = string.Format("select * FROM View_Summary where (Serial_Num+'|'+OA_number+'|'+PO+'|'+Fixed_number) like '%{0}%' and Mat_Code like '%{1}%' and Mat_Name like '%{2}%' and ApplyDate between '{3}' and '{4}'", DocumentNo, Mat_Code, Mat_Name, "2020-10-10", "2021-12-16");
            //string sql = @"select * FROM View_Summary where (Serial_Num+'|'+OA_number+'|'+PO+'|'+Fixed_number) like '%"+DocumentNo+"%' and Mat_Code like '%"+ Mat_Code + "%' and Mat_Name like '%"+ Mat_Name + "%' and ApplyDate between '"+ ApplyStartDate + "' and '"+ ApplyEndDate + "'";
            return db.Database.SqlQuery<View_Summary>(sql, new object[0]).ToList<View_Summary>();
        }
    }
}